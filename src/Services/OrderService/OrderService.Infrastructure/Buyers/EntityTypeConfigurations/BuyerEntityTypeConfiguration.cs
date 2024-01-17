using OrderService.Domain.Buyers;

namespace OrderService.Infrastructure.Buyers.EntityTypeConfigurations
{
    internal class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("Buyer", ServiceCollectionExtention.DEFAULT_SCHEMA);

            builder.HasKey(b => b.Id);

            builder.Ignore(b => b.DomainEvents);

            builder.Property(b => b.Id)
                .UseHiLo("buyerseq", ServiceCollectionExtention.DEFAULT_SCHEMA);

            builder.Property(b => b.IdentityGuild)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex("IdentityGuild")
                .IsUnique(true);

            builder.Property(b => b.Name);

            builder.HasMany(b => b.PaymentMethods)
                .WithOne()
                .HasForeignKey("BuyerId")
                .OnDelete(DeleteBehavior.Cascade);

            var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));

            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
