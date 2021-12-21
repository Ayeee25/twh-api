using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CargaClic.API.Dtos.Recepcion;
using CargaClic.Data.Interface;
using CargaClic.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CargaClic.ReadRepository.Interface.Despacho;
using CargaClic.Domain.Despacho;
using CargaClic.Repository.Contracts.Inventario;
using CargaClic.API.Dtos.Despacho;
using CargaClic.Repository.Contracts.Despacho;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using CargaClic.Domain.Mantenimiento;
using System.IO;
using System.Net.Http.Headers;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using CargaClic.Repository.Contracts.Seguimiento;

namespace CargaClic.API.Controllers.Despacho
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenSalidaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRepository<OrdenSalida> _repository;
        private readonly IRepository<Cliente> _repositoryCliente;
        private readonly IRepository<Direccion> _repositoryDireccion;
        private readonly IRepository<OrdenSalidaDetalle> _repositoryDetalle;
        private readonly IOrdenSalidaRepository _repo_OrdenSalida;
        private readonly IDespachoReadRepository _repo_Read_Despacho;
        private readonly IMapper _mapper;

        public OrdenSalidaController(IConfiguration config, 
         IRepository<OrdenSalida> repository ,
         IRepository<OrdenSalidaDetalle> repositoryDetalle,
         IOrdenSalidaRepository repo_OrdenSalida,
         IDespachoReadRepository repo_read_Despacho,
         IRepository<Cliente> repositoryCliente,
         IRepository<Direccion> repositoryDireccion,
         IMapper mapper) {

             _config = config;
            _repository = repository;
            _repositoryDetalle = repositoryDetalle;
            _repo_OrdenSalida = repo_OrdenSalida;
            _repo_Read_Despacho = repo_read_Despacho;
            _mapper = mapper;
            _repositoryCliente = repositoryCliente;
            _repositoryDireccion = repositoryDireccion;

        }
    
      [HttpDelete("DeleteOrder")]
      public async Task<IActionResult> DeleteOrder(long OrdenSalidaId)
      {
          var detalles = await _repositoryDetalle.GetAll(x=>x.OrdenSalidaId == OrdenSalidaId);
        //   if(detalles.Count() != 0 )
        //      throw new ArgumentException("err020"); 


          var ordenrecibo = await _repository.Get(x=>x.Id == OrdenSalidaId);
          _repository.Delete(ordenrecibo);
          
          
          return Ok(ordenrecibo);
      }
      [HttpDelete("DeleteOrderDetail")]
      public async Task<IActionResult> DeleteOrderDetail(long id)
      {
          var detalle = await _repositoryDetalle.Get(x=>x.Id == id);
          _repositoryDetalle.Delete(detalle);
          return Ok(detalle);
      }

      [HttpGet("GetAllOrderDetail")]
      public async Task<IActionResult> GetOrderDetail(long Id)
      { 
          var resp  =  await _repo_Read_Despacho.GetAllOrdenSalidaDetalle(Id);
          return Ok (resp);
      }

      [HttpGet("GetAllOrder")]
      public async Task<IActionResult> GetAllOrder(int AlmacenId, int PropietarioId, int EstadoId,  string fec_fin, string fec_ini)
      { 
          var resp  = await _repo_Read_Despacho.GetAllOrdenSalida(AlmacenId, PropietarioId,  EstadoId,  fec_ini , fec_fin);
          return Ok (resp);
      }
      [HttpGet("GetAllPedido")]
      public async Task<IActionResult> GetAllPedido(int AlmacenId, int PropietarioId, int? EstadoId,  string fec_fin, string fec_ini)
      { 
          var resp  = await _repo_Read_Despacho.GetAllOrdenPedido(AlmacenId, PropietarioId,  EstadoId,  fec_ini , fec_fin);
          return Ok (resp);
      }
      [HttpGet("GetAllOrderPendiente")]
      public async Task<IActionResult> GetAllOrderPendiente(int? AlmacenId, int? PropietarioId, int? EstadoId, int? DaysAgo)
      { 
          var resp  = await _repo_Read_Despacho.GetAllOrdenSalidaPendiente(AlmacenId , PropietarioId,  EstadoId,  DaysAgo);
          return Ok (resp);
      }
      [HttpGet("GetAllCargas")]
      public async Task<IActionResult> GetAllCargas(int PropietarioId, int EstadoId)
      { 
          var resp  = await _repo_Read_Despacho.GetAllCargas( PropietarioId,  EstadoId);
          return Ok (resp);
      }
      [HttpGet("GetAllCargas_Pendientes_Salida")]
      public async Task<IActionResult> GetAllCargas_Pendientes_Salida(int PropietarioId, int EstadoId)
      { 
          var resp  = await _repo_Read_Despacho.GetAllCargas_Pendientes_Salida( PropietarioId,  EstadoId);
          return Ok (resp);
      }
      [HttpGet("GetOrder")]
      public async  Task<IActionResult> GetOrder(long OrdenSalidaId)
      { 
          var resp  = await _repo_Read_Despacho.GetOrdenSalida(OrdenSalidaId);
          return Ok (resp);
      }
      [HttpGet("GetAllWork")]
      public async  Task<IActionResult> GetAllWork(int PropietarioId, int EstadoId)
      { 
          var resp  = await _repo_Read_Despacho.ListarTrabajo(PropietarioId,EstadoId );
          return Ok (resp);
      }

     [HttpGet("GetAllWorkAssigned")]
      public async  Task<IActionResult> GetAllWorkAssigned(int PropietarioId, int EstadoId)
      { 
          var resp  = await _repo_Read_Despacho.ListarTrabajo_TrabajoAsignado(PropietarioId,EstadoId );
          return Ok (resp);
      }
     [HttpGet("GetAllWorkDetail")]
      public async  Task<IActionResult> GetAllWorkDetail(long WrkId)
      { 
          var resp  = await _repo_Read_Despacho.ListarTrabajoDetalle(WrkId);
          return Ok (resp);
      }
      [HttpGet("GetAllPendienteCarga")]
      public async  Task<IActionResult> GetAllPendienteCarga()
      { 
          var resp  = await _repo_Read_Despacho.ListarPendienteCarga();
          return Ok (resp);
      }

      [HttpGet("ListarPickingPendiente")]
      public async  Task<IActionResult> ListarPickingPendiente()
      { 
          var resp  = await _repo_Read_Despacho.ListarPickingPendiente();
          return Ok (resp);
      }

      [HttpGet("ListarPickingPendienteDetalle")]
      public async  Task<IActionResult> ListarPickingPendienteDetalle(long ShipmentId)
      { 
          var resp  = await _repo_Read_Despacho.ListarPickingPendienteDetalle(ShipmentId);
          return Ok (resp);
      }


   
     
     

