using System;
using System.ComponentModel.DataAnnotations;
using CargaClic.Common;

namespace CargaClic.Domain.Prerecibo
{
    public class CargaMasivaDetalle : Entity
    {
            [Key]
            public long id { get; set; }
            public int? carga_id { get; set; }
            [MaxLength(30)]
            public string referencia {get;set;}

            public long ordensalidaid {get;set;}
            

    }
}