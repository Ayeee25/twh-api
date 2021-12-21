
using CargaClic.Domain.Prerecibo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaClic.Data.Mappings.Prerecibo
{
    public class CargaMasivaConfiguration: IEntityTypeConfiguration<CargaMasiva>
    {
        public void Configure(EntityTypeBuilder<CargaMasiva> builder)
        {
            builder.ToTable("CargaMasiva","Recepcion");
            builder.HasKey(x=>x.id);
            builder.Property(x=>x.fecha_registro).IsRequired();
            
        }
    }
}