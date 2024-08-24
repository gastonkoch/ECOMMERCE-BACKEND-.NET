using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
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
            var products = _productRepository.ListAsync().Result ?? throw new NotFoundException("No se encontraron productos");
            return ProductDto.ToList(products);
        }

        public ICollection<ProductDto> GetProductsAvaible()
        {
            var products = _productRepository.GetListAvaible().Result ?? throw new NotFoundException("No se encontraron productos");
            return ProductDto.ToList(products);
        }

        public ProductDto GetProductById(int id)
        {
            var product = _productRepository.GetByIdAsync(id).Result ?? throw new NotFoundException("No se encontró el producto");
            return ProductDto.ToDto(product);
        }

        public ICollection<ProductDto> GetProductByName(string name)
        {
            var products = _productRepository.GetByName(name).Result ?? throw new NotFoundException("No se encontró el producto");
            return ProductDto.ToList(products);
        }

        public ProductDto CreateProduct(ProductCreateRequest product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new AppValidationException("Ingrese el nombre del producto");
            }

            if (product.Price <= 0)
            {
                throw new AppValidationException("El precio del producto no es válido");
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                throw new AppValidationException("Ingrese una descripción para el producto");
            }

            if (product.Stock < 0)
            {
                throw new AppValidationException("El stock no puede ser menor a 0");
            }

            var productEntity = ProductCreateRequest.ToEntity(product);
            var createdProduct = _productRepository.AddAsync(productEntity).Result;
            return ProductDto.ToDto(createdProduct);
        }

        public async Task UpdateProduct(int id, ProductChangeRequest product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new AppValidationException("Ingrese el nombre del producto");
            }

            if (product.Price <= 0)
            {
                throw new AppValidationException("El precio del producto no es válido");
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                throw new AppValidationException("Ingrese una descripción para el producto");
            }

            var existingProduct = GetProductById(id);
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            await _productRepository.UpdateAsync(ProductDto.ToEntity(existingProduct));
        }

        public async Task ChangeAvailability(int id)
        {
            var product = _productRepository.GetByIdAsync(id).Result ?? throw new NotFoundException("No se encontró el producto");
            product.ProductAvaible = !product.ProductAvaible;
            await _productRepository.UpdateAsync(product);
        }

        public async Task<string> ChangeStockProduct(int id, ChangeStockRequest stock)
        {
            var product = _productRepository.GetByIdAsync(id).Result ?? throw new NotFoundException("No se encontró el producto");

            if (stock.Quantity < 0)
            {
                throw new AppValidationException("El stock ingresado no puede ser menor a 0");
            }
            product.Stock = stock.Quantity;

            await _productRepository.UpdateAsync(product);

            return $"Se realizó el cambio en el stock del producto {product.Name}, su nuevo stock es {product.Stock}.";
        }

        public async Task<string> DeleteProduct(int id)
        {
            var productOrder = _orderProductRepository.GetListOrdersWithProduct(id).Result;
            if (productOrder.Count > 0)
            {
                var product = _productRepository.GetByIdAsync(id).Result ?? throw new NotFoundException("No se encontró el producto");
                product.ProductAvaible = false;
                await _productRepository.UpdateAsync(product);
                return $"No se puede eliminar el producto ya que está en órdenes existentes. El estado del producto se ha establecido en inactivo.";
            }
            else
            {
                var product = _productRepository.GetByIdAsync(id).Result ?? throw new NotFoundException("No se encontró el producto");
                await _productRepository.DeleteAsync(product);
                return "El producto ha sido eliminado con éxito.";
            }
        }
    }
}
