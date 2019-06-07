namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Configuration
{
    public class RaceFileSettings
    {
        public string AllowedContentType { get; set; }
        public int LineMinLength { get; set; }
        public int LineMaxLength { get; set; }
        public RaceFileSettingsItem Date { get; set; }
        public RaceFileSettingsItem DriverNumber { get; set; }
        public RaceFileSettingsItem DriverName { get; set; }
        public RaceFileSettingsItem LapNumber { get; set; }
        public RaceFileSettingsItem LapTime { get; set; }
        public RaceFileSettingsItem LapAverageSpeed { get; set; }        
    }

    public class RaceFileSettingsItem
    {
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
