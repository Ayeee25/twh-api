namespace CargaClic.ReadRepository.Contracts.Mantenimiento.Results
{
    public class GetProductoResult
    {
        public System.Guid id	 {get;set;}
        public string Cliente {get;set;}
        public string Almacen {get;set;}
        public string Familia {get;set;}
        public string Codigo {get;set;}
        public string DescripcionLarga {get;set;}
        public decimal Peso {get;set;}
        public string UnidadMedida {get;set;}
        public int ClienteId {get;set;}
        public int FamiliaId {get;set;}
        public int UnidadMedidaId {get;set;}
        public bool Etiquetado {get;set;}
        public bool Seriado {get;set;}

    }
}