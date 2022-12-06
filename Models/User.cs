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
    public class User
    {
        
        public int UserID {get; set;} =default!;
        public string FullName {get;set;}=default!;
        public string UserName {get; set;} = default!;
        public string Password {get; set;} =default!;
        public int Age {get; set;} =default!;
        public string Phone {get; set;} = default!;
        public string Address {get; set;} =default!;
    }
}