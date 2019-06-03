using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;

namespace Yagohf.Gympass.RaceAnalyser.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class RacesController : Controller
    {
        private readonly IRaceService _raceService;

        public RacesController(IRaceService raceService)
        {
            this._raceService = raceService;
        }

        /// <summary>
        /// Consulta uma lista de resumos de corridas a partir de alguns parâmetros, sempre
        /// em ordem decrescente de data. Permite paginação.
        /// </summary>
        /// <param name="description">Descrição das corridas.</param>
        /// <param name="uploader">Usuário que fez o upload das corridas.</param>
        /// <param name="page">Página da listagem a ser exibida.</param>
        [HttpGet]
        [SwaggerResponse(200, typeof(Listing<RaceSummaryDTO>))]
        [SwaggerResponse(401)]
        public async Task<IActionResult> Get(string description, string uploader, int? page)
        {
            return Ok(await this._raceService.ListSummaryAsync(description, uploader, page));
        }

        /// <summary>
        /// Consulta os detalhes de uma corrida específica.
        /// </summary>
        /// <param name="id">Identificador único da corrida.</param>
        [HttpGet("{id}/result")]
        [SwaggerResponse(200, typeof(RaceResultDTO))]
        public async Task<IActionResult> GetResultById(int id)
        {
            return Ok(await this._raceService.GetResultByIdAsync(id));
        }

        /// <summary>
        /// Submete o resultado de uma corrida para análise.
        /// </summary>
        /// <param name="file">Arquivo com os resultados da corrida.</param>
        /// <param name="model">Dados da corrida.</param>
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(201, typeof(RaceResultDTO))]
        public async Task<IActionResult> Post(IFormFile file)
        {
            //TODO - substituir por model binding customizado, que permite enviar IFormFile junto com uma model.
            MemoryStream ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            CreateRaceDTO model = new CreateRaceDTO()
            {
                Date = DateTime.Now,
                Name = $"Race { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }",
                RaceTypeId = 1,
                TotalLaps = new Random().Next(1, 5),
                ResultsFile = ms
            };

            RaceResultDTO raceResult = await this._raceService.AnalyseAsync(model, this.GetLoggedUser());
            return CreatedAtAction(nameof(GetResultById), new { id = raceResult.RaceId }, raceResult);
        }
    }
}
