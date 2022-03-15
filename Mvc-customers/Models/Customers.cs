using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_customers.Models
{
    public class Customers
    {
        [Key]

        public int CustomerID { get; set; }

        [Required(ErrorMessage ="First Name is required")]
        [DisplayName("First Name")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        [DisplayName("Middle Name")]
        public string Middlename { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}
