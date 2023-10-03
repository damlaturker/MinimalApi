using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using System.Net;
using MinimalApi.Demo.Models;
using MinimalApi.Demo.Models.DTO;
using MinimalApi.Demo.Repository.IRepository;

namespace MinimalApi.Demo.Endpoints
{
    public static class ProductEndpoints
    {

        public static void ConfigureProductEndpoints(this WebApplication app)
        {

            app.MapGet("/api/product", GetAllProduct)
                .WithName("GetProducts").Produces<BaseResponse>(200);
            //.RequireAuthorization("AdminOnly") ;

            app.MapGet("/api/product/{id:int}", GetProduct)
                .WithName("GetProduct").Produces<BaseResponse>(200);
            //.AddFilter<ParameterIDValidator>();

            app.MapPost("/api/product", CreateProduct)
                .WithName("CreateProduct")
                .Accepts<ProductCreateDto>("application/json")
                .Produces<BaseResponse>(201)
                .Produces(400);
                //.AddFilter<BasicValidator<ProductCreateDto>>();

            //app.MapPut("/api/product", UpdateProduct)
            //    .WithName("UpdateProduct")
            //    .Accepts<ProductUpdateDTO>("application/json")
            //    .Produces<BaseResponse>(200).Produces(400)
            //    .AddFilter<BasicValidator<ProductUpdateDTO>>();

            app.MapDelete("/api/product/{id:int}", DeleteProduct);
        }

        private async static Task<IResult> GetProduct(IProductRepository _productRepo, ILogger<Program> _logger, int id)
        {
            Console.WriteLine("Endpoint executed.");
            BaseResponse response = new();
            response.Result = await _productRepo.GetAsync(id);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }
        // [Authorize]
        private async static Task<IResult> CreateProduct(IProductRepository _productRepo, IMapper _mapper,
                 [FromBody] ProductCreateDto product_C_DTO)
        {
            BaseResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            if (_productRepo.GetAsync(product_C_DTO.Name).GetAwaiter().GetResult() != null)
            {
                response.ErrorMessages.Add("Product Name already Exists");
                return Results.BadRequest(response);
            }

            Product product = _mapper.Map<Product>(product_C_DTO);


            await _productRepo.CreateAsync(product);
            await _productRepo.SaveAsync();
            ProductDto productDTO = _mapper.Map<ProductDto>(product);


            response.Result = productDTO;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return Results.Ok(response);
            //return Results.CreatedAtRoute("GetProduct",new { id=product.Id }, productDTO);
            //return Results.Created($"/api/product/{product.Id}",product);
        }
        // [Authorize]
        //private async static Task<IResult> UpdateProduct(IProductRepository _productRepo, IMapper _mapper,
        //         [FromBody] ProductUpdateDTO product_U_DTO)
        //{
        //    BaseResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };


        //    await _productRepo.UpdateAsync(_mapper.Map<Product>(product_U_DTO));
        //    await _productRepo.SaveAsync();

        //    response.Result = _mapper.Map<ProductDto>(await _productRepo.GetAsync(product_U_DTO.Id)); ;
        //    response.IsSuccess = true;
        //    response.StatusCode = HttpStatusCode.OK;
        //    return Results.Ok(response);
        //}
        //  [Authorize]
        private async static Task<IResult> DeleteProduct(IProductRepository _productRepo, int id)
        {
            BaseResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };


            Product productFromStore = await _productRepo.GetAsync(id);
            if (productFromStore != null)
            {
                await _productRepo.RemoveAsync(productFromStore);
                await _productRepo.SaveAsync();
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
                return Results.Ok(response);
            }
            else
            {
                response.ErrorMessages.Add("Invalid Id");
                return Results.BadRequest(response);
            }
        }

        private async static Task<IResult> GetAllProduct(IProductRepository _productRepo, ILogger<Program> _logger)
        {
            BaseResponse response = new();
            _logger.Log(LogLevel.Information, "Getting all Products");
            response.Result = await _productRepo.GetAllAsync();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }

    }
}
