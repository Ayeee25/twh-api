using System;
using System.ComponentModel.DataAnnotations;

public class GuiaCabecera

{ [Key]
    public long id {get;set;} 
    public int PropietarioId {get;set;}
    public int TipoMovimientoId {get;set;}
    public DateTime FechaGuía {get;set;}

}