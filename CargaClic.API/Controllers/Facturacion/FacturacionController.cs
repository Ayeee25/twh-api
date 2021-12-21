
using System.Threading.Tasks;
using AutoMapper;
using CargaClic.API.Dtos.Recepcion;
using CargaClic.Data.Interface;
using CargaClic.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CargaClic.Domain.Despacho;
using CargaClic.ReadRepository.Interface.Facturacion;
using CargaClic.Domain.Facturacion;
using System;
using System.Linq;

using System.Collections.Generic;

namespace CargaClic.API.Controllers.Facturacion
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FacturacionController : ControllerBase
    {

        private readonly IRepository<OrdenSalida> _repository;
        private readonly IRepository<OrdenSalidaDetalle> _repositoryDetalle;
        private readonly IRepository<Documento> _repository_Documento;
        private readonly IFacturacionReadRepository _repo_Read_Facturacion;
        private readonly IFacturacionRepository _repo_Facturacion;
        private readonly IMapper _mapper;
        private readonly IRepository<Tarifa> _repositoryTarifa;
        private readonly IRepository<Preliquidacion> _repo_Preliquidacion;

        public FacturacionController(
         IFacturacionReadRepository repo_read_Facturacion,
         IFacturacionRepository repo_Facturacion,
         IRepository<Documento> repo_Documento,
         IRepository<Tarifa> repositoryTarifa,
         IRepository<Preliquidacion> repo_Preliquidacion,
         IMapper mapper) {
            _repo_Read_Facturacion = repo_read_Facturacion;
            _repo_Facturacion = repo_Facturacion;
            _repository_Documento = repo_Documento;
            _repositoryTarifa = repositoryTarifa;
            _repo_Preliquidacion = repo_Preliquidacion;
            _mapper = mapper;
        }
        [HttpGet("GetPendientesLiquidacion")]
        public async Task<IActionResult> GetPendientesLiquidacion(int Id, string fechainicio, string fechafin)
        { 
            
            var resp  =  await _repo_Read_Facturacion.GetPendientesLiquidacion(Id,
            fechainicio, fechafin);
            

            if(Id  == 25)
            {
               foreach (var item in resp)
               {
                  if(resp.Where(x=>x.LodNum == item.LodNum).Count() > 1)
                  {
                     
                     foreach (var item2 in resp.Where(x=>x.LodNum == item.LodNum && x.Out > 0).ToList())
                     {
                         item.Posdia = 0;
                         item.PosTotal = 0;
                         item.Seguro = 0;
                         
                         
                     }
                      
                  }
               }
            }


            return Ok (resp);
        }
        
        [HttpGet("GetPreLiquidaciones")]
        public async Task<IActionResult> GetPreLiquidaciones(int Id)
        { 
            var resp  =  await _repo_Read_Facturacion.GetPreLiquidaciones(Id);
            
            if(Id  == 25)
            {
               foreach (var item in resp)
               {
                  if(resp.Where(x=>x.LodNum == item.LodNum).Count() > 1)
                  {
                     
                     foreach (var item2 in resp.Where(x=>x.LodNum == item.LodNum && x.Out > 0).ToList())
                     {
                         item.Posdia = 0;
                         item.PosTotal = 0;
                         
                         
                     }
                      
                  }
               }
            }

            return Ok (resp);
        }
         [HttpGet("GetPreLiquidacion")]
        public async Task<IActionResult> GetPreLiquidacion(int Id)
        { 
            var resp  =  await _repo_Read_Facturacion.GetPreLiquidacion(Id);
            return Ok (resp);
        }

        [HttpGet("GetReporteServicio")]
        public async Task<IActionResult> GetReporteServicio()
        { 
            var resp  =  await _repo_Read_Facturacion.GetReporteServicio();
            return Ok (resp);
        }

        [HttpPost("GenerarPreliquidacion")]
        public async  Task<IActionResult> GenerarPreliquidacion(PreliquidacionForRegister obj)
        { 

            var resp  = await _repo_Facturacion.GenerarPreliquidacion(obj);
            return Ok (resp);
        }   
        
        [HttpPost("GenerarComprobante")]
        public async  Task<IActionResult> GenerarComprobante(ComprobanteForRegister Id)
        { 
            var resp  = await _repo_Facturacion.GenerarComprobante(Id);
            return Ok (resp);
        }

        [HttpGet("GetAllTarifas")] 
        public async  Task<IActionResult> GetAllTarifas(int clienteid)
        { 
            var resp  = await _repo_Read_Facturacion.GetTarifas(clienteid);
            return Ok (resp);
        }
         [HttpGet("GetVentaMensual")] 
        public async  Task<IActionResult> GetVentaMensual()
        { 
            var resp  = await _repo_Read_Facturacion.GetVentaMensual();
            return Ok (resp);
        }
        
        [HttpGet("GetAllTarifasV2")] 
        public async  Task<IActionResult> GetAllTarifasV2(int clienteid, Guid? ProductoId)
        { 
            var resp  = await _repo_Read_Facturacion.GetTarifasV2(clienteid,ProductoId);
            return Ok (resp);
        }


        [HttpGet("GetAllSeries")]
        public async  Task<IActionResult> GetAllSeries()
        { 
            var resp  = await _repository_Documento.GetAll();
            return Ok (resp);
        }

        [HttpPost("InsertTarifa")]
        public async  Task<IActionResult> InsertTarifa(TarifaForRegister tarifaForRegister)
        { 
            var _tar = new Tarifa();
            
            _tar.ClienteId = tarifaForRegister.PropietarioId;
            _tar.ProductoId = tarifaForRegister.ProductoId;
            _tar.MontoTarifa =  tarifaForRegister.MontoTarifa ;
            _tar.TipoTarifaId = tarifaForRegister.TipoTarifaId;
            _tar.UnidadMedidaId = tarifaForRegister.UnidadMedidaId;
            _tar.FamiliaId = tarifaForRegister.FamiliaId;


            await _repositoryTarifa.AddAsync(_tar);
            await _repositoryTarifa.SaveAll();
            return Ok (_tar);
        }  
        [HttpDelete("DeletePreliquidacion")]
        public async Task<IActionResult> DeletePreliquidacion(int id)
        {
            var preliquidacion = await _repo_Preliquidacion.Get(x=>x.Id == id);
             _repo_Preliquidacion.Delete(preliquidacion);
             return Ok();

        }

        [HttpPost("UpdateTarifa")]
        public async  Task<IActionResult> UpdateTarifa(TarifaForRegister tarifaForRegister)
        { 
            var _tarifa =  _repositoryTarifa.Get(x=>x.Id == tarifaForRegister.Id).Result;
            
            _tarifa.Ingreso = tarifaForRegister.Ingreso;
            _tarifa.Pos = tarifaForRegister.Pos;
            _tarifa.Salida = tarifaForRegister.Salida;
            _tarifa.Seguro = tarifaForRegister.Seguro;

            await _repositoryTarifa.SaveAll();
            return Ok (_tarifa);
        }   
        [HttpGet("GetTarifa")]
        public async Task<IActionResult> GetTarifa(int Id)
        { 
            var resp  = await   _repositoryTarifa.Get(x=>x.Id == Id);
            return Ok (resp);
        }
        [HttpDelete("DeleteTarifa")]
        public async Task<IActionResult> DeleteTarifa(int Id)
        { 
            var resp  = await   _repositoryTarifa.Get(x=>x.Id == Id);
            _repositoryTarifa.Delete(resp);

            await _repositoryTarifa.SaveAll();
            return Ok (resp);
        }

    }
}