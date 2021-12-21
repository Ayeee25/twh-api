
using CargaClic.Domain.Prerecibo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaClic.Data.Mappings.Prerecibo
{
    public class CargaMasivaDetalleConfiguration: IEntityTypeConfiguration<CargaMasivaDetalle>
    {
        public void Configure(EntityTypeBuilder<CargaMasivaDetalle> builder)
        {
            builder.ToTable("CargaMasivaDetalle","Recepcion");
            builder.HasKey(x=>x.id);
            builder.Property(x=>x.referencia).IsRequired();
            
        }
    }
}