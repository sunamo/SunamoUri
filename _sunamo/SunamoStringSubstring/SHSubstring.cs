namespace SunamoUri._sunamo.SunamoStringSubstring;

internal class SHSubstring
{
    internal static string Substring(string sql, int indexFrom, int indexTo, SubstringArgs a = null)
    {
        if (a == null) a = SubstringArgs.Instance;

        if (sql == null) return null;

        var tl = sql.Length;

        if (indexFrom > indexTo)
        {
            if (a.returnInputIfIndexFromIsLessThanIndexTo)
                return sql;
            ThrowEx.ArgumentOutOfRangeException("indexFrom", "indexFrom is lower than indexTo");
        }

        if (tl > indexFrom)
        {
            if (tl > indexTo)
            {
                return sql.Substring(indexFrom, indexTo - indexFrom);
            }

            if (a.returnInputIfInputIsShorterThanA3) return sql;
        }

        // must return string.Empty, not null, because null cant be save to many of columns in db
        return string.Empty;
    }
}