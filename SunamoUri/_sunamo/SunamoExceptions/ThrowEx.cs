namespace SunamoUri._sunamo.SunamoExceptions;

internal partial class ThrowEx
{
    internal static bool ThrowIsNotNull<TFirst, TSecond>(Func<string, TFirst, TSecond, string?> function, TFirst firstArgument, TSecond secondArgument)
    {
        string? exceptionMessage = function(FullNameOfExecutedCode(), firstArgument, secondArgument);
        return ThrowIsNotNull(exceptionMessage);
    }

    internal static bool HasOddNumberOfElements(string listName, ICollection list)
    {
        var function = Exceptions.HasOddNumberOfElements;
        return ThrowIsNotNull(function, listName, list);
    }

    internal static bool ArgumentOutOfRangeException(string argumentName, string message = "")
    { return ThrowIsNotNull(Exceptions.ArgumentOutOfRangeException(FullNameOfExecutedCode(), argumentName, message)); }

    internal static bool Custom(Exception exception, bool isReallyThrowing = true)
    { return Custom(Exceptions.TextOfExceptions(exception), isReallyThrowing); }

    internal static bool Custom(string message, bool isReallyThrowing = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? exceptionText = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(exceptionText, isReallyThrowing);
    }

    #region Other
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    static string FullNameOfExecutedCode(object type, string methodName, bool isFromThrowEx = false)
    {
        if (methodName is null)
        {
            int depth = 2;
            if (isFromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (type is Type typeValue)
        {
            typeFullName = typeValue.FullName ?? "Type cannot be get via type is Type type2";
        }
        else if (type is MethodBase method)
        {
            typeFullName = method.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = method.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type objectType = type.GetType();
            typeFullName = objectType.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    internal static bool ThrowIsNotNull(string? exceptionMessage, bool isReallyThrowing = true)
    {
        if (exceptionMessage is not null)
        {
            Debugger.Break();
            if (isReallyThrowing)
            {
                throw new Exception(exceptionMessage);
            }
            return true;
        }
        return false;
    }
    #endregion
}
