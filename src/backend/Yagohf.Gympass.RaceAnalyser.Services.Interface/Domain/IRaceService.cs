using System.Collections.Generic;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Model;
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
        /// <param name="page">Página da consulta.</param>
        /// <returns>Lista de corridas (resumidas).</returns>
        Task<Listing<RaceSummaryDTO>> ListSummaryAsync(string description, int? page);

        /// <summary>
        /// Analisa os resultados de uma corrida, disponibilizando para os demais usuários
        /// quando os dados são válidos.
        /// </summary>
        /// <param name="createData">Dados para criação da corrida.</param>
        /// <param name="file">Arquivo da corrida.</param>
        /// <param name="uploader">Usuário fazendo upload da corrida para análise.</param>
        /// <returns>Resultado analisado da corrida.</returns>
        Task<RaceResultDTO> AnalyseAsync(CreateRaceDTO createData, FileDTO file, string uploader);

        /// <summary>
        /// Recupera o arquivo de exemplo para upload de corridas.
        /// </summary>
        /// <returns>Arquivo e suas informações.</returns>
        Task<FileDTO> GetExampleFileAsync();

        /// <summary>
        /// Lista os tipos de corridas existentes.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RaceTypeDTO>> GetRaceTypes();
    }
}
