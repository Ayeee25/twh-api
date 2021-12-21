using System;
using System.Threading.Tasks;
using CargaClic.API.Dtos.Recepcion;


namespace CargaClic.Repository.Interface
{
    public interface IFacturacionRepository
    {
        Task<Int64> GenerarID(PreliquidacionForRegister oPreliquidacionForRegister);
        
        Task<Int64> GenerarPreliquidacion(PreliquidacionForRegister oPreliquidacionForRegister);
        Task<Int64> GenerarComprobante(ComprobanteForRegister oPreliquidacionForRegister);

      
        Task<Int64> GenerarPreliquidacion_1(PreliquidacionForRegister oPreliquidacionForRegister);
        Task<Int64> GenerarPreliquidacion_2(PreliquidacionForRegister oPreliquidacionForRegister);
        Task<Int64> GenerarPreliquidacion_3(PreliquidacionForRegister oPreliquidacionForRegister);
        Task<Int64> GenerarPreliquidacion_4(PreliquidacionForRegister oPreliquidacionForRegister);
        Task<Int64> GenerarPreliquidacion_5(PreliquidacionForRegister oPreliquidacionForRegister);
        Task<Int64> GenerarPreliquidacion_6(PreliquidacionForRegister oPreliquidacionForRegister);




    }
}
