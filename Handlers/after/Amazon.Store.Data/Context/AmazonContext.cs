using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text;
using Amazon.Store.Data.Models;

namespace Amazon.Store.Data.Context
{
    public class AmazonContext : DbContext
    {
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        private static readonly string ConnectionString = @"Data Source=.\sqlexpress;database=amazon;Integrated Security=True;MultipleActiveResultSets=True;"; //Environment.GetEnvironmentVariable("Amazon.ConnectionString");

        public AmazonContext() : base(ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>()
                .HasKey(s => s.CartId);
                //.HasOptional(s => s.Products)
                //.WithOptionalDependent()
                //.WillCascadeOnDelete(true);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasKey(s => s.Id)
                .HasRequired(s => s.ShoppingCart)
                .WithMany(s => s.Products)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var message = new StringBuilder();
                message.AppendLine("Database validation error(s):");
                foreach (var error in ex.EntityValidationErrors)
                {
                    message.AppendFormat("\t{0}", error.Entry.Entity).AppendLine();
                    foreach (var i in error.ValidationErrors)
                    {
                        message.AppendFormat("\t\t{0} => {1}", i.PropertyName, i.ErrorMessage).AppendLine();
                    }
                }
                Debug.WriteLine(message);
                throw;
            }
        }

    }
}
