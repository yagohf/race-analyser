using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Authentication;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;

namespace Yagohf.Gympass.RaceAnalyser.Api.Controllers
{
    [Route("api/v1/[controller]")]
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
        /// <param name="login">Login do usuário.</param>
        [HttpGet("{login}")]
        [SwaggerResponse(200, typeof(UserDTO))]
        public async Task<IActionResult> GetByLogin(string login)
        {
            return Ok(await this._userService.GetByLoginAsync(login));
        }

        /// <summary>
        /// Registra um usuário.
        /// </summary>
        /// <param name="model">Dados do novo usuário.</param>
        [HttpPost]
        [SwaggerResponse(201, typeof(UserDTO))]
        public async Task<IActionResult> Post([FromBody]RegistrationDTO model)
        {
            UserDTO newUser = await this._userService.RegisterAsync(model);
            return CreatedAtAction(nameof(GetByLogin), new { login = newUser.Login }, newUser);
        }

        /// <summary>
        /// Obtém um token a partir de credenciais válidas de autenticação.
        /// </summary>
        /// <param name="model">Credenciais para autenticação.</param>
        [HttpPost("token")]
        [SwaggerResponse(201, typeof(UserDTO))]
        public async Task<IActionResult> PostAuth([FromBody]AuthenticationDTO model)
        {
            TokenDTO token = await this._userService.GenerateTokenAsync(model);
            return Ok(token);
        }
    }
}
