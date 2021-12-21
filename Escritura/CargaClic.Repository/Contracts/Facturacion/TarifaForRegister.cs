using System;
using System.ComponentModel.DataAnnotations;

namespace CargaClic.API.Dtos.Recepcion
{
   public class TarifaForRegister
   {
      public int? Id {get;set;}
      public int PropietarioId { get; set; }
      public Guid? ProductoId { get; set; }
      public decimal Pos {get;set;}
      public decimal Ingreso {get;set;}
      public decimal Salida {get;set;}
      public decimal Seguro {get;set;}
      public int FamiliaId {get;set;}

      public int UnidadMedidaId {get;set;}
      public int TipoTarifaId {get;set;}
      public decimal MontoTarifa {get;set;}
   }
}