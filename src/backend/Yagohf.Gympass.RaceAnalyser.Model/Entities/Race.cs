using System;
using System.Collections.Generic;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class Race : EntityBase
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int RaceTypeId { get; set; }
        public int TotalLaps { get; set; }
        public int UploaderId { get; set; }
        public DateTime UploadDate { get; set; }

        //Relacionamentos.
        public ICollection<Lap> Laps { get; set; }
        public RaceType RaceType { get; set; }
        public User Uploader { get; set; }
        public ICollection<DriverResult> DriverResults { get; set; }
    }
}
