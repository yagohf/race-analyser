using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Exception;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper;

namespace Yagohf.Gympass.RaceAnalyser.Services.Domain
{
    public class RaceService : IRaceService
    {
        private readonly IRaceFileHelper _raceFileProcessor;
        private readonly IRaceRepository _raceRepository;
        private readonly IRaceTypeRepository _raceTypeRepository;
        private readonly IRaceTypeQuery _raceTypeQuery;
        private readonly IUserRepository _userRepository;
        private readonly IUserQuery _userQuery;
        private readonly ILapRepository _lapRepository;

        public RaceService(IRaceFileHelper raceFileProcessor,
                           IRaceRepository raceRepository,
                           IRaceTypeRepository raceTypeRepository,
                           IRaceTypeQuery raceTypeQuery,
                           IUserRepository userRepository,
                           IUserQuery userQuery,
                           ILapRepository lapRepository)
        {
            this._raceFileProcessor = raceFileProcessor;
            this._raceRepository = raceRepository;
            this._raceTypeRepository = raceTypeRepository;
            this._raceTypeQuery = raceTypeQuery;
            this._userRepository = userRepository;
            this._userQuery = userQuery;
            this._lapRepository = lapRepository;
        }

        public async Task<RaceResultDTO> AnalyseAsync(CreateRaceDTO createData, string uploader)
        {
            //Validar minimamente os dados de input.
            if (createData == null || uploader == null)
                throw new Exception("Dados inválidos para análise.");
            else if (!this.ValidateRequiredFields(createData, out string requiredFieldsErrorMessage))
                throw new BusinessException(requiredFieldsErrorMessage);

            //Processar o arquivo.
            await this._raceFileProcessor.Process(createData.ResultsFile);

            //Tratar retorno.
            if (!this._raceFileProcessor.Success)
                throw new BusinessException(this._raceFileProcessor.ErrorMessage);
            else
                return await this.SaveProcessedData(createData, uploader, this._raceFileProcessor.Results);
        }

        public Task<RaceResultDTO> GetResultByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Listing<RaceSummaryDTO>> ListSummaryAsync(string description, string uploader, int? page)
        {
            throw new System.NotImplementedException();
        }

        #region [ Helpers ]

        private async Task<RaceResultDTO> SaveProcessedData(CreateRaceDTO createData, string uploader, IEnumerable<Lap> laps)
        {
            //Recuperar usuário da corrida.
            User user = await this._userRepository.GetSingleAsync(this._userQuery.ByLogin(uploader));

            using (var ts = new TransactionScope())
            {
                //Salvar corrida.
                Race race = new Race();
                race.Date = createData.Date;
                race.Description = createData.Description;
                race.TotalLaps = createData.TotalLaps;
                race.UploadDate = DateTime.Now;
                race.UploaderId = user.Id;

                await this._raceRepository.InsertAsync(race);

                //Salvar cada uma das voltas após atualizar o ID da corrida.
                foreach (var lap in laps)
                {
                    lap.RaceId = race.Id;
                    await this._lapRepository.InsertAsync(lap);
                }

                //Commitar transação.
                ts.Complete();

                return await this.GetResultByIdAsync(race.Id);
            }
        }

        private bool ValidateRequiredFields(CreateRaceDTO createData, out string requiredFieldsErrorMessage)
        {
            requiredFieldsErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(createData.Description))
                requiredFieldsErrorMessage += "Descrição não informada;";
            else if (createData.TotalLaps < 1)
                requiredFieldsErrorMessage += "Número de voltas inválido;";
            else if (createData.ResultsFile == null || createData.ResultsFile.Length == 0)
                requiredFieldsErrorMessage += "Arquivo inválido para análise;";
            else if (!this._raceTypeRepository.Exists(this._raceTypeQuery.ById(createData.RaceTypeId)))
                requiredFieldsErrorMessage += "Tipo de corrida inválido.";

            return string.IsNullOrEmpty(requiredFieldsErrorMessage);
        }

        #endregion
    }
}
