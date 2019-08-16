using System;
using System.ComponentModel.DataAnnotations;

namespace bankAccount.Models {
    public class SuccessView {
        public Transaction Transaction {get;set;}

        public User User {get;set;}
    }
}