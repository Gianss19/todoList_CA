using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using todoList.Domain;

namespace todoList.Infrastructure.Config;

public class UsuarioConfiguration: IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");
        builder.HasKey(u=> u.Id);
        
        builder.Property(u=> u.Nombre)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u=>u.Correo)
               .IsRequired()
               .HasMaxLength(100);
        
        builder.HasIndex(u=>u.Correo).IsUnique();

        builder.Property(u=>u.PasswordHash)
               .IsRequired()
               .HasMaxLength(255);
       
        
        builder.Property(u=>u.Activo)
               .IsRequired();
        builder.Property(u=>u.FechaCreacion)
               .IsRequired();
        
        builder.Property(u=>u.FechaActualizacion);




    }
}

    
