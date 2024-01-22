using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace BeanSceneWebApp.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Ingredient { get; set; }
        public string Dietary { get; set; }
        public ProductCategory Category { get; set; }
        public int ProductCategoryId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]

       
        public string ImageName { get; set; }
        [NotMapped] //Don't add this field to database
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
    }
}
