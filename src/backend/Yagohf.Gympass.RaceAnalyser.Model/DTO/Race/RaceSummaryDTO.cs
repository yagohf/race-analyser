﻿using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.DTO.Race
{
    public class RaceSummaryDTO
    {
        public int RaceId { get; set; }
        public string RaceName { get; set; }
        public DateTime RaceDate { get; set; }
        public string Winner { get; set; }
    }
}