using System;
using System.Collections.Generic;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class Race : EntityBase
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int RaceTypeId { get; set; }

        //Relacionamentos.
        public ICollection<Lap> Laps { get; set; }
        public RaceType RaceType { get; set; }
    }
}
