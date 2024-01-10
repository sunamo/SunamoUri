namespace SunamoUri._sunamo;

internal class SH
{
    internal static Func<string, string, string> PostfixIfNotEmpty;
    internal static Func<string, string> FirstCharUpper;
    internal static Func<string, string, bool, string> KeepAfterFirst;
    internal static Func<string, string> TextWithoutDiacritic;
    internal static Func<string, string, string, string> ReplaceOnce;
    internal static Func<string, char, string> RemoveAfterFirstChar;
    internal static Func<string, string, string> RemoveAfterFirst;
    internal static Func<string, String[], List<string>> SplitMore;
    internal static Func<string, string, List<string>> Split;
    internal static Func<string, string, string[], string> ReplaceAll;
    internal static Func<string, string, string> TrimEnd;
    internal static Func<string, char, bool, string> AddBeforeUpperChars;
    internal static Func<string, int, int, SubstringArgs, string> Substring;
    internal static Func<string, char, List<string>> SplitChar;
    internal static Func<string, string, bool, string> PrefixIfNotStartedWith;
    internal static Func<string, string, string> TrimStart;
    internal static Func<string, string> RemoveLastChar;
}
