using System.IO;

namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Model
{
    public class FileDTO
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public Stream Content { get; set; }
    }
}
