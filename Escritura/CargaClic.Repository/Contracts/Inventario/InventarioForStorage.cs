using System;
using System.ComponentModel.DataAnnotations;

namespace CargaClic.Repository.Contracts.Inventario
{
    public class InventarioForStorage
    {
        [Required]
        public long Id {get;set;}
    }
      public class InventarioForStorageMasivo
    {
        [Required]
        public Guid Id {get;set;}
    }
     public class ListarInventarioDtoMasivo
    {
            public long Id	{get;set;}
            public string LodNum	{get;set;}
            public long LodId {get;set;}
            public Guid ProductoId	{get;set;}
            public string DescripcionLarga	{get;set;}
            public int UbicacionId	{get;set;}
            public string Ubicacion	{get;set;}
            public string UbicacionProxima	{get;set;}
            public string LotNum	{get;set;}
            public DateTime FechaExpire	{get;set;}
            public int UntQty	{get;set;}
            public int UntPak	{get;set;}
            public DateTime FechaRegistro	{get;set;}
            public DateTime FechaUltMovimiento	{get;set;}
            public int UsuarioIngreso	{get;set;}
            public string UbicacionIdUlt	{get;set;}
            public int HuellaId	{get;set;}
            public string CodigoHuella{get;set;}
            public bool Almacenado {get;set;}
            public string Almacen {get;set;}
            public int AlmacenId {get;set;}
            public int cantidad_productos {get; set;}
            public string Referencia  {get;set;}
    }
}