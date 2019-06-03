using System.Collections.Generic;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class RaceType : DomainEntityBase
    {
        //Relacionamentos.
        public ICollection<Race> Races { get; set; }
    }
}
