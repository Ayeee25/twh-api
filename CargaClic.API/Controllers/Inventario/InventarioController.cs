using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CargaClic.Contracts.Parameters.Inventario;
using CargaClic.Contracts.Results.Inventario;
using CargaClic.Data.Interface;
using CargaClic.Domain.Inventario;
using CargaClic.ReadRepository.Contracts.Inventario.Parameters;
using CargaClic.ReadRepository.Interface.Inventario;
using CargaClic.Repository.Contracts.Inventario;
using CargaClic.Repository.Interface;
using Common.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargaClic.API.Controllers.Mantenimiento
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioRepository _repoInventario;
        private readonly IMapper _mapper;
        private readonly IRepository<InventarioGeneral> _repo;
        private readonly IInventarioReadRepository _repoReadInventario;
        private readonly IQueryHandler<ListarInventarioParameter> _handler;
        private readonly IRepository<InventarioDetalle> _repoDetalle ;

        public InventarioController(IInventarioRepository repoInventario
        , IMapper mapper
        , IRepository<InventarioGeneral> repo
        , IInventarioReadRepository repoReadInventario
        , IQueryHandler<ListarInventarioParameter> handler, IRepository<InventarioDetalle> repoDetalle)
        {
            _repoInventario = repoInventario;
            _mapper = mapper;
            _repo = repo;
            _repoReadInventario = repoReadInventario;
            _handler = handler;
            _repoDetalle = repoDetalle;
        }
        [HttpPost("register_inventario")]
        public async Task<IActionResult> RegisterInventario(InventarioForRegister inventarioGeneral)
        {
            //InventarioForRegister inventarioGeneral
            var createdInventario = await _repoInventario.RegistrarInventario(inventarioGeneral);
            return Ok(createdInventario);
        }
        [HttpPost("registrar_ajuste")]
        public async Task<IActionResult> RegisterAjuste(AjusteForRegister ajusteForRegister)
        {
            //InventarioForRegister inventarioGeneral
            var createdInventario = await _repoInventario.RegistrarAjuste(ajusteForRegister);
            return Ok(createdInventario);
        }
        [HttpPost("asignar_ubicacion")]
        public async Task<IActionResult> AsignarUbicacion(IEnumerable<InventarioForAssingment> inventarioGeneral)
        {
           var createdInventario = await _repoInventario.AssignarUbicacion(inventarioGeneral);
            return Ok(createdInventario);
        }
        [HttpPost("terminar_acomodo")]
        public async Task<IActionResult> terminar_acomodo(InventarioForFinishRecive inventarioForFinish)
        {
            var createdInventario = await _repoInventario.FinalizarRecibo(inventarioForFinish);
            return Ok();
        }
        [HttpPost("merge_ajuste")]
        public async Task<IActionResult> merge_ajuste(MergeInventarioRegister mergeInventarioRegister)
        {
            var createdInventario = await _repoInventario.MergeInventario(mergeInventarioRegister);
            return Ok();
        }
        [HttpPost("almacenamiento")]
        public async Task<IActionResult> almacenamiento(InventarioForStorage inventarioForFinish)
        {
            var createdInventario = await _repoInventario.Almacenamiento(inventarioForFinish);
            return Ok();
        }
         [HttpPost("almacenamientomasivo")]
        public async Task<IActionResult> almacenamientomasivo(InventarioForStorageMasivo inventarioForFinish)
        {

            var param = new ListarInventarioParameter {
              Id   = inventarioForFinish.Id
            };
            
            var resp = (ListarInventarioResult)_handler.Execute(param);
            var linea = new ListarInventarioDtoMasivo();
         
            List<ListarInventarioDtoMasivo> pallets = new List<ListarInventarioDtoMasivo>();
            foreach (var item in resp.Hits)
            {
                 linea = new ListarInventarioDtoMasivo();
                 linea.Almacen = item.Almacen;
                 linea.Almacenado = item.Almacenado;
                 linea.AlmacenId  = item.AlmacenId;
                 linea.cantidad_productos = item.cantidad_productos;
                 linea.CodigoHuella = item.CodigoHuella;
                 linea.DescripcionLarga = item.DescripcionLarga;
                 linea.FechaExpire = item.FechaExpire;
                 linea.FechaUltMovimiento = item.FechaUltMovimiento;
                 linea.HuellaId = item.HuellaId;
                 linea.LodId = item.LodId;
                 linea.LodNum = item.LodNum;
                 linea.ProductoId = item.ProductoId;
                 linea.Ubicacion = item.Ubicacion;
                 linea.UbicacionId = item.UbicacionId;
                 linea.UbicacionIdUlt  = item.UbicacionIdUlt;
                 linea.UbicacionProxima = item.UbicacionProxima;
                 linea.UntPak = item.UntPak;
                 linea.UntQty = item.UntQty;
                 linea.UsuarioIngreso = item.UsuarioIngreso;
                 linea.Referencia = item.Referencia;
                 linea.Id = item.Id;

                 pallets.Add(linea);
            
            }
         
            
            var createdInventario = await _repoInventario.AlmacenamientoMasivo(pallets);
            return Ok();
        }

        [HttpGet("GetAllInventario")]
        public async Task<IActionResult> GetAllInventario(Guid OrdenReciboId)
        {
            var Inventarios = await _repoReadInventario.GetAllInventario(OrdenReciboId);
            return Ok(Inventarios);
        }
        
        [HttpGet("GetInventarioByLotNum")]
        public async Task<IActionResult> GetInventarioByLotNum(Guid productoid, string lotnum)
        {
            var Inventarios = await _repoReadInventario.obtenerInventarioPorLote(productoid ,lotnum );
            return Ok(Inventarios);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll(Guid? Id)
        {
            var param = new ListarInventarioParameter {
              Id   = Id
            };
            
            var resp = (ListarInventarioResult)_handler.Execute(param);
            return Ok(resp.Hits);
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(long Id)
        {
            var result = await _repo.Get(x=>x.Id == Id);
            return Ok(result);
        }
        
        [HttpGet("GetPallet")]
        public async Task<IActionResult> GetPallet(Guid OrdenReciboId)
        {
            var result = await _repoReadInventario.GetPallet(OrdenReciboId);
            return Ok(result);
        }

        [HttpGet("GetGraficoStock")]
        public async Task<IActionResult> GetPalGetGraficoStocklet(int PropietarioId, int AlmacenId)
        {
            var result = await _repoReadInventario.GetGraficosStock(PropietarioId, AlmacenId);
            return Ok(result);
        }
        [HttpGet("GetGraficoRecepcion")]
        public async Task<IActionResult> GetGraficosRecepcion(int PropietarioId, int AlmacenId)
        {
            var result = await _repoReadInventario.GetGraficosRecepcion(PropietarioId, AlmacenId);
            return Ok(result);
        }
        [HttpGet("GetAllInvetarioAjuste")]
        public async Task<IActionResult> GetAllInvetarioAjuste(Guid ProductoId , 
         int ClienteId, string FechaInicio, int EstadoId)
        {
            
            var param = new GetAllInventarioParameters {
                ClientId = ClienteId,
                ProductoId = ProductoId,
                EstadoId = EstadoId
            };
            
            var resp = await  _repoReadInventario.GetAllInventario(param);
            return Ok(resp);
        }
        [HttpGet("GetAllInvetarioAjusteDetalle")]
        public async Task<IActionResult> GetAllInvetarioAjusteDetalle(long Id)
        {
            var resp = await  _repoReadInventario.GetAllInventarioDetalle(Id);
            return Ok(resp);
        }
        [HttpGet("GetAllInventarioDetalle")]
        public async Task<IActionResult> GetAllInventarioDetalle(long InventarioId)
        {
            var resp = await  _repoReadInventario.GetAllInventarioDetalle(InventarioId);
            return Ok(resp);
        }
        [HttpPost("RegistrarInventarioDetalle")]
        public async Task<IActionResult> RegistrarInventarioDetalle(InventarioDetalleForRegister inventarioDetalleForRegister)
        {
            

            var resp = await  _repoInventario.RegistrarInventarioDetalle(inventarioDetalleForRegister);
            return Ok(resp);
        }
        [HttpDelete("DeleteInventarioDetalle")]
        public async Task<IActionResult> DeleteInventarioDetalle(long id)
        {
            var detalle = await _repoDetalle.Get(x=>x.Id == id);

             var inventario =  await _repo.Get(x=>x.Id == detalle.InventarioId);
             inventario.ScanQty = inventario.ScanQty - 1;

              _repoDetalle.Delete(detalle);
              await _repo.SaveAll();

            return Ok();
        }
        
        [HttpPost("actualizar_inventario")]
        public async Task<IActionResult> ActualizarInventario(InventarioForEdit inventarioGeneral)
        {
            var editedInventario = await _repoInventario.ActualizarInventario(inventarioGeneral);
            return Ok(editedInventario);
        }
    }
}