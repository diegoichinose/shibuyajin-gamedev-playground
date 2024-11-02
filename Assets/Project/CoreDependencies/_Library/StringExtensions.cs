using System;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };

        
    public static string TryGetPluralVersion(this string input, float amount)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        if (input == "")
            throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));

        if (amount == 1)
            return input;

        var s = char.IsUpper(input[input.Length - 1]) ? "S" : "s";
        return input + s;
    }

    public static string TryGetPluralVersion(this string input, int amount) => input.TryGetPluralVersion((float) amount);

    static public string ReplaceWholeWord(this string original, string wordToFind, string replacement, RegexOptions regexOptions = RegexOptions.None)
    {
        string pattern = String.Format(@"\b{0}\b", wordToFind);
        string ret = Regex.Replace(original, pattern, replacement, regexOptions);
        return ret;
    }

    public static string FirstCharToLowerCase(this string str)
    {
        if ( !string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
            return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

        return str;
    }
}