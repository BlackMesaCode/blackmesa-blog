using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BlackMesa.Models
{
    public class BlackMesaDb : DbContext
    {
        public BlackMesaDb() : base("BlackMesaLocalDb")
        {
            
        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //configure model with fluent API
        }

    }
}