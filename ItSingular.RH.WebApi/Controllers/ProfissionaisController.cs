using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItSingular.RH.Application.DTOs;
using ItSingular.RH.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItSingular.RH.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionaisController : ControllerBase
    {
        private readonly IProfissionaisService _iprofissionaisService;


        public ProfissionaisController(IProfissionaisService profissionaisService)
        {
            _iprofissionaisService = profissionaisService;
        }

        // GET: api/Profissionais
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "CorsPolicy", Roles = "Administradores")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize("Bearer", Roles = "Administradores")]
        //[Authorize("Bearer")]
        [HttpGet("ListaDeProfissionais")]
        public async Task<IEnumerable<ProfissionaisDTO>> GetListaDeProfissionais()
        {

            var list = await _iprofissionaisService.GetProfissionaisList();

            return list.OrderBy(p => p.Nome);

            //return new string[] { "value1", "value2" };
        }

        // GET: api/Profissionais/5
        [Authorize("Bearer")]
        [HttpGet("{id}", Name = "Get")]
        public async Task<ProfissionaisDTO> Get(int id)
        {
            var retorno = await _iprofissionaisService.GetProfissionaisById(id);

            return retorno;
        }

        // POST: api/Profissionais
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Profissionais/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
