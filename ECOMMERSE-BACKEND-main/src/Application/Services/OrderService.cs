using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Models.Response;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IOrderNotificationRepository _orderNotificationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISendEmailService _sendEmailService;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository, ISendEmailService sendEmailService, IOrderProductRepository orderProductRepository, IOrderNotificationRepository orderNotificationRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _sendEmailService = sendEmailService;
            _orderProductRepository = orderProductRepository;
            _orderNotificationRepository = orderNotificationRepository;
        }

        public OrderWhitPriceResponse GetOrderById(int id)
        {
            OrderDto orderDto = OrderDto.ToDto(_orderRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro la orden"));

            OrderWhitPriceResponse orderWhitPrices = new OrderWhitPriceResponse();
            orderWhitPrices.Id = id;
            orderWhitPrices.PaymentMethod = orderDto.PaymentMethod;
            orderWhitPrices.StatusOrder = orderDto.StatusOrder;
            orderWhitPrices.Customer = orderDto.Customer;
            orderWhitPrices.Seller = orderDto.Seller;

            foreach (var item in orderDto.ProductsInOrder)
            {
                OrderProductWhitPriceResponse orderProductWhitPrice = new OrderProductWhitPriceResponse();
                orderProductWhitPrice.OrderProductId = item.OrderProductId;
                orderProductWhitPrice.ProductId = item.ProductId;
                orderProductWhitPrice.Name = item.Name;
                orderProductWhitPrice.Description = item.Description;
                orderProductWhitPrice.Stock = item.Stock;
                orderProductWhitPrice.Quantity = item.Quantity;
                orderProductWhitPrice.Price = item.Price;
                orderProductWhitPrice.TotalPerPrice = orderProductWhitPrice.Quantity * orderProductWhitPrice.Price;

                orderWhitPrices.TotalOrder += orderProductWhitPrice.TotalPerPrice;
                orderWhitPrices.ProductsInOrder.Add(orderProductWhitPrice);
            }
            return orderWhitPrices;
        }

        public ICollection<OrderDto> GetOrdersByCustomer(int customerId)
        {
            var customer = _userRepository.GetByIdAsync(customerId).Result ?? throw new Exception("No se encontro el usuario");
            if (customer.UserType != UserType.Customer)
            {
                throw new Exception("El id ingresado no es de tipo cliente");
            }
            List<OrderDto> orders = OrderDto.ToList(_orderRepository.OrdersByCustomer(customerId).Result);
            if (orders.Count == 0)
            {
                throw new Exception("No se encontraron las ordenes del cliente");
            }

            return orders;  
        }

        public ICollection<OrderDto> GetOrdersBySeller(int sellerId)
        {
            var seller = _userRepository.GetByIdAsync(sellerId).Result ?? throw new Exception("No se encontro el usuario");
            if (seller.UserType != UserType.Seller)
            {
                throw new Exception("El id ingresado no es de tipo vendedor");
            }
            List<OrderDto> orders = OrderDto.ToList(_orderRepository.OrdersBySellers(sellerId).Result);
            if (orders.Count == 0)
            {
                throw new Exception("No se encontraron las ordenes del vendedor");
            }
            return orders;
        }

        public List<OrderWhitPriceResponse> GetAllOrders()
        {
            var ListorderDto = OrderDto.ToList(_orderRepository.GetAllOrdersAsync().Result ?? throw new Exception("No se encontro la orden"));

            List<OrderWhitPriceResponse> listOrderWhitPriceResponse = new List<OrderWhitPriceResponse>();

            foreach (var orderDto in ListorderDto)
            {
                OrderWhitPriceResponse orderWhitPrices = new OrderWhitPriceResponse();
                orderWhitPrices.Id = orderDto.Id;
                orderWhitPrices.PaymentMethod = orderDto.PaymentMethod;
                orderWhitPrices.StatusOrder = orderDto.StatusOrder;
                orderWhitPrices.Customer = orderDto.Customer;
                orderWhitPrices.Seller = orderDto.Seller;

                foreach (var item in orderDto.ProductsInOrder)
                {
                    OrderProductWhitPriceResponse orderProductWhitPrice = new OrderProductWhitPriceResponse();
                    orderProductWhitPrice.OrderProductId = item.OrderProductId;
                    orderProductWhitPrice.ProductId = item.ProductId;
                    orderProductWhitPrice.Name = item.Name;
                    orderProductWhitPrice.Description = item.Description;
                    orderProductWhitPrice.Stock = item.Stock;
                    orderProductWhitPrice.Quantity = item.Quantity;
                    orderProductWhitPrice.Price = item.Price;
                    orderProductWhitPrice.TotalPerPrice = orderProductWhitPrice.Quantity * orderProductWhitPrice.Price;

                    orderWhitPrices.TotalOrder += orderProductWhitPrice.TotalPerPrice;
                    orderWhitPrices.ProductsInOrder.Add(orderProductWhitPrice);
                }

                listOrderWhitPriceResponse.Add(orderWhitPrices);
            }

            return listOrderWhitPriceResponse;
        }

        public OrderDto CreateOrder(OrderCreateRequest order)
        {
            if (order.CustomerId <= 0)
            {
                throw new Exception("El id de cliente invalido");
            }

            if (order.SellerId <= 0)
            {
                throw new Exception("El id de vendedor invalido");
            }

            var customer = _userRepository.GetByIdAsync(order.CustomerId).Result ?? throw new Exception("No se encontro el usuario");
            var seller = _userRepository.GetByIdAsync(order.SellerId).Result ?? throw new Exception("No se encontro el usuario");

            if (customer.UserType != UserType.Customer)
            {
                throw new Exception("El id ingresado no es de tipo cliente");
            }

            if (seller.UserType != UserType.Seller)
            {
                throw new Exception("El id ingresado no es de tipo vendedor");
            }

            if (customer.Avaible == false)
            {
                throw new Exception("El usuario cliente se encuentra deshabilitado");
            }

            if (seller.Avaible == false)
            {
                throw new Exception("El usuario vendedor se encuentra deshabilitado");
            }

            if (!Enum.IsDefined(typeof(PaymentMethod), order.PaymentMethod))
            {
                throw new Exception("El metodo de pago no es valido");
            }

            foreach (var item in order.ProductsInOrder)
            {
                if (item.Id <= 0)
                {
                    throw new Exception("El id del producto es invalido");
                }

                var productException = _productRepository.GetByIdAsync(item.Id).Result ?? throw new Exception($"No se encontro el producto con id {item.Id}");

                if (item.Quantity <= 0)
                {
                    throw new Exception($"No se permite cantidades menores a 0. Por favor revisar el producto {productException.Id} : {productException.Name}");
                }

                if (productException.ProductAvaible == false)
                {
                    throw new Exception($"El producto {productException.Id} : {productException.Name} se encuentra deshabilitado, por favor seleccione otro producto");
                }
            }

            List<OrderProduct> orderProductList = new List<OrderProduct>();

            var newOrder = new Order(customer, seller)
            {
                PaymentMethod = order.PaymentMethod
            };

            string messageOrderNotification = "";
            int itemMessageOrder = 0;
            decimal pricePerProduct = 0m;

            _orderRepository.AddAsync(newOrder).Wait();

            foreach (ProductsInOrderDto productInOrder in order.ProductsInOrder)
            {

                var product = _productRepository.GetByIdAsync(productInOrder.Id).Result ?? throw new Exception("No se encontro el producto");
                pricePerProduct = 0m;

                if (product.Stock < productInOrder.Quantity) //Endpoint No ABMC
                {
                    throw new Exception($"El producto {product.Id}:  {product.Name} no posee el stock suficiente. Su stock actual es {product.Stock}");
                }

                product.Stock -= productInOrder.Quantity;

                OrderProduct orderProduct = new OrderProduct();

                orderProduct.Order = newOrder;
                orderProduct.Product = product;
                orderProduct.Quantity = productInOrder.Quantity;

                pricePerProduct = orderProduct.Quantity * product.Price;
                itemMessageOrder += 1;
                messageOrderNotification += $" {itemMessageOrder} - Producto {product.Id} {product.Name} ${pricePerProduct} . ";

                //Da de alta en la tabla pivote para relacionar los productos con la orden
                _orderProductRepository.AddAsync(orderProduct).Wait();
                orderProductList.Add(orderProduct);
            }

            newOrder.ProductsInOrder = orderProductList;
            _orderRepository.UpdateAsync(newOrder);

            OrderNotification orderNotificationCustomer = new OrderNotification();
            orderNotificationCustomer.Order = newOrder;
            orderNotificationCustomer.User = customer;
            orderNotificationCustomer.Message = $"Su orden creo exitosamente, su vendedor asignado es {seller.Name} {seller.LastName}. Resumen de orden: {messageOrderNotification}";
            _orderNotificationRepository.AddAsync(orderNotificationCustomer).Wait();

            _sendEmailService.SendEmail(customer.Email, $"¡Su orden ha sido creada exitosamente! con el #{newOrder.Id}", $"Su vendedor asignado es {seller.Name} {seller.LastName}. Resumen de orden: {messageOrderNotification}");

            OrderNotification orderNotificationSeller = new OrderNotification();
            orderNotificationSeller.Order = newOrder;
            orderNotificationSeller.User = seller;
            orderNotificationSeller.Message = $"Tiene una nueva orden del cliente {customer.Name} {customer.LastName}";
            _orderNotificationRepository.AddAsync(orderNotificationSeller).Wait();

            _sendEmailService.SendEmail(seller.Email, $"Nueva orden #{newOrder.Id}", $"Tiene una nueva orden del cliente {customer.Name} {customer.LastName}");
           
            return OrderDto.ToDto(newOrder);
        }

        public string CheckStockProduct(int productId)
        {
            var product = _productRepository.GetByIdAsync(productId).Result ?? throw new Exception("No se encontro el producto");
            if (product.Stock <= 0)
            {
                throw new Exception($"El producto ingresado {product.Name} no posee stock");
            }

            return $"El producto ingresado {product.Name} posee {product.Stock} en stock";
        }


        public async Task UpdateOrderProduct(int orderId, ChangeOrderRequest changeOrder)
        {

            if (orderId <= 0)
            {
                throw new Exception("El id de la orden es invalido");
            }

            if (!Enum.IsDefined(typeof(PaymentMethod), changeOrder.PaymentMethod))
            {
                throw new Exception("El metodo de pago no es valido");
            }

            PaymentMethod paymentMethod = (PaymentMethod)changeOrder.StatusOrder; 

            if (!Enum.IsDefined(typeof(StatusOrder), changeOrder.StatusOrder))
            {
                throw new Exception("El estado no es válido");
            }

            StatusOrder statusOrder = (StatusOrder)changeOrder.StatusOrder;

            Order order = _orderRepository.GetByIdAsync(orderId).Result ?? throw new Exception("No se encontro la orden");

            if (order.StatusOrder == StatusOrder.InProgress)
            {
                order.PaymentMethod = paymentMethod;
                order.StatusOrder = statusOrder;
                await _orderRepository.UpdateAsync(order);

                ICollection<OrderProduct> productListDataBaseRemove = order.ProductsInOrder.ToList();

                ICollection<ProductsInOrderDto> productListProductOrderCreate = changeOrder.ProductsInOrder.ToList();

                foreach (var item in order.ProductsInOrder.ToList())
                {
                    foreach (var itemProduct in changeOrder.ProductsInOrder.ToList())
                    {
                        if (itemProduct.Id <= 0)
                        {
                            throw new Exception("El id del producto es invalido");
                        }

                        if (itemProduct.Quantity <= 0)
                        {
                            var productException = _productRepository.GetByIdAsync(item.Product.Id).Result ?? throw new Exception("No se encontro el producto");
                            throw new Exception($"No se permite cantidades menores a 0. Por favor revisar el producto {productException.Id} : {productException.Name}");
                        }

                        if (item.Product.Id == itemProduct.Id)
                        {
                            var product = _productRepository.GetByIdAsync(item.Product.Id).Result ?? throw new Exception("No se encontro el producto");

                            var newQuantity = item.Quantity - itemProduct.Quantity;

                            if (newQuantity < 0)
                            {
                                var newQuantityCompare = newQuantity * (-1);
                                if (product.Stock < newQuantityCompare)
                                {
                                    throw new Exception($"El producto {product.Id}:  {product.Name} no posee el stock suficiente. Su stock actual es {product.Stock}, usted ya habia solicitado {item.Quantity}");
                                }
                            }
                            //Modificamos el stock en Productos
                            product.Stock += newQuantity;

                            //Modificamos en OrderProducts el registro
                            item.Quantity = itemProduct.Quantity;
                            await _orderProductRepository.UpdateAsync(item);

                            //Eliminamos el regitro de las listas, indicando que no va a ser un create ni un delete
                            productListDataBaseRemove.Remove(item);
                            productListProductOrderCreate.Remove(itemProduct);
                            break;
                        }
                    }
                }

                // Delete
                foreach (var itemRemove in productListDataBaseRemove)
                {
                    var product = _productRepository.GetByIdAsync(itemRemove.Product.Id).Result ?? throw new Exception("No se encontro el producto");
                    product.Stock += itemRemove.Quantity;
                    await _orderProductRepository.DeleteAsync(itemRemove);
                }

                // Create
                foreach (var itemCreate in productListProductOrderCreate)
                {
                    var product = _productRepository.GetByIdAsync(itemCreate.Id).Result ?? throw new Exception("No se encontro el producto");
                    product.Stock -= itemCreate.Quantity;
                    OrderProduct orderProduct = new OrderProduct
                    {
                        Order = order,
                        Product = product,
                        Quantity = itemCreate.Quantity
                    };
                    await _orderProductRepository.AddAsync(orderProduct);
                }
            }
            else
            {
                throw new Exception("No es posibile realizar la modificación, debido a que la order no se encuentra en progreso");
            }
        }

        public void ChangeOrderStatus(int orderId, ChangeStatusOrderRequest changeOrderStatus)
        {
            if (!Enum.IsDefined(typeof(StatusOrder), changeOrderStatus.StatusOrder))
            {
                throw new Exception("El estado no es válido");
            }

            StatusOrder statusOrder = (StatusOrder)changeOrderStatus.StatusOrder;

            Order order = _orderRepository.GetByIdAsync(orderId).Result ?? throw new Exception("No se encontro la orden");

            if (order.StatusOrder == statusOrder)
            {
                throw new Exception("No es posible realizar esta acción, debido a que la orden ya se encuentra en este estado");
            }
            else
            {
                _orderRepository.ChangeStatusOrder(orderId, statusOrder);

                OrderNotification orderNotificationCustomer = new OrderNotification();
                orderNotificationCustomer.Order = order;
                orderNotificationCustomer.User = order.Customer;

                OrderNotification orderNotificationSeller = new OrderNotification();
                orderNotificationSeller.Order = order;
                orderNotificationSeller.User = order.Seller;

                switch (statusOrder)
                {
                    case StatusOrder.Finished:
                        orderNotificationCustomer.Message = "Su orden ha sido finalizada con exito";
                        orderNotificationSeller.Message = $"La orden asingada a {order.Customer.Name} {order.Customer.LastName} ha sido finalizada con exito";
                        break;

                    case StatusOrder.InProgress:
                        orderNotificationCustomer.Message = "Su orden se encuentra en progreso";
                        orderNotificationSeller.Message = $"La orden asingada a {order.Customer.Name} {order.Customer.LastName} se encuentra en progreso";
                        break;

                    case StatusOrder.Cancelled:
                        orderNotificationCustomer.Message = "Su orden se cancelo exitosamente";
                        orderNotificationSeller.Message = $"La orden asingada a {order.Customer.Name} {order.Customer.LastName} ha sido cancelada con exito";
                        break;

                    case StatusOrder.OnHold:
                        orderNotificationCustomer.Message = "Su orden se encuentra en espera, por favor comunicarse con la sucursal";
                        orderNotificationSeller.Message = $"La orden asingada a {order.Customer.Name} {order.Customer.LastName} se encuentra en estado de pausa ";
                        break;
                }

                _sendEmailService.SendEmail(order.Seller.Email, $"Modificaciones en la orden #{order.Id}", orderNotificationSeller.Message);
                _sendEmailService.SendEmail(order.Customer.Email, $"Modificaciones en la orden #{order.Id}", orderNotificationCustomer.Message);
                _orderNotificationRepository.AddAsync(orderNotificationCustomer).Wait();
                _orderNotificationRepository.AddAsync(orderNotificationSeller).Wait();
            }

        }

        public async Task<string> DeleteOrder(int id)
        {
            Order order = await _orderRepository.GetByIdAsync(id) ?? throw new Exception("No se encontró la orden");
            if (order.ProductsInOrder.Count > 0)
            {
                List<OrderProduct> orders = await _orderProductRepository.GetListOrdersByOrderId(id);
                foreach (var itemRemove in orders)
                {
                    var product = await _productRepository.GetByIdAsync(itemRemove.Product.Id) ?? throw new Exception("No se encontró el producto");
                    product.Stock += itemRemove.Quantity;
                    await _productRepository.UpdateAsync(product);
                    await _orderProductRepository.DeleteAsync(itemRemove);
                }
            }

            List<OrderNotification> notifications = await _orderNotificationRepository.GetListNotificationsByOrderId(id);
            if (notifications.Count > 0)
            {
                foreach (var itemRemove in notifications)
                {
                    await _orderNotificationRepository.DeleteAsync(itemRemove);
                }
            }
            await _orderRepository.DeleteAsync(order);
            return $"La orden {id} ha sido eliminada con éxito.";
        }
    }
}
