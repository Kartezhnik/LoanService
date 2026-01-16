using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .ValueGeneratedNever();           

            builder.Property(l => l.Number)
                .IsRequired()
                .HasMaxLength(50);                

            builder.HasIndex(l => l.Number)
                .IsUnique();                      

            builder.Property(l => l.Amount)
                .IsRequired()
                .HasPrecision(18, 2);             

            builder.Property(l => l.TermValue)
                .IsRequired();

            builder.Property(l => l.InterestValue)
                .IsRequired()
                .HasPrecision(5, 2);              

            builder.Property(l => l.Status)
                .IsRequired()
                .HasConversion<string>();        

            builder.Property(l => l.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");  

            builder.Property(l => l.ModifiedAt)
                .IsRequired(true)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}