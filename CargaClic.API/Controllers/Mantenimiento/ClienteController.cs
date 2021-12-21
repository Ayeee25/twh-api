using System.Threading.Tasks;
using CargaClic.Data.Interface;
using CargaClic.Domain.Mantenimiento;
using CargaClic.ReadRepository.Interface.Mantenimiento;
using CargaClic.Repository.Contracts.Mantenimiento;
using CargaClic.Repository.Interface.Mantenimiento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargaClic.API.Controllers.Mantenimiento
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IRepository<Cliente> _repo;
        private readonly IRepository<Propietario> _repo_propietario;
        private readonly IRepository<ClientePropietario> _repo_ClientePropietario;
        private readonly IMantenimientoRepository _repository;
        private readonly IClienteRepository _repository_Cliente;

        public ClienteController(IRepository<Cliente> repo,
         IMantenimientoRepository repository,
         IRepository<Propietario> repo_propietario,
         IClienteRepository repository_cliente, 
         IRepository<ClientePropietario> repo_ClientePropietario)
        {
            _repo = repo;
            _repository = repository;
            _repository_Cliente = repository_cliente;
            _repo_propietario = repo_propietario;
            _repo_ClientePropietario = repo_ClientePropietario;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await  _repo.Get(x=> x.Id == id);
            return Ok(usuario);
        }
        [HttpGet("GetPropietario")]
        public async Task<IActionResult> GetPropietario(int id)
        {
            var usuario = await  _repo_propietario.Get(x=> x.Id == id);
            return Ok(usuario);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var usuarios = await  _repo.GetAll();
            return Ok(usuarios);
        }
        [HttpGet("GetAllPropietarios")]
        public async Task<IActionResult> GetAllPropietarios(string criterio)
        {
            var result = await _repository.GetAllPropietarios(criterio);
            return Ok(result);
        }
        [HttpPost("DeleteCliente")]
        public async Task<IActionResult> DeleteCliente(int ClientePropietarioId)
        {
            var clientePropietario = await _repo_ClientePropietario.Get(x => x.Id  == ClientePropietarioId);

            _repo_ClientePropietario.Delete(clientePropietario);   

            await  _repo_ClientePropietario.SaveAll();

            return Ok();
        }
        

        [HttpGet("GetAllClientes")]
        public async Task<IActionResult> GetAllClientes(string criterio)
        {
            var result = await _repository.GetAllClientes(criterio);
            return Ok(result);
        }
        [HttpGet("GetAllClientesxPropietarios")]
        public async Task<IActionResult> GetAllClientesxPropietarios(int id)
        {
            var result = await _repository.GetAllClientesxPropietarios(id);
            return Ok(result);
        }
        [HttpPost("ClientRegister")]
        public async Task<IActionResult> ClientRegister(ClienteForRegister model)
        {
            var result = await _repository_Cliente.ClientRegister(model);
            return Ok(result);
        }
        [HttpPost("OwnerRegister")]
        public async Task<IActionResult> OwnerRegister(OwnerForRegister model)
        {
            var result = await _repository_Cliente.OwnerRegister(model);
            return Ok(result);
        }
        [HttpPost("OwnerEdit")]
        public async Task<IActionResult> OwnerEdit(OwnerForRegister model)
        {
            var clienteForEdit = await _repo_propietario.Get(x=> x.Id == model.Id);
            clienteForEdit.Documento = model.Documento;
            clienteForEdit.TipoDocumentoId = model.TipoDocumentoId;
            clienteForEdit.Nombre  = model.Nombre;
            clienteForEdit.NombreCorto  = model.NombreCorto;

            await _repo_propietario.SaveAll();

           // var result = await _repository_Cliente.OwnerRegister(model);
            return Ok(clienteForEdit);
        }
        [HttpPost("MatchOwnerClient")]
        public async Task<IActionResult> MatchOwnerClient(OwnerClientForAttach model)
        {
            var result = await _repository_Cliente.AttachOwnerClient(model);
            return Ok(result);
        }
        [HttpGet("GetAllDirecciones")]
        public async Task<IActionResult> GetAllDirecciones(int Id)
        {
            var resp = await  _repository.GetAllDirecciones(Id);
            return Ok(resp);
        }
        [HttpPost("AddressRegister")]
        public async Task<IActionResult> AddressRegister(AddressForRegister model)
        {
            var result = await _repository_Cliente.AddressRegister(model);
            return Ok(result);
        }
        [HttpGet("GetAllDepartamentos")]
        public async Task<IActionResult> GetAllDepartamentos()
        {
            var resp = await  _repository.GetAllDepartamentos();
            return Ok(resp);
        }
        [HttpGet("GetAllProvincias")]
        public async Task<IActionResult> GetAllProvincias(int DepartamentoId)
        {
            var resp = await  _repository.GetAllProvincias(DepartamentoId);
            return Ok(resp);
        }
        [HttpGet("GetAllDistritos")]
        public async Task<IActionResult> GetAllDistritos(int ProvinciaId)
        {
            var resp = await  _repository.GetAllDistritos(ProvinciaId);
            return Ok(resp);
        }
    }
}

