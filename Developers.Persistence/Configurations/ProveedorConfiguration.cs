using Developers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Developers.Persistence.Configurations;

public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        // Nombre la tabla
        builder.ToTable("Proveedores");

        // Clave primaria
        builder.HasKey(x => x.ProveedorId);

        // Atributos
        builder.Property(x => x.ProveedorName)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode();

        // Definir el campo ProveedorName como único
        builder.HasIndex(x => x.ProveedorName)
            .IsUnique();

        builder.Property(x => x.Ruc)
            .IsRequired()
            .HasMaxLength(11)
            .HasColumnType("varchar(11)");

        builder.Property(x => x.Direccion)
            .IsRequired(false)
            .HasColumnType("text");

        builder.Property(x => x.NombreContacto)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Telefono)
            .IsRequired()
            .HasMaxLength(15)
            .HasColumnType("varchar(15)");

        builder.Property(x => x.CorreoElectronico)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();
    }
}


