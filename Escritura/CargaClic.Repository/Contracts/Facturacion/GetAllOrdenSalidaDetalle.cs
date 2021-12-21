using System;
using CargaClic.Common;

namespace CargaClic.ReadRepository.Contracts.Despacho.Results
{
    public class GetPendientesLiquidacion 
    {

        public Guid? Id{get;set;}
        public string Producto {get;set;}
        public decimal Tarifa {get;set;}
        public decimal? Ingreso {get;set;}
        public decimal? Salida {get;set;}
        public decimal? Posdia {get;set;}
        public decimal? Seguro {get;set;}
        public decimal? Picking {get;set;}
        public decimal? Etiquetado {get;set;}
        public int Cantidad {get;set;}
        public int Paletas {get;set;}
        public decimal PosTotal{get;set;}
        public decimal? Total {get;set;}
        public DateTime? FechaIngreso {get;set;} 
        public DateTime? FechaSalida {get;set;}      
        public string LodNum {get;set;}
        public int EstadiaTotal {get;set;}
        public int EstadiaPeriodo {get;set;}
        //UnidadAlmacenamientoId

        
        public Guid ProductoId { get; set; }
        public long lodid {get;set;}
        public string Familia { get; set; }
        public string DescripcionLarga { get; set; }
        public string almacen { get; set; }
        public decimal Estadia { get; set; }
        public string UnidadAlmacenamiento { get; set; }
        public int UnidadAlmacenamientoId { get; set; }
        public bool fueratiempo {get;set;}
        
    }
}