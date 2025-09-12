using System.Text.RegularExpressions;
using DocTask.Core.Exceptions;

namespace DocTask.Service.Helpers;

public class TimeConverter
{
    public static long ConvertToMilliseconds(string timeString)
    {
        if (string.IsNullOrWhiteSpace(timeString))
            throw new ArgumentException("Time string cannot be null or empty.");
        
        timeString = timeString.Trim().ToLower();
        
        var match = Regex.Match(timeString, @"^(\d*\.?\d+)(s|min|h|d|w|m|y)$");
        if (!match.Success)
            throw new BadRequestException("Invalid time format. Expected format: number followed by s, min, h, d, w, m, or y (e.g., '1s', '2.5min', '6m', '7y').");
        
        if (!double.TryParse(match.Groups[1].Value, out double value))
            throw new BadRequestException("Invalid number format in time string.");

        var unit = match.Groups[2].Value;
        
        return unit switch
        {
            "s" => (long)(value * 1000),                    // Giây -> mili giây
            "min" => (long)(value * 60 * 1000),            // Phút -> mili giây
            "h" => (long)(value * 60 * 60 * 1000),         // Giờ -> mili giây
            "d" => (long)(value * 24 * 60 * 60 * 1000),    // Ngày -> mili giây
            "w" => (long)(value * 7 * 24 * 60 * 60 * 1000), // Tuần -> mili giây
            "m" => (long)(value * 30 * 24 * 60 * 60 * 1000), // Tháng (30 ngày) -> mili giây
            "y" => (long)(value * 365 * 24 * 60 * 60 * 1000), // Năm (365 ngày) -> mili giây
            _ => throw new ArgumentException("Unsupported time unit.")
        };
    }
}