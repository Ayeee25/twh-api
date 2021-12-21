using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CargaClic.Data;
using CargaClic.ReadRepository.Contracts.Inventario.Parameters;
using CargaClic.ReadRepository.Contracts.Inventario.Results;
using CargaClic.ReadRepository.Interface.Inventario;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CargaClic.ReadRepository.Repository.Inventario
{
    public class InventarioReadRepository : IInventarioReadRepository
    {
            private readonly DataContext _context;
            private readonly IConfiguration _config;

            public InventarioReadRepository(DataContext context,IConfiguration config)
            {
                _context = context;
                _config = config;
            }
            public IDbConnection Connection
            {   
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }
        public async Task<IEnumerable<GetAllInventarioResult>> GetAllInventario(GetAllInventarioParameters param)
        {
            var parametros = new DynamicParameters();
            parametros.Add("ProductoId", dbType: DbType.Guid, direction: ParameterDirection.Input, value: param.ProductoId);
            parametros.Add("ClienteId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: param.ClientId);
            parametros.Add("EstadoId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: param.EstadoId);
            parametros.Add("UbicacionId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: param.UbicacionId);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[inventario].[pa_listainventario_ajuste]";
                conn.Open();
                var result = await conn.QueryAsync<GetAllInventarioResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result;
            }
        }

        public async Task<IEnumerable<GetAllInventarioResult>> GetAllInventario(Guid OrdenReciboId)
        {
              var parametros = new DynamicParameters();
            parametros.Add("OrdenReciboId", dbType: DbType.Guid, direction: ParameterDirection.Input, value: OrdenReciboId);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[inventario].[pa_listarinventario]";
                conn.Open();
                var result = await conn.QueryAsync<GetAllInventarioResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result;
            }
        }

        public async Task<IEnumerable<GetAllInventarioDetalleResult>> GetAllInventarioDetalle(long InventarioId)
        {
            var parametros = new DynamicParameters();
            parametros.Add("InventarioId", dbType: DbType.Int64, direction: ParameterDirection.Input, value: InventarioId);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[inventario].[pa_listarinventariodetalle]";
                conn.Open();
                var result = await conn.QueryAsync<GetAllInventarioDetalleResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result;
            }
        }

        public async Task<IEnumerable<GetAllInventarioResult>> GetAllInventario(long Id)
        {
           var parametros = new DynamicParameters();
            parametros.Add("Id", dbType: DbType.Int64, direction: ParameterDirection.Input, value: Id);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[inventario].[pa_listainventario_ajuste_detalle]";
                conn.Open();
                var result = await conn.QueryAsync<GetAllInventarioResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result;
            }
        }

        public async Task<IEnumerable<GetGraficoRecepcionResult>> GetGraficosRecepcion(int PropietarioId, int AlmacenId)
        {
            var parametros = new DynamicParameters();
            parametros.Add("propietarioid", dbType: DbType.Int32, direction: ParameterDirection.Input, value: PropietarioId);
            parametros.Add("almacenid", dbType: DbType.Int32, direction: ParameterDirection.Input, value: AlmacenId);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[inventario].[pa_grafica_recepciones]";
                conn.Open();
                var result = await conn.QueryAsync<GetGraficoRecepcionResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result;
            }
        }

        public async Task<IEnumerable<GetGraficoStockResult>> GetGraficosStock(int PropietarioId, int AlmacenId)
        {
            var parametros = new DynamicParameters();
            parametros.Add("propietarioid", dbType: DbType.Int32, direction: ParameterDirection.Input, value: PropietarioId);
            parametros.Add("almacenid", dbType: DbType.Int32, direction: ParameterDirection.Input, value: AlmacenId);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[inventario].[pa_grafica_stock]";
                conn.Open();
                var result = await conn.QueryAsync<GetGraficoStockResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result;
            }
        }

        public async Task<IEnumerable<GetAllInventarioResult>> GetPallet(Guid OrdenReciboId)
        {
              var parametros = new DynamicParameters();
            parametros.Add("OrdenReciboId", dbType: DbType.Guid, direction: ParameterDirection.Input, value: OrdenReciboId);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[inventario].[pa_listarPallets]";
                conn.Open();
                var result = await conn.QueryAsync<GetAllInventarioResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result;
            }
        }

     

        public async Task<GetAllInventarioResult> obtenerInventarioPorLote(Guid ProductoId, string LotNum)
        {
              var parametros = new DynamicParameters();
            parametros.Add("lotnum", dbType: DbType.String, direction: ParameterDirection.Input, value: LotNum);
            parametros.Add("productoid", dbType: DbType.Guid, direction: ParameterDirection.Input, value: ProductoId);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[recepcion].[pa_obtenerLoteExistente]";
                conn.Open();
                var result = await conn.QueryAsync<GetAllInventarioResult>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
                return result.SingleOrDefault();
            }
        }
    }
}