using Common.QueryContracts;

namespace CargaClic.Contracts.Parameters.Prerecibo
{
    public class ListarUbicacionesParameter : QueryParameter
    {
        public int AlmacenId { get; set; }
        public int AreaId {get;set;}
        public int? FilaId {get; set;}
        public int? ColumnaId {get;set;}
    }
}