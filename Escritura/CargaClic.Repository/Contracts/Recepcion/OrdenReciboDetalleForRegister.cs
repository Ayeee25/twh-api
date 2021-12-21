using System;
using System.ComponentModel.DataAnnotations;

namespace CargaClic.API.Dtos.Recepcion
{
    public class OrdenReciboDetalleForIdentifyDto
    {

        public Int64 Id {get;set;}
        public string LotNum {get;set;}
        public int HuellaId {get;set;}
        [Required]
        public int EstadoID {get;set;}
        [Required]
        public int untQty {get;set;}
        // public int? CantidadSobrante {get;set;}
        // public int? CantidadFaltante { get;set;}
        public int? UbicacionId { get; set; }
        public String FechaExpire { get;set; }
        public String FechaManufactura { get;set; }
        public int? UnidadMedidaId { get;set; }
        public int? HuellaDetalleId {get;set;}
        public int PropietarioId {get;set;}
        public decimal? Peso {get;set;}

        public Guid ProductoId {get;set;}
        public Guid OrdenReciboId {get;set;}
        public long OrdenReciboDetalleId {get;set;}
        public string Referencia {get;set;}

        


    }
    public class CalendarioResult
    {
        public int idvehiculo	 {get;set;}
        public string title	 {get;set;}
        public DateTime start	 {get;set;}
        public DateTime end {get;set;}
        public int idtipo {get;set;}
        public int idpuerta {get;set;}
    }
}