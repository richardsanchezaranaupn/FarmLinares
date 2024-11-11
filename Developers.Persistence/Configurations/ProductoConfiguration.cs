using Developers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        // Nombre la tabla
        builder.ToTable("Productos");

        // Clave primaria
        builder.HasKey(x => x.ProductoId);

        // Atributos
        builder.Property(x => x.ProductoName)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode();
        builder.Property(x => x.Cantidad)
            .IsRequired();
        builder.Property(x => x.Precio)
            .HasPrecision(5, 2)
            .IsRequired();
        builder.Property(x => x.Receta)
            .IsRequired();
        builder.Property(x => x.FechaIngreso)
            .HasColumnType("date")
            .IsRequired();
        builder.Property(x => x.CategoriaId).IsRequired();
        builder.Property(x => x.ProveedorId).IsRequired();
        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        builder.Property(x => x.Status)
            .IsRequired();

        // Relaciones
        builder.HasOne(x => x.Categoria)
            .WithMany()
            .HasForeignKey(x => x.CategoriaId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Proveedor)
            .WithMany()
            .HasForeignKey(x => x.ProveedorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
