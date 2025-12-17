using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public static class HelperExtensions
{
    // Core logic
    private static string FormatShortNumber(double number, int decimalPlaces)
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" };
        int suffixIndex = 0;
        double absNumber = Math.Abs(number);

        while (absNumber >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            absNumber /= 1000;
            suffixIndex++;
        }

        string format = "0." + new string('#', decimalPlaces);
        string shortNumber = absNumber.ToString(format, CultureInfo.InvariantCulture);
        return (number < 0 ? "-" : "") + shortNumber + suffixes[suffixIndex];
    }

    // For double
    public static string ToShortNumberString(this double number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);

    // For float
    public static string ToShortNumberString(this float number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);

    // For int
    public static string ToShortNumberString(this int number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);

    // For long
    public static string ToShortNumberString(this long number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);

    // For decimal
    public static string ToShortNumberString(this decimal number, int decimalPlaces = 2)
        => FormatShortNumber((double)number, decimalPlaces);

    // For string
    public static string ToShortNumberString(this string input, int decimalPlaces = 2)
    {
        if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
        {
            return FormatShortNumber(number, decimalPlaces);
        }

        return input;
    }
    public static string ToTimeFormat(this float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.TotalHours >= 1 
            ? time.ToString(@"hh\:mm\:ss") 
            : time.ToString(@"mm\:ss");
    }

    public static string ToTimeFormat(this int seconds)
    {
        return ((float)seconds).ToTimeFormat();
    }

    public static void SetValues(this TextMeshPro textMeshPro, string value, Color color=default)
    {
        textMeshPro.text = value;
        textMeshPro.color = color==default? textMeshPro.color:color;
    }
}