using System;
using System.Collections.Generic;
using Common.QueryContracts;

namespace CargaClic.Contracts.Results.Mantenimiento
{
    public class ListarUbicacionesResult : QueryResult
    {
        public IEnumerable<ListarUbicacionesDto> Hits { get;set; }
    }
    public class ListarUbicacionesDto 
    {
        public int  Id	{get;set;}
        public string  Ubicacion	{get;set;}
        public string  Area	{get;set;}
        public string  Estado	{get;set;}
        public string Almacen {get;set;}
        public int NivelId {get;set;}
        public int PosicionId {get;set;}

        
    }
    public class ListarUbicacionesMaster {
        public string Area {get;set;}
        public List<ListarUbicacionesDto> detalle {get;set;}
        
    }
    




}