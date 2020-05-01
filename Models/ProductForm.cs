using Microsoft.AspNetCore.Http;

namespace Api.Models
{
    public class ProductForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public IFormFile Imagem { get; set; }
    }
}