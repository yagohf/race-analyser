using System.Collections.Generic;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        //Relacionamentos.
        public ICollection<Race> Races { get; set; }
    }
}
