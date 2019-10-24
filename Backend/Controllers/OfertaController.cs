using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaController : ControllerBase
    {
        InstitutoFriggaContext _context = new InstitutoFriggaContext();

        /// <summary>
        /// Mostra lista de tipos de usuários
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Oferta>>> Get()
        {
            var oferta = await _context.Oferta.ToListAsync();

            if(oferta == null)
            {
                return NotFound();
            }
            
            return oferta;
        }
        /// <summary>
        /// Mostra tipo de usuário por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Oferta>> Get(int id)
        {
            var oferta = await _context.Oferta.FindAsync(id);

            if(oferta == null)
            {
                return NotFound();
            }

            return oferta;
        }
        /// <summary>
        /// Insere dados em Oferta
        /// </summary>
        /// <param name="oferta"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Oferta>> Post(Oferta oferta)
        {
        
            try
            {
                UploadController upload =  new UploadController();
                oferta.ImagemProduto = upload.Upload();
                
            
                await _context.AddAsync(oferta);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            return oferta;
        }
        
        /// <summary>
        /// Atualiza dados em Tipo Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oferta"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id , Oferta oferta)
        {
            if (id != oferta.OfertaId)
            {
                return BadRequest();
            }

            _context.Entry(oferta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                var oferta_valido = await _context.Oferta.FindAsync();

                if(oferta_valido == null)
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
        public async Task<ActionResult<Oferta>> Delete(int id)
        {
            var oferta = await _context.Oferta.FindAsync(id);
            if(oferta == null)
            {
                return NotFound();
            }
            _context.Oferta.Remove(oferta);
            await _context.SaveChangesAsync();

            return oferta;
        }
    }
}