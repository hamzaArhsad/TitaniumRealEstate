using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Domain
{
    public class Property
    {
        public int Id { get; set; } = 1;
        [Required]
        [DataType(DataType.Text)]
        public string Location { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("^(Plot|File)$", ErrorMessage = "The Type must be 'plot' or 'file'.")]
        //[TypeValidationAttribute]
        public string Type { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Area { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public string Path { get; set; } = "path";
    }
}
