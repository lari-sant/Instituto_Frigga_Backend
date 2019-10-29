using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domains;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "1")]
    public class EnderecoController : ControllerBase
    {
        EnderecoRepository repositorio = new EnderecoRepository();

        /// <summary>
        /// Mostra lista de tipos de usuários
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Endereco>>> Get()
        {
            var endereco = await repositorio.Listar();

            if(endereco == null)
            {
                return NotFound();
            }
            
            return endereco;
        }
        /// <summary>
        /// Mostra tipo de usuário por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize (Roles = "1")]
        public async Task<ActionResult<Endereco>> Get(int id)
        {
            var endereco = await repositorio.BuscarPorId(id);

            if(endereco == null)
            {
                return NotFound();
            }

            return endereco;
        }
        /// <summary>
        /// Insere dados em Endereco
        /// </summary>
        /// <param name="endereco"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize (Roles = "1")]
        [Authorize (Roles = "2")]
        [Authorize (Roles = "3")]
        public async Task<ActionResult<Endereco>> Post(Endereco endereco)
        {
            try
            {
                await repositorio.Salvar(endereco);
                return endereco;
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            
        }
        /// <summary>
        /// Atualiza dados em Tipo Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="endereco"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize (Roles = "1")]
        [Authorize (Roles = "2")]
        [Authorize (Roles = "3")]
        public async Task<ActionResult> Put(int id , Endereco endereco)
        {
            if (id != endereco.EnderecoId)
            {
                return BadRequest();
            }

            try
            {
                await repositorio.Alterar(endereco);
            }
            catch(DbUpdateConcurrencyException)
            {
                var endereco_valido = await repositorio.BuscarPorId(id);

                if(endereco_valido == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return Accepted();
        }
        
        /// <summary>
        /// Deleta dados em Tipo usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize (Roles = "1")]
        [Authorize (Roles = "2")]
        [Authorize (Roles = "3")]
        public async Task<ActionResult<Endereco>> Delete(int id)
        {
            var endereco = await repositorio.BuscarPorId(id);
            if(endereco == null)
            {
                return NotFound();
            }
            endereco = await repositorio.Excluir(endereco);

            return endereco;
        }
    }
}