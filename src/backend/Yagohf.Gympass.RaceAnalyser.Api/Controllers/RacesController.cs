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
    public class RacesController : Controller
    {
        private readonly IRaceService _raceService;

        public RacesController(IRaceService raceService)
        {
            this._raceService = raceService;
        }

        /// <summary>
        /// Consulta uma lista de resumos de corridas a partir de alguns parâmetros, sempre
        /// em ordem decrescente de data de realização. Permite paginação.
        /// </summary>
        /// <param name="description">Descrição das corridas.</param>
        /// <param name="page">Página da listagem a ser exibida.</param>
        [HttpGet]
        [SwaggerResponse(200, typeof(Listing<RaceSummaryDTO>))]
        public async Task<IActionResult> Get(string description, int? page)
        {
            return Ok(await this._raceService.ListSummaryAsync(description, page));
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
        [HttpPost]
        [SwaggerResponse(201, typeof(RaceResultDTO))]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]CreateRaceDTO model)
        {
            //TODO - substituir por model binding customizado, que permite enviar IFormFile junto com uma model.
            MemoryStream ms = new MemoryStream();
            using (FileStream fs = new FileStream(@"C:\Dev\race-analyser\src\simulations\ITALY_1.txt", FileMode.Open))
            {
                await fs.CopyToAsync(ms);
            }

            model.ResultsFile = ms;
            RaceResultDTO raceResult = await this._raceService.AnalyseAsync(model, this.GetLoggedUser());
            return CreatedAtAction(nameof(GetResultById), new { id = raceResult.RaceId }, raceResult);
        }
    }
}
