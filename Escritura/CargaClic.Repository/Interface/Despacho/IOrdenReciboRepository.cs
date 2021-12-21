using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargaClic.API.Dtos.Despacho;
using CargaClic.Repository.Contracts.Despacho;
using CargaClic.Repository.Contracts.Inventario;
using CargaClic.Repository.Contracts.Seguimiento;

namespace CargaClic.Repository.Interface
{
    public interface IOrdenSalidaRepository
    {
        
        Task<Int64> RegisterOrdenSalida(OrdenSalidaForRegister ordenSalidaForRegister);
        Task<Int64> RegisterOrdenSalidaDetalle(OrdenSalidaDetalleForRegister ordenSalidaForRegister);

        Task<Int64> PlanificarPicking(PickingPlan pickingPlan);
        Task<Int64> matchTransporteCarga(string CargaId, long EquipoTransporteId);
        Task<Int64> MovimientoSalida(InventarioForStorage command);


        Task<Int64> MovimientoSalidaMasivo(long WorkId);

       
         Task<int> RegisterCargaMasiva (CargaMasivaForRegister cargaMasiva , IEnumerable<CargaMasivaDetalleForRegister> cargaMasivaDetalle );


        Task<Int64> assignmentOfDoor(AsignarPuertaSalida asignarPuertaSalida);
        Task<Int64> assignmentOfUser(AsignarUsuarioSalida asignarPuertaSalida);

        Task<Int64> RegisterCarga(CargaForRegister cargaForRegister);
        Task<Int64> EliminarPlanificacion(long cargaForRegister);
        Task<Int64> RegisterSalida(CargaForRegister ordenSalidaForRegister);


    }
}