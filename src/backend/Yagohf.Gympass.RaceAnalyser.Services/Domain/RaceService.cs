using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Queries;
using Yagohf.Gympass.RaceAnalyser.Data.Interface.Repositories;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Exception;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Extensions;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging;
using Yagohf.Gympass.RaceAnalyser.Model.DTO.Race;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Domain;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper;

namespace Yagohf.Gympass.RaceAnalyser.Services.Domain
{
    public class RaceService : IRaceService
    {
        private readonly IRaceFileReader _raceFileReader;
        private readonly IRaceRepository _raceRepository;
        private readonly IRaceQuery _raceQuery;
        private readonly IRaceTypeRepository _raceTypeRepository;
        private readonly IRaceTypeQuery _raceTypeQuery;
        private readonly IUserRepository _userRepository;
        private readonly IUserQuery _userQuery;
        private readonly ILapRepository _lapRepository;
        private readonly IDriverResultRepository _driverResultRepository;
        private readonly IDriverResultQuery _driverResultQuery;
        private readonly IMapper _mapper;

        public RaceService(IRaceFileReader raceFileProcessor,
                           IRaceRepository raceRepository,
                           IRaceQuery raceQuery,
                           IRaceTypeRepository raceTypeRepository,
                           IRaceTypeQuery raceTypeQuery,
                           IUserRepository userRepository,
                           IUserQuery userQuery,
                           ILapRepository lapRepository,
                           IDriverResultRepository driverResultRepository,
                           IDriverResultQuery driverResultQuery,
                           IMapper mapper)
        {
            this._raceFileReader = raceFileProcessor;
            this._raceRepository = raceRepository;
            this._raceQuery = raceQuery;
            this._raceTypeRepository = raceTypeRepository;
            this._raceTypeQuery = raceTypeQuery;
            this._userRepository = userRepository;
            this._userQuery = userQuery;
            this._lapRepository = lapRepository;
            this._driverResultRepository = driverResultRepository;
            this._driverResultQuery = driverResultQuery;
            this._mapper = mapper;
        }

        public async Task<RaceResultDTO> AnalyseAsync(CreateRaceDTO createData, string uploader)
        {
            //Validar minimamente os dados de input.
            if (createData == null || uploader == null)
                throw new Exception("Dados inválidos para análise.");
            else if (!this.ValidateRequiredFields(createData, out string requiredFieldsErrorMessage))
                throw new BusinessException(requiredFieldsErrorMessage);

            //Processar o arquivo.
            await this._raceFileReader.Read(createData.ResultsFile);

            //Tratar retorno.
            if (this.ValidatePostProcessing(out string postProcessingErrorMessage))
            {
                int newRaceId = await this.ProcessAndSaveData(createData, uploader, this._raceFileReader.Results);
                return await this.GetResultByIdAsync(newRaceId);
            }
            else
            {
                throw new BusinessException(postProcessingErrorMessage);
            }
        }

        public async Task<RaceResultDTO> GetResultByIdAsync(int id)
        {
            RaceResultDTO result = new RaceResultDTO();
            var race = await this._raceRepository.GetSingleAsync(this._raceQuery.ById(id));
            var results = await this._driverResultRepository.ListAsync(this._driverResultQuery.ByRace(id));
            var winner = results.First();
            var bestLap = results.OrderBy(x => x.BestLap).Select(x => new BestLapDTO()
            {
                Driver = $"{x.DriverNumber} - {x.DriverName}",
                Time = x.BestLap
            }).First();

            result.RaceId = race.Id;
            result.RaceDescription = race.Description;
            result.TotalLaps = race.TotalLaps;
            result.RaceDate = race.Date;
            result.UploadDate = race.UploadDate;
            result.Uploader = race.Uploader.Login;
            result.Winner = $"{winner.DriverNumber} - {winner.DriverName}";
            result.BestLap = bestLap;
            result.Results = results.Map<DriverResult, DriverResultDTO>(this._mapper);
            return result;
        }

        public async Task<Listing<RaceSummaryDTO>> ListSummaryAsync(string description, int? page)
        {
            Listing<Race> listagem;
            if (page.HasValue)
            {
                listagem = await this._raceRepository.ListPagingAsync(this._raceQuery.ByDescription(description), page.Value, 10);
            }
            else
            {
                listagem = new Listing<Race>(await this._raceRepository.ListAsync(this._raceQuery.ByDescription(description)));
            }

            return listagem.Map<Race, RaceSummaryDTO>(this._mapper);
        }

        #region [ Helpers ]

