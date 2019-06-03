﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;

namespace Yagohf.Gympass.RaceAnalyser.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// Consulta os detalhes de um usuário específico.
        /// </summary>
        /// <param name="id">Identificador único do usuário.</param>
        [HttpGet("{id}")]
        [SwaggerResponse(200, typeof(UserDTO))]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await this._userService.GetByIdAsync(id));
        }

        /// <summary>
        /// Registra um usuário.
        /// </summary>
        /// <param name="model">Dados do novo usuário.</param>
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(201, typeof(UserDTO))]
        public async Task<IActionResult> Post([FromBody]RegistrationDTO model)
        {
            UserDTO newUser = await this._userService.RegisterAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        /// <summary>
        /// Obtém um token a partir de credenciais válidas de autenticação.
        /// </summary>
        /// <param name="model">Credenciais para autenticação.</param>
        [AllowAnonymous]
        [HttpPost("token")]
        [SwaggerResponse(201, typeof(UserDTO))]
        public async Task<IActionResult> PostAuth([FromBody]AuthenticationDTO model)
        {
            TokenDTO token = await this._userService.GenerateTokenAsync(model);
            return Ok(token);
        }
    }
}
