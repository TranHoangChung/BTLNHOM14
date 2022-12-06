using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BTLNHOM14.Models
{
    public class Product
    {
        [Key]
        public int? ProductID { get; set; } = default!;
        public string? ProductName { get; set; } = default!;
        
        public string? Info {get; set;} = default!; 
        public int Sale {get; set;}
        public int Price {get; set;}
        public string? Note {get; set;} = default!;
        public string? ImageName { get; set; } = default!;

        [NotMapped]
        public IFormFile? ImageFile { get; set; } = default!;
        public int CategoryID {get; set;} = default!;
       // public string? CategoryName{get;set;}= default!;
        [ForeignKey("CategoryID")]
        
        
        public Category? Category {get; set;} = default!;

    }
}