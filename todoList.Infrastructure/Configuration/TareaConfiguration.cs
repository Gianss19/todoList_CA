using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using todoList.Domain;

namespace todoList.Infrastructure.Config;

public class TareaConfiguration : IEntityTypeConfiguration<Tarea>
{
    public void Configure(EntityTypeBuilder<Tarea> builder)
    {
        builder.ToTable("tareas");
        builder.HasKey(t=>t.Id);
        
        builder.Property(t=>t.Nombre)
               .IsRequired()
               .HasMaxLength(100);
        
        builder.Property(t=>t.IsCompleted)
               .IsRequired();      
        builder.Property(t=>t.FechaCreacion)
               .IsRequired();
        
        builder.Property(t=>t.FechaActualizacion);
        
        builder.Property(t=>t.UsuarioId)
               .IsRequired();

        builder.HasOne(t=>t.Usuario)
               .WithMany(u=>u.Tareas)
               .HasForeignKey(t=>t.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);
    }

}
