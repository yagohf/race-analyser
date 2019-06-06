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
    public class RaceFileReader : IRaceFileReader
    {
        private List<Lap> _results;
        private int _currentLine;
        private readonly RaceFileSettings _settings;

        public RaceFileReader(IOptions<RaceFileSettings> options)
        {
            this._results = new List<Lap>();
            this._settings = options.Value;
        }

        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public IEnumerable<Lap> Results { get { return this._results; } }

        public async Task Read(Stream file)
        {
            if(file == null || file.Length == 0)
            {
                this.Success = false;
                this.ErrorMessage = "Arquivo inválido para processamento";
                return;
            }

            this._results = new List<Lap>();
            this._currentLine = 1;
            file.Position = 0; //Rebobinar o stream.
            bool success = true;

            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string lineContent = await sr.ReadLineAsync();
                    if (this._currentLine > 1)
                    {
                        Lap lap = this.ReadLine(lineContent);

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

        private Lap ReadLine(string lineContent)
        {
            Lap lap = new Lap();
            StringBuilder sb = new StringBuilder();

            if (lineContent.Length < this._settings.LineMinLength || lineContent.Length > this._settings.LineMaxLength)
            {
                sb.Append($"A linha {this._currentLine} não tem o tamanho correto;");
            }
            else
            {
                //Time.
                ExtractTime(lineContent, lap, sb);

                //Driver number.
                ExtractDriverNumber(lineContent, lap, sb);

                //Driver name.
                ExtractDriverName(lineContent, lap, sb);

                //Lap number.
                ExtractLapNumber(lineContent, lap, sb);

                //Lap time.
                ExtractLapTime(lineContent, lap, sb);

                //Lap average speed.
                ExtractLapAverageSpeed(lineContent, lap, sb);
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

        private void ExtractTime(string lineContent, Lap lap, StringBuilder sb)
        {
            if (DateTime.TryParse(lineContent.Substring(this._settings.Date.Start, this._settings.Date.Length), out DateTime time))
                lap.Date = time;
            else
                sb.Append($"Linha: {this._currentLine} / Campo: Hora - dado inválido;");
        }

        private void ExtractDriverNumber(string lineContent, Lap lap, StringBuilder sb)
        {
            if (int.TryParse(lineContent.Substring(this._settings.DriverNumber.Start, this._settings.DriverNumber.Length).Trim(), out int driverNumber))
                lap.DriverNumber = driverNumber;
            else
                sb.Append($"Linha: {this._currentLine} / Campo: Piloto (número) - dado inválido;");
        }

        private void ExtractDriverName(string lineContent, Lap lap, StringBuilder sb)
        {
            string driverName = lineContent.Substring(this._settings.DriverName.Start, this._settings.DriverName.Length).Trim();
            if (!string.IsNullOrEmpty(driverName))
                lap.DriverName = driverName;
            else
                sb.Append($"Linha: {this._currentLine} / Campo: Piloto (nome) - dado inválido;");
        }
        private void ExtractLapNumber(string lineContent, Lap lap, StringBuilder sb)
        {
            if (int.TryParse(lineContent.Substring(this._settings.LapNumber.Start, this._settings.LapNumber.Length).Trim(), out int lapNumber))
                lap.Number = lapNumber;
            else
                sb.Append($"Linha: {this._currentLine} / Campo: Nº Volta - dado inválido;");
        }

        private void ExtractLapTime(string lineContent, Lap lap, StringBuilder sb)
        {
            if (TimeSpan.TryParseExact(lineContent.Substring(this._settings.LapTime.Start, this._settings.LapTime.Length), new string[] { @"m\:ss\.fff", @"m\:ss\.ff" }, CultureInfo.InvariantCulture, out TimeSpan time))
                lap.Time = time;
            else
                sb.Append($"Linha: {this._currentLine} / Campo: Tempo Volta - dado inválido;");
        }

        private void ExtractLapAverageSpeed(string lineContent, Lap lap, StringBuilder sb)
        {
            try
            {
                string avgSpeed = lineContent.Substring(this._settings.LapAverageSpeed.Start).Trim();
                if (avgSpeed.Length < this._settings.LapAverageSpeed.Length)
                    avgSpeed.PadRight(this._settings.LapAverageSpeed.Length, '0');

                lap.AverageSpeed = decimal.Parse(avgSpeed);
            }
            catch
            {
                sb.Append($"Linha: {this._currentLine} / Campo: Velocidade Média da Volta - dado inválido;");
            }
        }
    }
}
