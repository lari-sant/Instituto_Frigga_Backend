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
    public class UsuarioController : ControllerBase
    {
        UsuarioRepository repositorio = new UsuarioRepository();

        /// <summary>
        /// Mostra listande usuários
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize (Roles = "1")]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var usuario = await repositorio.Listar();

            if(usuario == null)
            {
                return NotFound();
            }
            
            return usuario;
        }
        /// <summary>
        /// Exibe usuário por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize (Roles = "1")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var usuario = await repositorio.BuscarPorId(id);

            if(usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }
        /// <summary>
        /// Insere dados em Usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario)
        {
            try
            {
                bool ValidaDoc = false;
                if( usuario.CnpjCpf.Length == 11)
                {
                    ValidaDoc = ValidaCpf(usuario.CnpjCpf);
                }
                if( usuario.CnpjCpf.Length == 14 )
                {
                    ValidaDoc = ValidaCnpj(usuario.CnpjCpf);
                }     
                

                if(ValidaDoc == false)
                {
                    return BadRequest();
                }
                await repositorio.Salvar(usuario);
                return usuario;
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            
        }

        public bool ValidaCpf(string cpfUsuario)
        {
            cpfUsuario = cpfUsuario.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "");
            bool resultado    = false;
            int[] v1 = {10, 9, 8, 7, 6, 5, 4, 3, 2};

            string cpfCalculo = "";
            int resto         = 0;
            int calculo       = 0;

            string digito_v1 = "";
            string digito_v2 = "";

            cpfCalculo = cpfUsuario.Substring(0 , 9);

            for(int i = 0; i <= 8; i++){
                
                calculo = calculo + int.Parse(cpfUsuario[i].ToString() )* v1[i];
            }
           
            resto = calculo % 11;
            calculo = 11 - resto;

            if(calculo > 9 ){
                 digito_v1 = "0";
                
            }else{
                digito_v1 = calculo.ToString();
            }
            if( digito_v1 == cpfUsuario[9].ToString() ){
                resultado = true;

            }

            int[] v2 = {11, 10, 9, 8, 7 , 6, 5 , 4, 3 , 2};
            resto = 0;
            
            cpfCalculo = cpfCalculo + calculo;

            calculo = 0;

            for(int i = 0; i <= 9; i++){
                
                calculo = calculo + int.Parse(cpfUsuario[i].ToString() )* v2[i];
            }

            resto = calculo % 11;
            calculo = 11 - resto;

            if(calculo > 9 ){
                 digito_v2 = "0";
                
            }else{
                digito_v2 = calculo.ToString();
            }
            if( digito_v2 == cpfUsuario[9].ToString() ){
                resultado = true;

            }

            return resultado;
        }

        public bool ValidaCnpj(string cnpjUsuario){

            bool resultado = false;

            int[] v1 = {5 , 4 , 3 , 2 , 9 , 8 , 7 , 6 , 5 , 4 , 3 , 2};
            
            string cnpjCalculo = "";
            int resto = 0;
            int calculo = 0;

            string digito_v1 = "";
            string digito_v2 = "";

            cnpjUsuario = cnpjUsuario.Replace(" ", "");
            cnpjUsuario = cnpjUsuario.Replace(".", "");
            cnpjUsuario = cnpjUsuario.Replace("-", "");
            cnpjUsuario = cnpjUsuario.Replace("/", "");

            cnpjCalculo = cnpjUsuario.Substring(0 , 13);

            

            for(int i = 0; i <= 11; i++){

                calculo = calculo + int.Parse(cnpjUsuario[i].ToString() )* v1[i];

            }

            resto = calculo % 11;
            calculo = 11 - resto;
           
            if(resto < 2){

                digito_v1 = "0";

            }else{
                digito_v1 = calculo.ToString();
            }
            if(digito_v1 == cnpjUsuario[12].ToString() ){

                resultado = true;
            }

            int[] v2 = {6 , 5 , 4 , 3 , 2 , 9 , 8 , 7 , 6 , 5 , 4 , 3 , 2};

            cnpjCalculo = cnpjCalculo + calculo;

            calculo = 0;



            for(int i = 0; i <= 12; i++){

                calculo = calculo + int.Parse(cnpjUsuario[i].ToString() )* v2[i];
            }

            resto = calculo % 11;
            calculo = 11 - resto;
           
            if(resto < 2){

                digito_v2 = "0";

            }else{
                digito_v2 = calculo.ToString();
            }
            if(digito_v2 == cnpjUsuario[13].ToString() ){

                resultado = true;
            }



            return  resultado;
        }
        /// <summary>
        /// Atualiza dados de Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize (Roles = "1, 2, 3")]
        public async Task<ActionResult> Put(int id , Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }

            try
            {
                await repositorio.Alterar(usuario);
            }
            catch(DbUpdateConcurrencyException)
            {
                var usuario_valido = await repositorio.BuscarPorId(id);

                if(usuario_valido == null)
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
            
            return Accepted();
        }
        
        /// <summary>
        /// Deleta dados de Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize (Roles = "1")]
        public async Task<ActionResult<Usuario>> Delete(int id)
        {
            var usuario = await repositorio.BuscarPorId(id);
            if(usuario == null)
            {
                return NotFound();
            }
            usuario = await repositorio.Excluir(usuario);

            return usuario;
        }
    }
}