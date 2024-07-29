using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        ProductDto GetProductById(int id);
        ProductDto CreateProduct(ProductCreateRequest product);
        ICollection<ProductDto> GetAllProducts();
        Task UpdateProduct(int id, ProductChangeRequest productDto);
        Task ChangeAvailability(int id);
        Task<string> ChangeStockProduct(int id, ChangeStockRequest stock);
        Task<string> DeleteProduct(int id);
        ICollection<ProductDto> GetProductsAvaible();
        ICollection<ProductDto> GetProductByName(string name);
    }
}
