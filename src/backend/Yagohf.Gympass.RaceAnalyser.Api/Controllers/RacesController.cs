using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Swagger.Attributes;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Model;
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
        /// Consulta os tipos de corridas existentes.
        /// </summary>
        [HttpGet("types")]
        [SwaggerResponse(200, typeof(IEnumerable<RaceTypeDTO>))]
        public async Task<IActionResult> GetRaceTypes()
        {
            return Ok(await this._raceService.ListRaceTypesAsync());
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
        /// <param name="model">Dados da corrida.</param>
        /// <param name="file">Arquivo com os resultados da corrida</param>
        [HttpPost]
        [SwaggerResponse(201, typeof(RaceResultDTO))]
        [SwaggerResponse(400)]
        [SwaggerConsumes("application/x-www-form-urlencoded", "multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Post([FromForm]CreateRaceDTO model, [FromForm]IFormFile file)
        {
            FileDTO fileDTO = new FileDTO();
            if (file != null && file.Length > 0)
            {
                fileDTO.Extension = file.FileName.Split('.').Last();
                fileDTO.ContentType = file.ContentType;
                fileDTO.Content = new MemoryStream();
                await file.CopyToAsync(fileDTO.Content);
            }

            RaceResultDTO raceResult = await this._raceService.AnalyseAsync(model, fileDTO, this.GetLoggedUser());
            return CreatedAtAction(nameof(GetResultById), new { id = raceResult.RaceId }, raceResult);
        }

        /// <summary>
        /// Obtém o arquivo de exemplo para upload de dados das corridas.
        /// </summary>
        [HttpGet("example")]
        [SwaggerResponse(200, typeof(byte[]))]
        public async Task<IActionResult> GetExample()
        {
            FileDTO example = await this._raceService.GetExampleFileAsync();
            return File(example.Content, example.ContentType, example.Name);
        }
    }
}
