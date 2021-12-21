using CargaClic.Domain.Mantenimiento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaClic.Data.Mappings.Mantenimiento
{
    public class NivelConfiguration : IEntityTypeConfiguration<Nivel>
    {
        public void Configure(EntityTypeBuilder<Nivel> builder)
        {
            builder.ToTable("Nivel","Mantenimiento");
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Descripcion).IsRequired();
        }
    }
}