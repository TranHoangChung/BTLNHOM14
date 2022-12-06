using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BTLNHOM14.Models
{
    public class ProductModel 
    {
        public List<Category> cat {get; set;} = default!;
        public List<Product> pro {get; set;} = default!;
    }
}