        private bool ValidatePostProcessing(out string postProcessingErrorMessage)
        {
            postProcessingErrorMessage = string.Empty;

            if (this._raceFileReader.Success)
            {
                if (this._raceFileReader.Results == null || !this._raceFileReader.Results.Any())
                {
                    postProcessingErrorMessage = "Arquivo não possui dados para análise;";
                }
                else
                {
                    var duplicatedDriverNumber =
                                                from driver in
                                                (from l in this._raceFileReader.Results
                                                 group l by new { l.DriverNumber, l.DriverName } into driverGrouping
                                                 select driverGrouping)
                                                group driver by driver.Key.DriverNumber into driverNumberGrouping
                                                where driverNumberGrouping.Count() > 1
                                                select driverNumberGrouping;

                    if (duplicatedDriverNumber.Any())
                    {
                        postProcessingErrorMessage += "Existem pilotos diferentes com mesmo número: ";
                        postProcessingErrorMessage += string.Join(",", duplicatedDriverNumber.Select(duplDriverNumber => $"Número: {duplDriverNumber.Key} / Pilotos: { string.Join(",", duplDriverNumber.Select(x => x.Key.DriverName)) }"));
                        postProcessingErrorMessage += ";";
                    }
                    else
                    {
                        var duplicatedLapsToSameDriver = from l in this._raceFileReader.Results
                                                         group l by new { l.DriverNumber, l.Number } into driverLapGrouping
                                                         where driverLapGrouping.Count() > 1
                                                         select driverLapGrouping;

                        if (duplicatedLapsToSameDriver.Any())
                        {
                            postProcessingErrorMessage += "Existem voltas repetidas para o mesmo piloto: ";
                            postProcessingErrorMessage += string.Join(",", duplicatedLapsToSameDriver.Select(duplLap => $"Piloto: {duplLap.Key.DriverNumber} / Volta: { duplLap.Key.Number }"));
                            postProcessingErrorMessage += ";";
                        }
                    }
                }
            }
            else
            {
                postProcessingErrorMessage = this._raceFileReader.ErrorMessage;
            }

            return string.IsNullOrEmpty(postProcessingErrorMessage);
        }

        private async Task<int> ProcessAndSaveData(CreateRaceDTO createData, string uploader, IEnumerable<Lap> laps)
        {
            //Recuperar usuário da corrida.
            User user = await this._userRepository.GetSingleAsync(this._userQuery.ByLogin(uploader));

            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //Salvar corrida.
                Race race = new Race();
                race.Date = createData.Date;
                race.Description = createData.Description;
                race.TotalLaps = createData.TotalLaps;
                race.UploadDate = DateTime.Now;
                race.UploaderId = user.Id;
                race.RaceTypeId = createData.RaceTypeId;

                await this._raceRepository.InsertAsync(race);

                //Salvar cada uma das voltas após atualizar o ID da corrida.
                foreach (var lap in laps)
                {
                    lap.RaceId = race.Id;
                    await this._lapRepository.InsertAsync(lap);
                }

                //Processar o resultado dos pilotos e persistir resultados.
                IEnumerable<DriverResult> driverResults = this.ProcessDriverResults(laps);
                foreach (var driverResult in driverResults)
                {
                    driverResult.RaceId = race.Id;
                    await this._driverResultRepository.InsertAsync(driverResult);
                }

                //Commitar transação.
                ts.Complete();

                //Devolver ID para o chamador.
                return race.Id;
            }
        }

        private IEnumerable<DriverResult> ProcessDriverResults(IEnumerable<Lap> laps)
        {
            List<DriverResult> driverResults = new List<DriverResult>();

            var lapGrouping = from l in laps
                              group l by new { l.DriverNumber, l.DriverName } into lapGroup
                              select lapGroup;

            foreach (var lapGroup in lapGrouping)
            {
                DriverResult result = new DriverResult();
                result.AverageSpeed = lapGroup.Sum(l => l.AverageSpeed) / lapGroup.Count();
                result.BestLap = lapGroup.OrderBy(l => l.Time).First().Time;
                result.DriverName = lapGroup.Key.DriverName;
                result.DriverNumber = lapGroup.Key.DriverNumber;
                result.Laps = lapGroup.Count();
                result.TotalRaceTime = lapGroup.Aggregate(TimeSpan.Zero, (current, next) => current + next.Time);
                driverResults.Add(result);
            }

            driverResults = driverResults.OrderByDescending(dr => dr.Laps).ThenBy(dr => dr.TotalRaceTime).ToList();

            //Preencher posição e gap.
            for (int i = 0; i < driverResults.Count; i++)
            {
                driverResults[i].Position = i + 1;
                driverResults[i].Gap = i > 0 ? driverResults[i].TotalRaceTime - driverResults[i - 1].TotalRaceTime : (TimeSpan?)null;
            }

            return driverResults;
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
