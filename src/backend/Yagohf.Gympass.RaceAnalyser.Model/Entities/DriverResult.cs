using System;

namespace Yagohf.Gympass.RaceAnalyser.Model.Entities
{
    public class DriverResult : EntityBase
    {
        public int RaceId { get; set; }
        public int Position { get; set; }
        public int DriverNumber { get; set; }
        public string DriverName { get; set; }
        public int Laps { get; set; }
        public long TotalRaceTimeTicks { get; set; }
        public TimeSpan TotalRaceTime
        {
            get
            {
                return new TimeSpan(TotalRaceTimeTicks);
            }
            set
            {
                TotalRaceTimeTicks = value.Ticks;
            }
        }
        public long BestLapTicks { get; set; }
        public TimeSpan BestLap
        {
            get
            {
                return new TimeSpan(BestLapTicks);
            }
            set
            {
                BestLapTicks = value.Ticks;
            }
        }
        public decimal AverageSpeed { get; set; }
        public long? GapTicks { get; set; }
        public TimeSpan? Gap
        {
            get
            {
                if (!this.GapTicks.HasValue)
                    return null;

                return new TimeSpan(GapTicks.Value);
            }
            set
            {
                if (value.HasValue)
                    GapTicks = value.Value.Ticks;
                else
                    GapTicks = null;
            }
        }

        //Relacionamentos.
        public Race Race { get; set; }
    }
}
