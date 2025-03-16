// -- TimezoneDetector/Models/TimezoneInfoDTO.cs --
namespace WorldClock
{
    /// <summary>
    /// Data Transfer Object for timezone information
    /// </summary>
    public class TimezoneInfoDTO
    {
        public string DisplayName { get; set; } = string.Empty;
        public string CurrentTime { get; set; } = string.Empty;
        public string OffsetFromLocal { get; set; } = string.Empty;
    }
}