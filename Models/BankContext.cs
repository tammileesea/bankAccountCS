using Microsoft.EntityFrameworkCore;

namespace bankAccount.Models {
    public class  BankContext : DbContext {
        public BankContext(DbContextOptions options) : base(options) {}
        public DbSet<User> users {get;set;}
        public DbSet<Transaction> transactions {get;set;}
    }
}