#region _Registros

      [HttpPost("assignmentOfDoor")]
      public async Task<IActionResult> assignmentOfDoor(AsignarPuertaSalida asignarPuertaSalida)
      {
        var result = await _repo_OrdenSalida.assignmentOfDoor(asignarPuertaSalida);
        return Ok(result);
      }
      [HttpPost("MovimientoSalida")]
      public async Task<IActionResult> MovimientoSalida(InventarioForStorage inventarioForStorage)
      {
        var result = await _repo_OrdenSalida.MovimientoSalida(inventarioForStorage);
        return Ok(result);
      }
    
      [HttpPost("MovimientoSalidaMasivo")]
      public async Task<IActionResult> MovimientoSalidaMasivo(long WrkId)
      {
        var result = await _repo_OrdenSalida.MovimientoSalidaMasivo(WrkId);
        return Ok(result);
      }

      [HttpPost("assignmentOfUser")]
      public async Task<IActionResult> assignmentOfUser(AsignarUsuarioSalida asignarPuertaSalida)
      {
        var result = await _repo_OrdenSalida.assignmentOfUser(asignarPuertaSalida);
        return Ok(result);
      }

      [HttpPost("RegisterOrdenSalida")]
      public async Task<IActionResult> RegisterOrdenSalida(OrdenSalidaForRegister ordenSalidaForRegister)
      {

            string SMTP_SERVER =  _config.GetSection("AppSettings:SMTPSERVER").Value;
            string SMTP_MAIL =  _config.GetSection("AppSettings:MAIL_SMTP").Value;
            string SMTP_PASSWORD =  _config.GetSection("AppSettings:SMTP_PASSWORD").Value;
            string CORREO_PRUEBA =  _config.GetSection("AppSettings:PRUEBA_CORREO").Value;

            var cliente = await _repositoryCliente.Get(x=>x.Id== ordenSalidaForRegister.ClienteId);
            var direccion = await _repositoryDireccion.Get(x=>x.iddireccion == ordenSalidaForRegister.DireccionId);

            var createdUser = await _repo_OrdenSalida.RegisterOrdenSalida(ordenSalidaForRegister);


  // var cliente = await _repo_cliente.Get(x=>x.id == ordenTransporte.destinatario_id);
                // if(cliente.mail_notificacion != null || cliente.mail_notificacion != string.Empty)
                //         CORREO_PRUEBA = CORREO_PRUEBA + ";"  + cliente.mail_notificacion;

                // var provincia = _seguimiento.ObtenerProvincia(ordenTransporte.provincia_entrega.Value);

                // var vehiculo = await _repo_Vehiculo.Get(x => x.Id == equipotransporte.VehiculoId);
             
                
                string htmlString = @"<html>
                        <body>
                        <table border='0' cellspacing='0' cellpadding='0' width='600' style='width:450.0pt;border-collapse:collapse' id='m_-8533450555590531398title'><tbody><tr style='height:18.75pt'><td width='25' style='width:18.75pt;padding:0cm 0cm 0cm 0cm;height:18.75pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td style='padding:0cm 0cm 0cm 0cm;height:18.75pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td width='25' style='width:18.75pt;padding:0cm 0cm 0cm 0cm;height:18.75pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td width='25' style='width:18.75pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal' align='center' style='text-align:center'><strong><span style='font-size:16.5pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#db0414;letter-spacing:-.4pt'>Tu pedido ha sido registrado </span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif'><u></u><u></u></span></p></td><td width='25' style='width:18.75pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr style='height:21.0pt'><td width='25' style='width:18.75pt;padding:0cm 0cm 0cm 0cm;height:21.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td style='padding:0cm 0cm 0cm 0cm;height:21.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td width='25' style='width:18.75pt;padding:0cm 0cm 0cm 0cm;height:21.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table>
                        <table border='0' cellspacing='0' cellpadding='0' width='600' style='width:450.0pt;border-collapse:collapse' id='m_-8533450555590531398content'><tbody><tr><td width='25' style='width:18.75pt;border:none;border-bottom:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td style='border:none;border-bottom:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><div align='center'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse' id='m_-8533450555590531398inner-content'><tbody><tr><td valign='top' style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>

                        <span style='font-size:11.5pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                            Estimados: Grupo BCM   
                        </span>
                        
                        <span style='font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr><tr style='height:14.25pt'><td style='padding:0cm 0cm 0cm 0cm;height:14.25pt'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal' style='text-align:justify'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                       
                        <br>Se ha registrado un pedido para la atención según los siguientes datos:  <u></u><u></u></span></p></td></tr><tr style='height:14.25pt'><td style='padding:0cm 0cm 0cm 0cm;height:14.25pt'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td></tr></tbody></table></td></tr></tbody></table></td><td width='20' style='width:15.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td width='170' valign='top' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td style='padding:0cm 0cm 0cm 0cm'></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm;border-radius:3px'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr style='height:22.5pt'><td style='background:#db0414;padding:0cm 0cm 0cm 0cm;height:22.5pt'><p class='MsoNormal' align='center' style='text-align:center'><span style='font-family:&quot;Helvetica&quot;,sans-serif;color:black'> <a href='http://104.36.166.65/twh/#/pedido/verordenpedido/"+ createdUser + @"'><span style='font-size:11.5pt;color:white;text-decoration:none'>Ver mi pedido</span></a> </span><span style='font-family:&quot;Helvetica&quot;,sans-serif'><u></u><u></u></span></p></td></tr></tbody></table></td></tr><tr style='height:10.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:10.5pt'></td></tr></tbody></table></td></tr></tbody></table></div></td><td width='25' style='width:18.75pt;border:none;border-bottom:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table>
                        
                        <table border='0' cellspacing='0' cellpadding='0' width='100%' 
                        style='width:100.0%;border-collapse:collapse'><tbody><tr style='height:22.5pt'>
                        <td style='background:#db0414;padding:0cm 0cm 0cm 0cm;height:22.5pt'><p class='MsoNormal' 
                        align='center' style='text-align:center'><span style='font-family:&quot;Helvetica&quot;,sans-serif;color:black'>
                       
                       
                         <span style='font-family:&quot;Helvetica&quot;,sans-serif'><u></u><u></u></span></p></td></tr></tbody></table>
                        <p>   
                        </body>
                        <table border='0' cellspacing='0' cellpadding='0' width='600' style='width:450.0pt;border-collapse:collapse' id='m_-1387954621597574012order-details'><tbody><tr style='height:15.0pt'><td width='25' style='width:18.75pt;border:none;border-top:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm;height:15.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td style='border:none;border-top:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm;height:15.0pt'><div align='center'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse' id='m_-1387954621597574012inner-content'><tbody><tr><td valign='top' style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:11.5pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#db0414'>Datos del envío:</span></strong><span style='font-family:&quot;Helvetica&quot;,sans-serif'> <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td width='170' valign='top' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong>
                        <span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Fecha de Entrega:</span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr>
                        <tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'>
                        <span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>

                        " + Convert.ToDateTime(ordenSalidaForRegister.FechaRequerida).ToShortDateString() + @"
                        
                        <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table></td><td width='9' style='width:6.75pt;border:none;border-right:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td><td width='10' style='width:7.5pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td><td width='170' valign='top' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Hora de Entrega:</span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>

                        "+ ordenSalidaForRegister.HoraRequerida + @"
                        <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table></td><td width='9' style='width:6.75pt;border:none;border-right:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td></tr><tr><td width='170' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Domicilio de Entrega:</span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>

                        "+ direccion.direccion + @"
                        
                        <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table></td><td width='9' style='width:6.75pt;border:none;border-right:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td><td width='10' style='width:7.5pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td><td width='170' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Número de Teléfono:</span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr>
                        <tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp;<u></u><u></u></span></p></td></tr></tbody></table></td><td width='9' style='width:6.75pt;border:none;border-right:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td></tr><tr><td width='170' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Otros:</span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                        
                        
                        <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>&nbsp;<u></u><u></u></span></p></td></tr><tr style='height:12.0pt'><td style='padding:0cm 0cm 0cm 0cm;height:12.0pt;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                        # N° Pedido </span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                        "+  createdUser.ToString().PadLeft(7,'0')   + @" 
                        <u></u><u></u></span></p></td></tr><tr style='height:30.0pt'><td style='padding:0cm 0cm 0cm 0cm;height:30.0pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table></td><td width='9' style='width:6.75pt;border:none;border-right:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp;<u></u><u></u></p></td><td width='10' style='width:7.5pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp;<u></u><u></u></p></td><td width='170' valign='top' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>:</span></strong><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' style='word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td></tr>

                        <tr><td style='padding:0cm 0cm 0cm 0cm;height:30.0pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table></td><td width='9' style='width:6.75pt;border:none;border-right:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'>&nbsp;<u></u><u></u></p></td></tr></tbody></table></td></tr></tbody></table></td><td width='10' style='width:7.5pt;padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td width='170' valign='top' style='width:127.5pt;padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:11.5pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#db0414'>Razón social o Nombre:</span></strong><span style='font-family:&quot;Helvetica&quot;,sans-serif'> <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal' align='center' style='text-align:center'><span style='font-size:1.0pt'><img border='0' height='95' style='height:.9895in' id='m_-1387954621597574012_x0000_i1026' src='https://ci3.googleusercontent.com/proxy/Md5He3hPe9rJeJALU5ArtgdTYUhgRbRT82SpKo3pAwsTu13eV1QAzy4TPKwlF0qXMShaSEpjxx_5YBxs_B1wWxOVIUhyGRMq4U6hSKtL4-dw5Rx6nUxvWJv5YjjtlzNB-Rc-y2JjHA=s0-d-e1-ft#http://toscargo.e-strategit.com/Public/Imagenes/Chofer/fotochofer_06082019050819.png' alt='Foto conductor' class='CToWUd'><u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td valign='top' style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:9.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Nombre:</span></strong><span style='font-size:9.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td><td width='105' style='width:78.75pt;padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' align='right' style='text-align:right;word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>&nbsp;<u></u><u></u></span></p></td></tr></tbody></table></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt;word-wrap:break-word'><p class='MsoNormal' align='right' style='text-align:right;word-break:break-all'><span style='font-size:8.5pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                        "+    cliente.Nombre   +@"
                         <u></u><u></u></span></p></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td valign='top' style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:9.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Documento:</span></strong><span style='font-size:9.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td><td width='105' style='width:78.75pt;padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' align='right' style='text-align:right;word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                         "+ cliente.Documento +@"
                         <u></u><u></u></span></p></td></tr></tbody></table></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'>&nbsp; <u></u><u></u></p></td></tr><tr><td style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td valign='top' style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><strong><span style='font-size:9.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>Etiquetado:</span></strong><span style='font-size:9.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'> <u></u><u></u></span></p></td><td width='105' style='width:78.75pt;padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' align='right' style='text-align:right;word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>
                         " + (cliente.Etiquetado==true ? "Si" : "No") +@"
                          <u></u><u></u></span></p></td></tr></tbody></table></td></tr>
                        <tr><td style='padding:0cm 0cm 0cm 0cm'><table border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%;border-collapse:collapse'><tbody><tr><td valign='top' style='padding:0cm 0cm 0cm 0cm'><p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>&nbsp;<u></u><u></u></span></p></td><td width='105' style='width:78.75pt;padding:0cm 0cm 0cm 0cm;word-wrap:break-word'><p class='MsoNormal' align='right' style='text-align:right;word-break:break-all'><span style='font-size:10.0pt;font-family:&quot;Helvetica&quot;,sans-serif;color:#666666'>&nbsp;<u></u><u></u></span></p></td></tr></tbody></table></td></tr><tr style='height:7.5pt'><td style='padding:0cm 0cm 0cm 0cm;height:7.5pt'><p class='MsoNormal'><span style='font-size:1.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table></td></tr></tbody></table></div></td><td width='25' style='width:18.75pt;border:none;border-top:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm;height:15.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td></tr><tr style='height:15.0pt'><td width='25' style='width:18.75pt;border:none;border-bottom:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm;height:15.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td style='border:none;border-bottom:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm;height:15.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td><td width='25' style='width:18.75pt;border:none;border-bottom:solid #f2f2f2 1.0pt;padding:0cm 0cm 0cm 0cm;height:15.0pt'><p class='MsoNormal'><span style='font-size:10.0pt'>&nbsp; <u></u><u></u></span></p></td></tr></tbody></table>
                        </html>";   

                        string body =  "Se ha registrado una entrega (" +  createdUser + " ) para la OT " + ordenSalidaForRegister.DireccionId;
;
                        try
                        {
                                if(ordenSalidaForRegister.TipoRegistroId != 170) 
                                {
                                    MailHelper.EnviarMail(true,SMTP_SERVER,SMTP_MAIL,SMTP_PASSWORD,CORREO_PRUEBA,
                                        "", "TWH : Su pedido N°: " + createdUser.ToString().PadLeft(7,'0') + " ha sido registrado.", htmlString,true );
                                }
                        }
                        catch(Exception ex)
                        {

                        }

            return Ok(createdUser);
      }
      [HttpPost("UpdateOrdenSalida")]
      public async Task<IActionResult> UpdateOrdenSalida(OrdenSalidaForRegister ordenSalidaForRegister)
      {
          


         var updatedOrdenSalida =    await _repository.Get(x=>x.Id == ordenSalidaForRegister.Id);
         updatedOrdenSalida.AlmacenId = ordenSalidaForRegister.AlmacenId;
         updatedOrdenSalida.ClienteId = ordenSalidaForRegister.ClienteId;
         updatedOrdenSalida.DireccionId = ordenSalidaForRegister.DireccionId;
         updatedOrdenSalida.FechaRequerida = Convert.ToDateTime(ordenSalidaForRegister.FechaRequerida);
         updatedOrdenSalida.HoraRequerida = ordenSalidaForRegister.HoraRequerida;
         updatedOrdenSalida.GuiaRemision = ordenSalidaForRegister.GuiaRemision;
         updatedOrdenSalida.OrdenCompraCliente = ordenSalidaForRegister.OrdenCompraCliente;

         await _repository.SaveAll();
            
         return Ok(updatedOrdenSalida);
      }
        [HttpPost("UploadFile")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(int usrid)
        {
          // var seguimiento = new Seguimiento();
            try
            {
                // Grabar Excel en disco
                string fullPath =  SaveFile(0);
                // Leer datos de excel
                var celdas = GetExcel(fullPath);
                // Generar entidades
                var entidades = ObtenerEntidades_CargaMasiva(celdas);
                // Grabar entidades en base de datos
                
               var carga = new CargaMasivaForRegister();
               carga.estado_id = 1;
               carga.fecha_registro = DateTime.Now;
               carga.ordensalidaid = usrid;
            
                var resp =  await _repo_OrdenSalida.RegisterCargaMasiva(carga, entidades);

                //Generar Ordenes de trabajo y Manifiestos      
            //    var detalles_cargados =  _repo_CargaMasiva.GetAll(x=>x.carga_id == resp).Result;


            //    var lista = detalles_cargados.ToList();
               // var manifiestos = _seguimiento.ObtenerEntidades_Manifiesto(lista);
                //Registrar manifiestos 
                //await _repo.RegisterOrdenes(manifiestos,usrid);

                

            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
                throw ex;
              
            }
            return Ok();
         }
        public List<List<string>> GetExcel(string fullPath)
        {     
             List<List<string>> valores = new List<List<string>>();
            try
            {
                
                using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fullPath, false))
                {
                    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string relationshipId = sheets.First().Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();
                    // foreach (Cell cell in rows.ElementAt(0))
                    // {
                    //     dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                    // }
                    foreach (Row row in rows) //this will also include your header row...
                    {
                         List<String> linea = new List<string>();
                        int columnIndex = 0;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            // Gets the column index of the cell with data
                            int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                            cellColumnIndex--; //zero based index
                            if (columnIndex < cellColumnIndex)
                            {
                                do
                                {
                                    linea.Add(""); //Insert blank data here;
                                    columnIndex++;
                                }
                                while (columnIndex < cellColumnIndex);
                            }
                             linea.Add(GetCellValue(spreadSheetDocument, cell));
                          
                            columnIndex++;
                        }
                        valores.Add(linea);
                    }
                }
               // dt.Rows.RemoveAt(0); //...so i'm taking it out here.
                     return valores;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         public List<CargaMasivaDetalleForRegister> ObtenerEntidades_CargaMasiva(List<List<String>> data) 
        {
                data = validar_fin(data);
                var totales = new List<CargaMasivaDetalleForRegister>();
                CargaMasivaDetalleForRegister linea ;

                foreach (var item in data.Skip(1))
                {
                    linea =  new CargaMasivaDetalleForRegister();
                    linea.referencia = ValidarRequerido(item[0] , "referencia" );
                
                
                    totales.Add(linea);
                    
                }
                return totales;
        }
   private string ValidarRequerido(string v, string field)
        {
            if(String.IsNullOrEmpty(v))
            {
              throw new ArgumentException( $" {field} no puede estar en blanco .");
            }
            return v;
        }

       
        private List<List<string>> validar_fin(List<List<string>> data)
        {
            List<List<string>> new_data = new List<List<string>>();
            foreach (var item in data)
            {
                if(item[0] == "" && item[2] == ""){
                    break;
                }
                else 
                new_data.Add(item);
                
            }
            return new_data;
        }

        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
      
        public static int? GetColumnIndexFromName(string columnName)
        {
                       
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue ==null)
            {
            return "";
            }
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        private  String SaveFile(long usuario_id)
        {
            
            var fullPath = string.Empty;
          
            
            var ruta =  _config.GetSection("AppSettings:Uploads").Value;

            var file = Request.Form.Files[0];
            var idOrden = usuario_id;

            string folderName = idOrden.ToString();
            string webRootPath = ruta ;
            string newPath = Path.Combine(webRootPath, folderName);

            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(Request.Form.Files[0].OpenReadStream()))
            {
                fileData = binaryReader.ReadBytes(Request.Form.Files[0].ContentDisposition.Length);
            }

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                fullPath = Path.Combine(newPath, fileName);

                var checkextension = Path.GetExtension(fileName).ToLower();
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {

                    file.CopyTo(stream);
                    
                }

            }
            return fullPath;

        }

      [HttpPost("RegisterSalidaShipment")]
      public async Task<IActionResult> RegisterSalidaShipment(CargaForRegister ordenSalidaForRegister)
      {
            var createdUser = await _repo_OrdenSalida.RegisterSalida(ordenSalidaForRegister);
            return Ok(createdUser);
      }
      
      [HttpPost("register_detail")]
      public async Task<IActionResult> Register_Detail(OrdenSalidaDetalleForRegister ordenReciboDetalleForRegisterDto)
      {
            var resp = await _repo_OrdenSalida.RegisterOrdenSalidaDetalle(ordenReciboDetalleForRegisterDto);
            return Ok(resp);
      }
      [HttpPost("PlanificarPicking")]
      public async Task<IActionResult> PlanificarPicking(PickingPlan model)
      {
            var resp = await _repo_OrdenSalida.PlanificarPicking(model);
            return Ok(resp);
      }
      [HttpPost("RegisterCarga")]
      public async Task<IActionResult> RegisterCarga(CargaForRegister model)
      {
            var resp = await _repo_OrdenSalida.RegisterCarga(model);
            return Ok(resp);
      }
    [HttpPost("MatchTransporteCarga")]
    public async Task<IActionResult> MatchTransporteOrdenIngreso(MatchCargaEquipoTransporte matchTransporteCarga)
    {
        var createdEquipoTransporte = await _repo_OrdenSalida.matchTransporteCarga(matchTransporteCarga.CargasId,matchTransporteCarga.EquipoTransporteId);
        return Ok(createdEquipoTransporte);
    }

    
      [HttpDelete("EliminarPlanificacion")]
      public async Task<IActionResult> EliminarPlanificacion(long Id)
      { 
          var resp  =  await _repo_OrdenSalida.EliminarPlanificacion(Id);
          return Ok (resp);
      }
#endregion


    }
}