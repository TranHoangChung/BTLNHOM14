using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace BTLNHOM14.Models
{
    public class Category
    {
        [Key]
        
        public int CategoryID {get; set;} =default!;
        public string CategoryName {get; set;} = default!;
    }
}