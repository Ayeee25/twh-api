using System;

namespace CargaClic.Repository.Contracts.Seguimiento
{
    public class CargaMasivaForRegister
    {
        public int id { get; set; }
        public DateTime? fecha_registro { get; set; }
        public int? usuario_id { get; set; }
        public int? estado_id { get; set; }
        public long? ordensalidaid {get;set;}
    
    }
}