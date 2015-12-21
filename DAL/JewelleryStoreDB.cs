namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class JewelleryStoreDB : DbContext
    {
        public JewelleryStoreDB()
            : base("name=JewelleryStoreDB")
        {
        }

        public virtual DbSet<tblLog> tblLog { get; set; }
        public virtual DbSet<tblProduct> tblProduct { get; set; }
        public virtual DbSet<tblUser> tblUser { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblLog>()
                .Property(e => e.LogType)
                .IsUnicode(false);

            modelBuilder.Entity<tblProduct>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblProduct>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblProduct>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProduct>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<tblUser>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblUser>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<tblUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<tblUser>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tblUser>()
                .HasMany(e => e.tblLog)
                .WithOptional(e => e.tblUser)
                .HasForeignKey(e => e.UserID);
        }
    }
}
