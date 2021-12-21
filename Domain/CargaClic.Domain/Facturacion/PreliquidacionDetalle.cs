using System;
using System.ComponentModel.DataAnnotations;
using CargaClic.Common;

namespace CargaClic.Domain.Facturacion
{
    public class  PreliquidacionDetalle : Entity
    {
        [Key]
        public long Id { get; set; }
        public Guid? ProductoId { get; set; }
        public decimal? Pos { get; set; }
        public decimal? Ingreso { get; set; }
        public decimal? Salida { get; set; }
        public decimal? Seguro { get; set; }
        public decimal? Montacarga { get; set; }
        public decimal? Movilidad { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public long? PreliquidacionId { get; set; }
        public String LodNum {get;set;}
        public int? Pallet {get;set;}
        public decimal? Picking {get;set;}
        public decimal? Etiquetado {get;set;}
        public decimal? Total {get;set;}
        public int? EstadiaTotal {get;set;}
        public int EstadiaPeriodo {get;set;}
        public int Cantidad {get;set;}
        public bool? fueratiempo {get;set;}
        public string Familia {get;set;}
    }
}