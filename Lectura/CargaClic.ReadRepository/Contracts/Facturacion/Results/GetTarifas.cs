using System;

namespace CargaClic.ReadRepository.Contracts.Despacho.Results
{
    public class GetTarifas
    {
        public int Id	{get;set;}
        public string DescripcionLarga	{get;set;}
        public decimal Pos	{get;set;}
        public decimal Ingreso	{get;set;}
        public decimal Salida	{get;set;}
        public decimal Seguro	{get;set;}
        public decimal Montacarga	{get;set;}
        public decimal Movilidad {get;set;}
        
        public string Producto {get;set;}
        public string TipoTarifa {get;set;}
        public string UnidadMedida {get;set;}
        public decimal MontoTarifa {get;set;}

    }
    public class VentaMensualResult
    {
        public int id {get;set;}
        public int ClienteId {get;set;}
        public string Propietario {get;set;}
        public decimal subtotal {get;set;}
        public int? anio {get;set;} 
        public int? mes  {get;set;}
        public int periodo {get;set;}
    }


}