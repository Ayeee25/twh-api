using System;
using System.ComponentModel.DataAnnotations;

namespace CargaClic.API.Dtos.Despacho
{
   public class  OrdenSalidaForRegister
   {
        public long Id {get;set;}
        [Required]
        public int PropietarioId { get; set; }
        [Required]
        public string Propietario { get; set; }
        public string NumOrden {get;set;}
        public int AlmacenId { get; set; }
        
        public string GuiaRemision { get; set; }

        [Required]
        public String FechaRequerida { get; set; }
        [Required]
        public string HoraRequerida {get;set;}
        public string OrdenCompraCliente { get; set; }
        public String FechaRegistro { get; set; }
        public int ClienteId { get; set; }
        public int DireccionId { get; set; }
        public long? EquipoTransporteId { get; set; }
        public int EstadoId {get;set;}
        public int UsuarioRegistro {get;set;}
        public int UbicacionId {get;set;}
        [Required]
        public int TipoRegistroId {get;set;}
        
    }
}