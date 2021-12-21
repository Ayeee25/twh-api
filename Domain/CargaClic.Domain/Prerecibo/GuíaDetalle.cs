using System;
using System.ComponentModel.DataAnnotations;

public class GuíaDetalle
{[Key]
    public long Id {get;set;}
    public int cantidad {get;set;}
    public string lote {get;set;}
    public int ProductoId {get;set;}
    public int EstadoId {get;set;}
    public string Referencia {get;set;}
    public int ErrorGuía {get;set;}
}