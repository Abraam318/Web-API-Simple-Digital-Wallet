using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_API_Simple_Digital_Wallet.Models;

namespace Web_API_Simple_Digital_Wallet.Data
{
    public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Sender)
            .WithMany()
            .HasForeignKey(t => t.SAddress)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Receiver)
            .WithMany()
            .HasForeignKey(t => t.RAddress)
            .OnDelete(DeleteBehavior.Restrict);

    }
}

}