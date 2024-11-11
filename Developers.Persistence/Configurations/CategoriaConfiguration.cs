using Developers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Developers.Persistence.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        // Nombre la tabla
        builder.ToTable("Categorias");

        // Clave primaria
        builder.HasKey(x => x.CategoriaId);

        // Atributos
        builder.Property(x => x.CategoriaName)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode();
        builder.Property(x => x.Descripcion)
            .IsRequired(false)
            .IsUnicode()
            .HasColumnType("text");
        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        builder.Property(x => x.Status)
            .IsRequired();

    }
}
