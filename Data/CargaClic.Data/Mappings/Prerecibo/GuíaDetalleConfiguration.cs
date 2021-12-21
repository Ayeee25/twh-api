

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaClic.Data.Mappings.Prerecibo
{
public class GuíaDetalleConfiguration: IEntityTypeConfiguration<GuíaDetalle>
{


    public void Configure(EntityTypeBuilder<GuíaDetalle> builder)
    {
         builder.ToTable("GuiaDetalle","Recepcion");
            builder.HasKey(x=>x.Id);
    }
}
    
}