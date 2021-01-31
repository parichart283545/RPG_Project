using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG_Project.DTOs;
using RPG_Project.Services;
namespace RPG_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productservice;
        public ProductController(IProductService productservice)
        {
            _productservice = productservice;
        }


        [HttpGet("getallproducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productservice.GetAllProducts());
        }

        [HttpGet("getallproductgroups")]
        public async Task<IActionResult> GetAllGroupProducts()
        {
            return Ok(await _productservice.GetAllGroupProducts());
        }

        [HttpGet("getallproductById")]
        public async Task<IActionResult> GetProductrById(int productId)
        {
            return Ok(await _productservice.GetProductrById(productId));
        }

        [HttpPost("addproductr")]
        public async Task<IActionResult> AddProductr(AddProductDto newProduc)
        {
            return Ok(await _productservice.AddProductr(newProduc));
        }

        [HttpPut("editproductr")]
        public async Task<IActionResult> EditProductr(EditProductDto editProduc)
        {
            return Ok(await _productservice.EditProductr(editProduc));
        }

        [HttpDelete("removeproductr")]
        public async Task<IActionResult> DelProducts(int DelId)
        {
            return Ok(await _productservice.DelProducts(DelId));
        }
    }
}