using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;

namespace Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain
{
    public interface IRaceService
    {
        /// <summary>
        /// Obtém o resultado de uma corrida através de seu identificador único.
        /// </summary>
        /// <param name="id">Identificador único da corrida.</param>
        /// <returns>Resultado.</returns>
        Task<RaceResultDTO> GetResultByIdAsync(int id);

        /// <summary>
        /// Obtém uma lista de corridas (resumidas) através de múltiplos parâmetros.
        /// </summary>
        /// <param name="description">Descrição da corrida.</param>
        /// <param name="uploader">ID do usuário que enviou a análise.</param>
        /// <param name="page">Página da consulta.</param>
        /// <returns>Lista de corridas (resumidas).</returns>
        Task<Listing<RaceSummaryDTO>> ListSummaryAsync(string description, string uploader, int? page);

        /// <summary>
        /// Analisa os resultados de uma corrida, disponibilizando para os demais usuários
        /// quando os dados são válidos.
        /// </summary>
        /// <param name="createData">Dados para criação da corrida.</param>
        /// <param name="uploader">Usuário fazendo upload da corrida para análise.</param>
        /// <returns>Resultado analisado da corrida.</returns>
        Task<RaceResultDTO> AnalyseAsync(CreateRaceDTO createData, string uploader);
    }
}
