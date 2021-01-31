using System.Collections.Generic;
using System.Threading.Tasks;
using RPG_Project.DTOs;
using RPG_Project.Models;
namespace RPG_Project.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<GetProductDto>>> GetAllProducts();
        Task<ServiceResponse<List<GetProductGroupDto>>> GetAllGroupProducts();
        Task<ServiceResponse<GetProductDto>> GetProductrById(int productrId);
        Task<ServiceResponse<GetProductDto>> AddProductr(AddProductDto newProductr);
        Task<ServiceResponse<GetProductDto>> EditProductr(EditProductDto editProductr);
        Task<ServiceResponse<List<GetProductDto>>> DelProducts(int DelId);
    }
}