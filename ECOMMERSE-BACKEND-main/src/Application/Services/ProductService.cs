using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        public ProductService(IProductRepository productRepository, IOrderProductRepository orderProductRepository)
        {
            _productRepository = productRepository;
            _orderProductRepository = orderProductRepository;  
        }

        public ICollection<ProductDto> GetAllProducts()
        {
            var products = ProductDto.ToList(_productRepository.ListAsync().Result ?? throw new Exception("No se encontraron productos"));
            return products;
        }

        public ICollection<ProductDto> GetProductsAvaible()
        {
            var products = ProductDto.ToList(_productRepository.GetListAvaible().Result ?? throw new Exception("No se encontraron productos"));
            return products;
        }

        public ProductDto GetProductById(int id)
        {
            ProductDto productDto = ProductDto.ToDto(_productRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el producto"));
            return productDto;
        }
        public ICollection<ProductDto> GetProductByName(string name)
        {
            List<ProductDto> product = ProductDto.ToList(_productRepository.GetByName(name).Result ?? throw new Exception("No se encontro el producto"));
            return product;
        }

        public ProductDto CreateProduct(ProductCreateRequest product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Ingrese el nombre del producto", nameof(product.Name));
            }

            if (product.Price <= 0)
            {
                throw new Exception("El precio del producto no es valido");
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                throw new ArgumentException("Ingrese una descripción para el producto", nameof(product.Description));
            }

            if (product.Stock < 0)
            {
                throw new Exception("El stock no puede ser menor a 0");
            }

            return ProductDto.ToDto(_productRepository.AddAsync(ProductCreateRequest.ToEntity(product)).Result);
        }

        public async Task UpdateProduct(int id, ProductChangeRequest product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Ingrese el nombre del producto", nameof(product.Name));
            }

            if (product.Price <= 0)
            {
                throw new Exception("El precio del producto no es valido");
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                throw new ArgumentException("Ingrese una descripción para el producto", nameof(product.Description));
            }

            ProductDto existingProduct = GetProductById(id) ?? throw new KeyNotFoundException("No se encontró el producto");
           
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            //ProductChangeRequest productWithId = new ProductChangeRequest();

            //productWithId.Name = product.Name;
            //productWithId.Price = product.Price;
            //productWithId.Description = product.Description;

            //Product productEntity = ProductChangeRequest.ToEntity(productWithId);

            //productEntity.Id = id;
            //productEntity.Stock = existingProduct.Result.Stock;

            await _productRepository.UpdateAsync(ProductDto.ToEntity(existingProduct));
        }


        public async Task ChangeAvailability(int id)
        {
            var product = _productRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el producto");

            product.ProductAvaible = !product.ProductAvaible;

           await _productRepository.UpdateAsync(product);
        }


        public async Task<string> ChangeStockProduct(int id, ChangeStockRequest stock)
        {
            var product = _productRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el producto");

            if (stock.Quantity < 0)
            {
                throw new Exception("El stock ingresado no puede ser menor a 0");
            }
            product.Stock = stock.Quantity;

            await _productRepository.UpdateAsync(product);

            return $"Se realizo el cambio en el stock del producto {product.Name}, su nuevo stock es {product.Stock}.";
        }


        public async Task<string> DeleteProduct(int id)
        {
            var productOrder = _orderProductRepository.GetListOrdersWithProduct(id).Result;
            if (productOrder.Count > 0)
            {
                var product = _productRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el producto");
                product.ProductAvaible = false;
                await _productRepository.UpdateAsync(product);
                return $"No se puede eliminar dicho producto ya que se encuentra en Ordenes existentes, el estado del producto se ha establecido en inactivo.";
            }
            else
            {
                var product = _productRepository.GetByIdAsync(id).Result ?? throw new Exception("No se encontro el producto");
                await _productRepository.DeleteAsync(product);
                return "El producto ha sido eliminado con éxito.";
            }
        }

    }
}
