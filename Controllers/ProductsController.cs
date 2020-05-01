using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Api.Services;
using Api.Data;
using Api.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiContext _context;

        public static IWebHostEnvironment _environment;
        
        public ProductsController(ApiContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //create product 
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromForm] ProductForm productForm)
        {
            var productRepsitory =  new ProductsRepository(_context, _environment);
    
            var prod = await productRepsitory.Add(productForm);
            if(prod == null)
            {
                return BadRequest(new {status = false, message = "Failed !"});
            }
            return CreatedAtAction(nameof(GetByID), new{ id = prod.Id}, prod);
        }

        //update
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> UpdateByID(int Id, [FromForm] ProductForm productForm)
        {
           var prodRepository = new ProductsRepository(_context, _environment);
           var prodUpdate = prodRepository.Update(Id, productForm);
           if(prodUpdate == null)
           {
               return BadRequest(new {status = false, message = "Failed !"});
           }

           return  CreatedAtAction(nameof(GetByID), new { id = prodUpdate.Id }, prodUpdate);
        }
        //deleta um objeto do tipo Product pelo id
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteByID(int id)
        {
            try
            {  
               var prodRepository = new ProductsRepository(_context, _environment); 
               if (await prodRepository.Delete(id))
               {
                  return Ok(new { status = true, message = "Removido com sucesso !"});
               }
               else{
                  return BadRequest("Não foi possível remover objeto");
               }
               
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
               
        //retorna um objeto do tipo Product pelo id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> GetByID(int id)
        {
            var productRepsitory = new ProductsRepository(_context, _environment);
            return await productRepsitory.GetById(id);
        }

        //retorna lista de objetos do tipo Product
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Product>> GetAll()
        {
          var prodRepository = new ProductsRepository(_context, _environment);
          var products = prodRepository.GetAll();
          if(products == null)
          {
              return BadRequest(new { status = false, message = "Houve alguma problema, porfavor tente novamente !"});
          }
          else
          {
              return Ok(products);
          }
        }

        [HttpGet]
        [Route("/imagem/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetImagemById(int id){
            Byte[] imagem;
            var product = await _context.Products.FindAsync(id);
            
            if(product.Image_path == null){
                imagem = System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\assets\\semImagem.jpeg");
                return File(imagem, "image/jpeg");
            }
            else
            {
                var dataType = product.Image_path.Split('.');
                imagem = System.IO.File.ReadAllBytes(product.Image_path);
                return File(imagem, "image/" + dataType[1]);
            }

            
        }
    }
}