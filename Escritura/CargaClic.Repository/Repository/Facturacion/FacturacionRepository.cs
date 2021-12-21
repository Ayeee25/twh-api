
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CargaClic.API.Dtos.Recepcion;
using CargaClic.Common;
using CargaClic.Data;
using CargaClic.Domain.Facturacion;
using CargaClic.ReadRepository.Contracts.Despacho.Results;
using CargaClic.Repository.Contracts.Inventario;
using CargaClic.Repository.Interface;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CargaClic.Repository
{
    public class FacturacionRepository : IFacturacionRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public FacturacionRepository(DataContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public IDbConnection Connection
        {   
            get
            {
                var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                try
                {
                     connection.Open();
                     return connection;
                }
                catch (System.Exception)
                {
                    connection.Close();
                    connection.Dispose();
                    throw;
                }
            }
        }

        public async Task<long> GenerarComprobante(ComprobanteForRegister command)
        {
            Preliquidacion entity ;
            Comprobante comprobante;
            ComprobanteDetalle comprobantedetalle;
            using(var transaction = _context.Database.BeginTransaction())
            {
                entity = await _context.Preliquidacion.Where(x=>x.Id==command.PreliquidacionId).SingleOrDefaultAsync();
               // entity_detalles = await _context.PreliquidacionDetalle.Where(x=>x.PreliquidacionId == command.PreliquidacionId).ToListAsync();
                
                comprobante = new Comprobante();
                comprobante.Igv = entity.Igv;
                comprobante.NumeroComprobante = "001-005498";
                comprobante.PreliquidacionId = command.PreliquidacionId;
                comprobante.SubTotal = entity.SubTotal;
                comprobante.TipoComprobanteId = 1;
                comprobante.Total = entity.Total;
                comprobante.UsuarioRegistroId = 1;


                await _context.AddAsync<Comprobante>(comprobante);
                await _context.SaveChangesAsync();

                // foreach (var item in entity_detalles)
                // {
                    comprobantedetalle = new ComprobanteDetalle();
                    comprobantedetalle.ComprobanteId =  comprobante.Id;
                    comprobantedetalle.Descripcion = "Por almacenamiento" ;
                    comprobantedetalle.Subtotal = entity.SubTotal;
                    comprobantedetalle.Total = entity.Total;
                    comprobantedetalle.Igv = entity.Igv;
                    await _context.AddAsync<ComprobanteDetalle>(comprobantedetalle);
                    await _context.SaveChangesAsync();
                // }

                transaction.Commit();


               
            }
            return 1;
        }
        public async Task<long> Almacenamiento(InventarioForStorage command)
        {

            var dominio = await _context.InventarioGeneral.Where(x=>x.Id == command.Id).SingleOrDefaultAsync();
            var invlod  = await _context.InvLod.Where(x=>x.Id == dominio.LodId).SingleOrDefaultAsync();

           

            /// Lógica para cerrar pedido

            using(var transaction = _context.Database.BeginTransaction())
            {
               
                try
                {
                        invlod.UbicacionId =   invlod.UbicacionProxId.Value;
                        invlod.UbicacionProxId = null;
                         
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                }
                catch (Exception ex)
                    {
                        transaction.Rollback();  
                        throw ex; 
                    }
                return command.Id;
            }

            
        }
        public async Task<long> GenerarPreliquidacion(PreliquidacionForRegister command)
        {
            
            IEnumerable<GetPendientesLiquidacion> result ;
            Decimal? SubTo = 0;

            var parametros = new DynamicParameters();

            parametros.Add("PropietarioId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: command.ClienteId);
            parametros.Add("strcorteinicio", dbType: DbType.String, direction: ParameterDirection.Input, value: command.InicioCorte);
            parametros.Add("strcortefin", dbType: DbType.String, direction: ParameterDirection.Input, value: command.FinCorte);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[Facturacion].[pa_listarpendientesliquidacion_fase2]";
                 result = await conn.QueryAsync<GetPendientesLiquidacion>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );
            }
            foreach (var item in result)
            {
                SubTo = SubTo + item.Total ;
            }

            using(var transaction = _context.Database.BeginTransaction())
            {
                var preliquidacion = new  Preliquidacion();
                try
                {
                preliquidacion.AlmacenId = 1;
                preliquidacion.ClienteId = command.ClienteId;
                preliquidacion.FechaLiquidacion = DateTime.Now;
                preliquidacion.SubTotal = SubTo;
                preliquidacion.Igv = Convert.ToDecimal(Convert.ToDouble(SubTo) * 0.18);
                preliquidacion.Total = preliquidacion.SubTotal + preliquidacion.Igv;
                preliquidacion.EstadoId = (int) Constantes.EstadoPreliquidacion.Pendiente;
                
                DateTime dtInicioCorte =DateTime.ParseExact(command.InicioCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                preliquidacion.FechaInicio = dtInicioCorte ;

                DateTime dtInicioFin =DateTime.ParseExact(command.FinCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                preliquidacion.FechaFin = dtInicioFin;
                
                await _context.AddAsync<Preliquidacion>(preliquidacion);
                await _context.SaveChangesAsync();

                PreliquidacionDetalle detalle ;

                    if(command.ClienteId  == 25 || command.ClienteId == 26 || command.ClienteId == 27   
                                                        || command.ClienteId == 19  || command.ClienteId == 29  )
                    {
                          foreach (var item in result)
                          {
                                
                                int cantidades = result.Where(x=>x.LodNum == item.LodNum && x.Cantidad > 0).ToList().Count ;
                                if(cantidades > 0 )
                                {
                                       // HAY ALGUNO EN EL INVENTARIO
                                        var temp =  result.Where(x=>x.LodNum == item.LodNum && x.Salida > 0).ToList();
                                        foreach (var item2 in temp)
                                        {
                                            item2.Posdia = 0;
                                            item2.PosTotal = 0;
                                            item2.Ingreso = 0;
                                            item2.Seguro = 0;
                                            item2.Total = item2.Salida + item2.Picking;
                                        }          
                                }
                                else {
                                       // TODOS SALIERON 
                                        bool segundos = true;
                                        foreach (var item2 in result.Where(x=>x.LodNum == item.LodNum && x.Salida > 0).ToList().OrderByDescending(x=>x.FechaSalida))
                                        {
                                            if(!segundos) {
                                                item2.Posdia = 0;
                                                item2.PosTotal = 0;
                                                item2.Ingreso = 0;
                                                item2.Seguro = 0;
                                                item2.Total = item2.Salida + item2.Picking;
                                            }
                                            segundos = false;
                                            
                                        }             
                                }

                            }
                    }
                    
                foreach (var item in result)
                {
                    detalle = new PreliquidacionDetalle();
                    detalle.Ingreso = item.Ingreso;
                    detalle.Pos = item.PosTotal;
                    detalle.PreliquidacionId = preliquidacion.Id;
                    detalle.ProductoId = item.Id;
                    detalle.Salida = item.Salida;
                    
                    detalle.Seguro = item.Seguro;
                    detalle.Picking = item.Picking;
                    detalle.Total = item.Total;
                    detalle.Pallet = item.Paletas;
                    detalle.LodNum = item.LodNum;
                    detalle.FechaIngreso = item.FechaIngreso;
                    detalle.FechaSalida = item.FechaSalida;
                    detalle.EstadiaPeriodo = item.EstadiaPeriodo;
                    detalle.EstadiaTotal = item.EstadiaTotal;
                    detalle.Cantidad = item.Cantidad;
                    detalle.Etiquetado = item.Etiquetado;
                    detalle.fueratiempo = item.fueratiempo;
                    detalle.Familia = item.Familia;
                    await _context.AddAsync<PreliquidacionDetalle>(detalle);
                    
                }
                await _context.SaveChangesAsync();
              

                preliquidacion.NumLiquidacion = "030-" + preliquidacion.Id.ToString().PadLeft(6,'0');
                await _context.SaveChangesAsync();
                transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();  
                    throw ex; 
                }
                return 1;
            }

        }

        public async Task<long> GenerarPreliquidacion_1(PreliquidacionForRegister command)
        {
            var parametros = new DynamicParameters();

            parametros.Add("PropietarioId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: command.ClienteId);
            parametros.Add("strcorteinicio", dbType: DbType.String, direction: ParameterDirection.Input, value: command.InicioCorte);
            parametros.Add("strcortefin", dbType: DbType.String, direction: ParameterDirection.Input, value: command.FinCorte);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[Facturacion].[pa_listarpendientespreliquidacion_1_reporte_csharp]";
                var result = await conn.QueryAsync<GetPendientesLiquidacion>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );

                var detalles = result.ToList();
                ///////////Evaluacion por pallet///////////////////////////////////////////////
                var paletas = detalles.Where(x => x.UnidadAlmacenamientoId == 161).GroupBy(x => x.LodNum);
                foreach (var a in paletas)
                {
                   var detalle = getDetails( command.PrequilidacionId.Value , a.First().LodNum
                                        , a.First().Estadia * a.First().EstadiaPeriodo
                                        , null
                                        , null
                                        , null
                                        , null
                                        , a.First().FechaIngreso
                                        , null
                                        , a.First().EstadiaPeriodo
                                        , a.First().Cantidad
                                         );

                    Console.WriteLine("{0} -  {1} ", a.First().LodNum, a.First().Estadia * a.First().EstadiaPeriodo);
                    await _context.AddAsync<PreliquidacionDetalle>(detalle);

                    
                }
                ////////////////////////////////////////////////////////////////////
                 // Evaluación por Unidad
                foreach (var a in detalles)
                {
                    if (a.UnidadAlmacenamientoId != 161)
                    {
                      var  detalle = getDetails(command.PrequilidacionId.Value, a.LodNum
                                           , a.Estadia * a.EstadiaPeriodo
                                           , null
                                           , null
                                           , a.ProductoId
                                           , a.Familia
                                           , a.FechaIngreso
                                           , null
                                           , a.EstadiaPeriodo
                                           , a.Cantidad
                                            );

                        Console.WriteLine("{0} -  {1} ", a.LodNum, a.Estadia * a.EstadiaPeriodo);

                        await _context.AddAsync<PreliquidacionDetalle>(detalle);
                    }

                }
                await _context.SaveChangesAsync();
            }
            return 1;
        }

        public async Task<long> GenerarPreliquidacion_2(PreliquidacionForRegister command)
        {
            var detalle = new PreliquidacionDetalle();

              var parametros = new DynamicParameters();

            parametros.Add("PropietarioId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: command.ClienteId);
            parametros.Add("strcorteinicio", dbType: DbType.String, direction: ParameterDirection.Input, value: command.InicioCorte);
            parametros.Add("strcortefin", dbType: DbType.String, direction: ParameterDirection.Input, value: command.FinCorte);

            using (IDbConnection conn = Connection)
            {
                string sQuery = "[Facturacion].[pa_listarpendientespreliquidacion_2_reporte_csharp]";
                var result = await conn.QueryAsync<GetPendientesLiquidacion>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );

                var detalles = result.ToList();

                ///////////Evaluacion por pallet///////////////////////////////////////////////

                var paletas = detalles.Where(x => x.UnidadAlmacenamientoId == 161).GroupBy(x => x.LodNum);

               
                foreach (var a in paletas)
                {
                    detalle = getDetails(command.PrequilidacionId.Value , a.First().LodNum
                                        , a.First().Estadia * a.First().EstadiaPeriodo
                                        , null
                                          , null
                                          ,null
                                          ,null
                                        , a.First().FechaIngreso
                                        , null
                                        , a.First().EstadiaPeriodo
                                        , a.First().Cantidad
                                         );

                    Console.WriteLine("{0} -  {1} ", a.First().LodNum, a.First().Estadia * a.First().EstadiaPeriodo);

                    await _context.AddAsync<PreliquidacionDetalle>(detalle);
                }
                ////////////////////////////////////////////////////////////////////


                // Evaluación por Unidad  e ingreso
                foreach (var a in detalles)
                {
                    if (a.UnidadAlmacenamientoId == 128)
                    {
                        detalle = getDetails(command.PrequilidacionId.Value, a.LodNum
                                           , a.Estadia * a.EstadiaPeriodo
                                           , a.Ingreso 
                                            , null
                                            , a.Id
                                           , a.Familia
                                           , a.FechaIngreso
                                           , null
                                           , a.EstadiaPeriodo
                                           , a.Cantidad
                                            );

                        Console.WriteLine("{0} -  {1} ", a.LodNum, a.Estadia * a.EstadiaPeriodo);

                        await _context.AddAsync<PreliquidacionDetalle>(detalle);
                    }
                    else
                    {
                        detalle = getDetails(command.PrequilidacionId.Value, a.LodNum
                                         , a.Estadia * 0
                                         , a.Ingreso * a.Cantidad
                                         , null
                                         , a.Id
                                         , a.Familia
                                         , a.FechaIngreso
                                         , null 
                                         , a.EstadiaPeriodo
                                         , a.Cantidad
                                          );
                        await _context.AddAsync<PreliquidacionDetalle>(detalle);
                    }


                }
                await _context.SaveChangesAsync();
                return 1;

            }

        }

        public async Task<long> GenerarPreliquidacion_3(PreliquidacionForRegister command)
        {
             var detalle = new PreliquidacionDetalle();

              var parametros = new DynamicParameters();

            parametros.Add("PropietarioId", dbType: DbType.Int32, direction: ParameterDirection.Input, value: command.ClienteId);
            parametros.Add("strcorteinicio", dbType: DbType.String, direction: ParameterDirection.Input, value: command.InicioCorte);
            parametros.Add("strcortefin", dbType: DbType.String, direction: ParameterDirection.Input, value: command.FinCorte);


             using (IDbConnection conn = Connection)
            {
                string sQuery = "[Facturacion].[pa_listarpendientespreliquidacion_3_reporte_cshrap]";
                var result = await conn.QueryAsync<GetPendientesLiquidacion>(sQuery,
                                                                           parametros
                                                                          ,commandType:CommandType.StoredProcedure
                  );

                var detalles = result.ToList();


                //Evaluacion por empaque
                long lod = 0;

                var paletas = detalles.Where(x => x.UnidadAlmacenamientoId == 161).GroupBy(x => x.LodNum);

                foreach (var a in paletas)
                {
                    lod = a.First().lodid;

                    if (_context.InventarioGeneral.Where(x => x.LodId == lod).ToList().Count > 0)
                        continue;


                    detalle = getDetails(command.PrequilidacionId.Value, a.First().LodNum
                                        , a.First().Estadia * a.First().EstadiaPeriodo
                                        , null
                                        , null
                                        , null
                                        , null
                                        , a.First().FechaIngreso
                                        , a.First().FechaSalida
                                        , a.First().EstadiaPeriodo
                                        , a.First().Cantidad
                                         );

                    Console.WriteLine("{0} -  {1} ", a.First().LodNum, a.First().Estadia * a.First().EstadiaPeriodo);
                    _context.PreliquidacionDetalle.Add(detalle);

                }
                
             

                //Console.ReadKey();
                foreach (var a in detalles)
                {
                    lod = a.lodid;

                    if (a.UnidadAlmacenamientoId == 128)
                    {
                        if (_context.InventarioGeneral.Where(x => x.LodId == lod).ToList().Count > 0)
                            continue;

                        detalle = getDetails(command.PrequilidacionId.Value, a.LodNum
                                              , a.Estadia * a.EstadiaPeriodo
                                              , null
                                              , a.Salida
                                               , a.Id
                                                 , a.Familia
                                              , a.FechaIngreso
                                              , a.FechaSalida
                                              , a.EstadiaPeriodo
                                              , a.Cantidad
                                               );

                        Console.WriteLine("{0} -  {1} ", a.LodNum, a.Estadia * a.EstadiaPeriodo);

                         _context.PreliquidacionDetalle.Add(detalle);
                    }
                    else
                    {
                        detalle = getDetails(command.PrequilidacionId.Value, a.LodNum
                                         , a.Estadia * 0
                                         , null
                                         , a.Salida * a.Cantidad
                                           , a.Id
                                             , a.Familia
                                         , a.FechaIngreso
                                         , a.FechaSalida
                                         , a.EstadiaPeriodo
                                         , a.Cantidad
                                          );
                         _context.PreliquidacionDetalle.Add(detalle);
                    }

                }

                //Console.WriteLine(total_pallet.Value);
                await _context.SaveChangesAsync();
                Console.WriteLine("Terminado");
                return 1;
               
            }
        }

        public Task<long> GenerarPreliquidacion_4(PreliquidacionForRegister oPreliquidacionForRegister)
        {
            throw new NotImplementedException();
        }

        public Task<long> GenerarPreliquidacion_5(PreliquidacionForRegister oPreliquidacionForRegister)
        {
            throw new NotImplementedException();
        }

        public Task<long> GenerarPreliquidacion_6(PreliquidacionForRegister oPreliquidacionForRegister)
        {
            throw new NotImplementedException();
        }

         private static PreliquidacionDetalle getDetails(long id, string lodNum, decimal? POS, decimal? IN , decimal? OUT
         , Guid? productoid , string familia ,  DateTime? fechaIngreso, DateTime? fechaSalida, int estadiaPeriodo, int cantidad)
        {
            PreliquidacionDetalle detalle = new PreliquidacionDetalle();
            detalle.Ingreso = IN;
            detalle.Pos = POS;
            detalle.PreliquidacionId = id;
            detalle.ProductoId = productoid;
            detalle.Salida = OUT;
            detalle.Seguro =null;
            detalle.Picking = null;
            detalle.Total =  (IN==null?0:IN) + (POS==null?0:POS) + (OUT==null?0:OUT);
            detalle.Pallet = null;
            detalle.LodNum = lodNum;
            detalle.FechaIngreso = fechaIngreso;
            detalle.FechaSalida = fechaSalida;
            detalle.EstadiaPeriodo = estadiaPeriodo;
            detalle.EstadiaTotal = null;
            detalle.Cantidad = cantidad;
            detalle.Etiquetado = null;
            return detalle;
        }

        public async Task<long> GenerarID(PreliquidacionForRegister oPreliquidacionForRegister)
        {
             var preliquidacion = new Preliquidacion();
             using(var transaction = _context.Database.BeginTransaction())
            {
              
                    try
                    {
                        preliquidacion.AlmacenId = 1;
                        preliquidacion.ClienteId = oPreliquidacionForRegister.ClienteId;
                        preliquidacion.FechaLiquidacion = DateTime.Now;
                        preliquidacion.SubTotal = 0;
                        preliquidacion.Igv = 0;
                        preliquidacion.Total = preliquidacion.SubTotal + preliquidacion.Igv;
                        preliquidacion.EstadoId = 28;

                        DateTime dtInicioCorte = DateTime.ParseExact(oPreliquidacionForRegister.InicioCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        preliquidacion.FechaInicio = dtInicioCorte;

                        DateTime dtInicioFin = DateTime.ParseExact(oPreliquidacionForRegister.FinCorte, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        preliquidacion.FechaFin = dtInicioFin;

                        await _context.AddAsync<Preliquidacion>(preliquidacion);
                        await _context.SaveChangesAsync();

                        preliquidacion.NumLiquidacion = "030-" + preliquidacion.Id.ToString().PadLeft(6, '0');
                        await _context.SaveChangesAsync();

                        transaction.Commit();



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    return preliquidacion.Id;
                
            }
        }
    }
}
    
