using Developers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        // Nombre la tabla
        builder.ToTable("Ventas");

        // Clave primaria
        builder.HasKey(x => x.VentaId);

        // Atributos
        builder.Property(x => x.FechaRegistro)
            .HasColumnType("date")
            .IsRequired();
        builder.Property(x => x.ClienteName)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        builder.Property(x => x.Status)
            .IsRequired();

        // Relaciones
        builder.HasOne(x => x.ApplicationUser)
            .WithMany()
            .HasForeignKey(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configuración de la relación uno a muchos con VentaDetalle
        builder.HasMany(x => x.VentaDetalles)
            .WithOne(x => x.Venta)
            .HasForeignKey(x => x.VentaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

