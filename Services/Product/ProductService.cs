using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPG_Project.Data;
using RPG_Project.DTOs;
using RPG_Project.DTOs.Figth;
using RPG_Project.Models;
namespace RPG_Project.Services
{
    public class ProductService : IProductService
    {

        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public ProductService(AppDBContext dBContext, IMapper mapper, ILogger<ProductService> log)
        {
            this._dBContext = dBContext;
            this._mapper = mapper;
            this._log = log;
        }

        public async Task<ServiceResponse<GetProductDto>> AddProductr(AddProductDto newProductr)
        {
            var product = new Product();

            product.Name = newProductr.Name;
            product.Price = newProductr.Price;
            product.StockCount = newProductr.StockCount;
            product.ProductGroupId = newProductr.ProductGroupId;
            _dBContext.Product.Add(product);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<GetProductDto>(product);
            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<List<GetProductDto>>> DelProducts(int DelId)
        {

            var product = await _dBContext.Product.FirstOrDefaultAsync(x => x.Id == DelId);

            if (product != null)
            {
                _dBContext.Product.Remove(product);
                await _dBContext.SaveChangesAsync();
            }


            var Productgroup = await _dBContext.Product.AsNoTracking().ToListAsync();

            var dto = _mapper.Map<List<GetProductDto>>(Productgroup);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetProductDto>> EditProductr(EditProductDto editProductr)
        {
            var product = await _dBContext.Product.FirstOrDefaultAsync(x => x.Id == editProductr.Id);
            //var characters = await _dBContext.Characters.Include(x => x.Weapon).AsNoTracking().ToListAsync();
            if (product == null)
            {
                return ResponseResult.Failure<GetProductDto>("Product not found.");
            }

            var productgroup = await _dBContext.ProductGroup.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == editProductr.Id);
            //var characters = await _dBContext.Characters.Include(x => x.Weapon).AsNoTracking().ToListAsync();
            if (productgroup == null)
            {
                return ResponseResult.Failure<GetProductDto>("ProductGroup not found.");
            }



            product.Name = editProductr.Name;
            product.Price = editProductr.Price;
            product.StockCount = editProductr.StockCount;
            product.ProductGroupId = editProductr.ProductGroupId;

            _dBContext.Product.Update(product);
            await _dBContext.SaveChangesAsync();

            var dto = _mapper.Map<GetProductDto>(product);
            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<List<GetProductGroupDto>>> GetAllGroupProducts()
        {
            var ProductGroup = await _dBContext.ProductGroup.Include(x => x.Product).AsNoTracking().ToListAsync();
            var dto = _mapper.Map<List<GetProductGroupDto>>(ProductGroup);
            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProducts()
        {
            var Product = await _dBContext.Product.AsNoTracking().ToListAsync();

            var dto = _mapper.Map<List<GetProductDto>>(Product);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductrById(int productrId)
        {
            // var productgroup = await _dBContext.ProductGroup.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == productrId);
            var product = await _dBContext.Product.FirstOrDefaultAsync(x => x.Id == productrId);
            if (product == null)
            {
                return ResponseResult.Failure<GetProductDto>("Product not found.");
            }

            var dto = _mapper.Map<GetProductDto>(product);

            return ResponseResult.Success(dto);
        }

    }
}