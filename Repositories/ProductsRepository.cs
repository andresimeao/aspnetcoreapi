using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Hosting;
using Api.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Repositories
{
    public class ProductsRepository
    {

        public static IWebHostEnvironment _environment;

        private readonly ApiContext _context;

        public ProductsRepository(ApiContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //create new register in products
        public async Task<Product> Add(ProductForm productForm)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    UploadImage._environment = _environment;
                    if (productForm.Imagem == null)
                    {
                        var newProd = new Product() { Name = productForm.Name, Value = productForm.Value };
                        _context.Products.Add(newProd);
                        _context.SaveChanges();
                        await transaction.CommitAsync();
                        return newProd;
                    }
                    else
                    {
                        var newProd = new Product() { Name = productForm.Name, Value = productForm.Value };

                        _context.Products.Add(newProd);

                        _context.SaveChanges();

                        //salvando imagem no root
                        var imagePath = UploadImage.upload(productForm.Imagem, newProd.Id.ToString());
                        //validando se imagem foi salva com sucesso
                        if (imagePath != "Failed")
                        {
                            newProd.Image_path = imagePath;
                            _context.Update(newProd);
                            _context.SaveChanges();
                            await transaction.CommitAsync();
                            return newProd;
                        }
                        else
                        {
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        //delete
        public async Task<bool> Delete(int Id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(e => e.Id == Id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;

            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        //list all
        public IEnumerable<Product> GetAll()
        {
            try
            {
                var products = _context.Products.ToList();
                return products;
            }
            catch (System.Exception)
            {

                return null;
            }
        }

        public async Task<Product> GetById(int Id)
        {


            var product = await _context.Products.FindAsync(Id);

            return product;
        }

        //update
        public Product Update(int Id, ProductForm productForm)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    UploadImage._environment = _environment;
                    if (productForm.Imagem == null)
                    {
                        try
                        {
                            var prodUpdate = _context.Products.FirstOrDefault(p => p.Id == Id);
                            prodUpdate.Name = productForm.Name;
                            prodUpdate.Value = productForm.Value;
                            //_context.Products.Update(prodUpdate);
                            _context.SaveChanges();
                            transaction.Commit();
                            return prodUpdate;
                        }
                        catch (System.Exception)
                        {
                            transaction.Rollback();
                            return null;
                        }
                    }
                    else
                    {
                        try
                        {
                            var prodUpdate = _context.Products.Single(p => p.Id == Id);
                            _context.SaveChanges();

                            prodUpdate.Name = productForm.Name;
                            prodUpdate.Value = productForm.Value;

                            //_context.Products.Update(prodUpdate);
                            _context.SaveChanges();

                            //salvando imagem no root
                            var imagePath = UploadImage.upload(productForm.Imagem, prodUpdate.Id.ToString());
                            //validando se imagem foi salva com sucesso
                            if (imagePath != "Failed")
                            {
                                prodUpdate.Image_path = imagePath;
                                _context.Update(prodUpdate);
                                _context.SaveChanges();
                                transaction.Commit();
                                return prodUpdate;
                            }
                            else
                            {
                                transaction.Rollback();
                                return null;
                            }

                        }
                        catch (System.Exception)
                        {
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }
    }
}