using Developers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class VentaDetalleConfiguration : IEntityTypeConfiguration<VentaDetalle>
{
    public void Configure(EntityTypeBuilder<VentaDetalle> builder)
    {
        builder.ToTable("VentaDetalles");

        builder.HasKey(vd => vd.VentaDetalleId);

        builder.Property(vd => vd.Cantidad).IsRequired();
        builder.Property(vd => vd.PrecioUnitario).IsRequired();

        builder.HasOne(vd => vd.Venta)
               .WithMany(v => v.VentaDetalles)
               .HasForeignKey(vd => vd.VentaId);

        builder.HasOne(vd => vd.Producto)
               .WithMany()
               .HasForeignKey(vd => vd.ProductoId);
    }
}
