using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bankAccount.Models {
    public class User {
        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName {get;set;}

        [Required]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        [Display(Name = "Last Name")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [Display(Name = "Password")]
        public string Password {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Display(Name = "Password Confirmation")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string pwConfirm {get;set;}

        public decimal CurrentBalance {get;set;} = 0; 

        public List<Transaction> Transactions {get;set;}
    }
}