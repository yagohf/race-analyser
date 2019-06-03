using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Services.Interface.Helper
{
    public interface IRaceFileHelper
    {
        bool Success { get; }
        string ErrorMessage { get; }
        IEnumerable<Lap> Results { get; }
        Task Process(Stream file);
    }
}
