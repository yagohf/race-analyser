using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Configuration;
using Yagohf.Gympass.RaceAnalyser.Services.Helper;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Services.Helper
{
    [TestClass]
    public class RaceFileReaderTests
    {
        private RaceFileReader _raceFileReader;
        private readonly Mock<IOptions<RaceFileSettings>> _raceFileSettingsOptionsMock;

        public RaceFileReaderTests()
        {
            //Configurar RaceFileSettings.
            this._raceFileSettingsOptionsMock = new Mock<IOptions<RaceFileSettings>>();
            RaceFileSettings raceFileSettings = new RaceFileSettings();
            TestUtil.GetConfiguration().GetSection("RaceFile").Bind(raceFileSettings);
            this._raceFileSettingsOptionsMock.Setup(x => x.Value).Returns(raceFileSettings);
        }

        [TestInitialize]
        public void Initialize()
        {
            this._raceFileReader = new RaceFileReader(this._raceFileSettingsOptionsMock.Object);
        }

        [TestMethod]
        public async Task Test_Read_NoFile()
        {
            //Arrange.


            //Act.
            await this._raceFileReader.Read(null);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual("Arquivo inválido para processamento", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(0, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_EmptyStream()
        {
            //Arrange.


            //Act.
            await this._raceFileReader.Read(new MemoryStream());

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual("Arquivo inválido para processamento", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(0, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_EmptyFile()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("EMPTY.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual("Arquivo inválido para processamento", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(0, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileOnlyWithHeaders()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("ONLY-HEADER.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsTrue(this._raceFileReader.Success);
            Assert.IsNull(this._raceFileReader.ErrorMessage);
            Assert.AreEqual(0, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithEmptyRow()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-EMPTY.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"A linha 2 não tem o tamanho correto;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithInvalidRowSize()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-ROWLENGTH.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"A linha 2 não tem o tamanho correto;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithInvalidDataOnTime()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-TIME.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"Linha: 2 / Campo: Hora - dado inválido;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithInvalidDataOnDriverNumber()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-DRIVERNUMBER.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"Linha: 2 / Campo: Piloto (número) - dado inválido;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithInvalidDataOnDriverName()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-DRIVERNAME.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"Linha: 2 / Campo: Piloto (nome) - dado inválido;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithInvalidDataOnLapNumber()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-LAPNUMBER.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"Linha: 2 / Campo: Nº Volta - dado inválido;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithInvalidDataOnLapTime()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-LAPTIME.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"Linha: 2 / Campo: Tempo Volta - dado inválido;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        [TestMethod]
        public async Task Test_Read_FileWithInvalidDataOnAverageSpeed()
        {
            //Arrange.
            var file = await this.ReadFileFromAssembly("INVALID-ROW-AVGSPD.txt");

            //Act.
            await this._raceFileReader.Read(file);

            //Assert.
            Assert.IsFalse(this._raceFileReader.Success);
            Assert.AreEqual($"Linha: 2 / Campo: Velocidade Média da Volta - dado inválido;", this._raceFileReader.ErrorMessage);
            Assert.AreEqual(22, this._raceFileReader.Results.Count());
        }

        #region [ Helpers ]

        private async Task<MemoryStream> ReadFileFromAssembly(string fileName)
        {
            string fileAssemblyPath = $"Yagohf.Gympass.RaceAnalyser.UnitTests.Embedded.{fileName}";
            using (var fileAssemblyStream = Assembly.GetAssembly(typeof(RaceFileReaderTests)).GetManifestResourceStream(fileAssemblyPath))
            {
                MemoryStream ms = new MemoryStream();
                fileAssemblyStream.Position = 0;
                await fileAssemblyStream.CopyToAsync(ms);
                return ms;
            }
        }

        #endregion
    }
}
