using System;

namespace CargaClic.Repository.Contracts.Inventario
{
    public class InventarioDetalleForRegister
    {
        public long id { get; set; }
        public long InventarioId { get;set; }
        public string CodigoProducto {get;set;}
        public string CodigoMac {get;set;}
        public string CodigoSerie {get;set;}
        public DateTime? fechahora_scan {get;set;}
        public Guid ProductoId {get;set;}
        
    }
}