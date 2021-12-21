using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargaClic.API.Dtos.Recepcion;
using CargaClic.Domain.Inventario;
using CargaClic.Domain.Mantenimiento;


namespace CargaClic.Repository.Interface
{
    public interface IOrdenReciboRepository
    {
        Task<EquipoTransporte> RegisterEquipoTransporte(EquipoTransporte eq, Guid Id);
        Task<EquipoTransporte> assignmentOfDoor(long EquipoTransporteId, int UbicacionId);
        Task<Int64> identifyDetail(OrdenReciboDetalleForIdentifyDto ordenReciboDetalleForIdentifyDto);
        Task<Int64> identifyDetailMix(IEnumerable<OrdenReciboDetalleForIdentifyDto> ordenReciboDetalleForIdentifyDto);
        Task<Guid> closeDetails(Guid OrdenReciboId);
        Task<Guid> matchTransporteOrdenIngreso(EquipoTransporteForRegisterDto eq);
        Task<IEnumerable<CalendarioResult>> GetListarCalendario();

     


    }
}