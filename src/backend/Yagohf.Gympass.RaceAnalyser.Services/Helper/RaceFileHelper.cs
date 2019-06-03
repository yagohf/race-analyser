using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Configuration;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;
using Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper;

namespace Yagohf.Gympass.RaceAnalyser.Services.Helper
{
    public class RaceFileHelper : IRaceFileHelper
    {
        private List<Lap> _results;
        private int _currentLine;
        private readonly IOptions<RaceFileSettings> _options;

        public RaceFileHelper(IOptions<RaceFileSettings> options)
        {
            this._results = new List<Lap>();
            this._options = options;
        }

        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public IEnumerable<Lap> Results { get { return this._results; } }

        public async Task Process(Stream file)
        {
            this._results = new List<Lap>();
            this._currentLine = 1;
            file.Position = 0;
            bool success = true;

            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string lineContent = await sr.ReadLineAsync();
                    if (this._currentLine > 1)
                    {
                        Lap lap = this.ProcessLine(lineContent);

                        if (lap != null)
                            this._results.Add(lap);
                        else
                            success = false;
                    }

                    this._currentLine++;
                }
            }

            this.Success = success;
        }

        private Lap ProcessLine(string lineContent)
        {
            Lap lap = new Lap();
            StringBuilder sb = new StringBuilder();

            if (lineContent.Length < this._options.Value.LineMinLength)
            {
                sb.Append($"A linha {this._currentLine} não tem o tamanho correto;");
            }
            else
            {
                //Time.
                try
                {
                    lap.Date = DateTime.Parse(lineContent.Substring(this._options.Value.Date.Start, this._options.Value.Date.Length));
                }
                catch
                {
                    sb.Append($"Linha: {this._currentLine} / Campo: Hora - dado inválido;");
                }

                //Driver number.
                try
                {
                    lap.DriverNumber = int.Parse(lineContent.Substring(this._options.Value.DriverNumber.Start, this._options.Value.DriverNumber.Length).Trim());
                }
                catch
                {
                    sb.Append($"Linha: {this._currentLine} / Campo: Piloto (número) - dado inválido;");
                }

                //Driver name.
                try
                {
                    lap.DriverName = lineContent.Substring(this._options.Value.DriverName.Start, this._options.Value.DriverName.Length).Trim();
                }
                catch
                {
                    sb.Append($"Linha: {this._currentLine} / Campo: Piloto (nome) - dado inválido;");
                }

                //Lap number.
                try
                {
                    lap.Number = int.Parse(lineContent.Substring(this._options.Value.LapNumber.Start, this._options.Value.LapNumber.Length).Trim());
                }
                catch
                {
                    sb.Append($"Linha: {this._currentLine} / Campo: Nº Volta - dado inválido;");
                }

                //Lap time.
                try
                {
                    lap.Time = TimeSpan.ParseExact(lineContent.Substring(this._options.Value.LapTime.Start, this._options.Value.LapTime.Length), new string[] { @"m\:ss\.fff", @"m\:ss\.ff" }, CultureInfo.InvariantCulture);
                }
                catch
                {
                    sb.Append($"Linha: {this._currentLine} / Campo: Tempo Volta - dado inválido;");
                }

                //Lap average speed.
                try
                {
                    lap.AverageSpeed = decimal.Parse(lineContent.Substring(this._options.Value.LapAverageSpeed.Start, this._options.Value.LapAverageSpeed.Length).Trim());
                }
                catch
                {
                    sb.Append($"Linha: {this._currentLine} / Campo: Velocidade Média da Volta - dado inválido;");
                }               
            }

            if (string.IsNullOrEmpty(sb.ToString()))
            {
                return lap;
            }
            else
            {
                this.ErrorMessage += sb.ToString();
                return null;
            }
        }
    }
}
