using System;

using System.ComponentModel.DataAnnotations;


namespace CargaClic.Domain.Prerecibo
{
   public class CargaMasiva
    {
        [Key]
        public int id { get; set; }
        public DateTime? fecha_registro { get; set; }
        public int? usuario_id { get; set; }
        public int? estado_id { get; set; }
        public int? ordensalidaid {get;set;}

    }
}