using System;

namespace CargaClic.ReadRepository.Contracts.Despacho.Results
{
    public class GetLiquidaciones
    {
        public long Id {get;set;}
        public string NumLiquidacion {get;set;}
        public string Propietario {get;set;}
        public DateTime FechaLiquidacion {get;set;}
        public DateTime? FechaInicio {get;set;}
        public DateTime? FechaFin {get;set;}
        public decimal SubTotal {get;set;}
        public decimal Igv {get;set;}
        public decimal Total {get;set;}
        public String Estado {get;set;}
        public string LodNum {get;set;}
        public decimal Posdia {get;set;}
        public decimal PosTotal {get;set;}
        public decimal Out {get;set;}
        
    }
     public class GetReporteServicio
    {
        public long Id {get;set;}
        public string Propietario {get;set;}
        public decimal Pos {get;set;}
        public decimal Ingreso {get;set;}
        public decimal Salida {get;set;}
        public decimal Seguro {get;set;}
        public decimal Picking {get;set;}
        public decimal Etiquetado {get;set;}
        public DateTime? FechaInicio {get;set;}
        public DateTime? FechaFin {get;set;}
        public decimal SubTotal {get;set;}
        public decimal Igv {get;set;}
        public decimal Total {get;set;}
        public int MonedaId {get;set;}
        public string  Moneda {get;set;}
        public string  SimboloMoneda {get;set;}
        public decimal? Porcentaje {get;set;}
        public decimal? Anterior {get;set;}


        
    }
}