using CargaClic.Domain.Inventario;
using CargaClic.Domain.Mantenimiento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaClic.Data.Mappings.Inventario
{
    public class InventarioDetalleConfiguration : IEntityTypeConfiguration<InventarioDetalle>
    {
        public void Configure(EntityTypeBuilder<InventarioDetalle> builder)
        {
            builder.ToTable("InventarioDetalle","Inventario");
            builder.HasKey(x=>x.Id);
        }
    }
}