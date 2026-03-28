namespace SunamoUri._sunamo.SunamoExceptions;

/// <summary>
/// Provides methods that throw exceptions based on conditions.
/// </summary>
internal partial class ThrowEx
{
    /// <summary>
    /// Throws an exception if the function returns a non-null error message.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first argument.</typeparam>
    /// <typeparam name="TSecond">The type of the second argument.</typeparam>
    /// <param name="function">The validation function.</param>
    /// <param name="firstArgument">The first argument to validate.</param>
    /// <param name="secondArgument">The second argument to validate.</param>
    /// <returns>True if an exception was found.</returns>
    internal static bool ThrowIsNotNull<TFirst, TSecond>(Func<string, TFirst, TSecond, string?> function, TFirst firstArgument, TSecond secondArgument)
    {
        string? exceptionMessage = function(FullNameOfExecutedCode(), firstArgument, secondArgument);
        return ThrowIsNotNull(exceptionMessage);
    }

    /// <summary>
    /// Checks if a collection has an odd number of elements and throws if so.
    /// </summary>
    /// <param name="listName">The name of the list for the error message.</param>
    /// <param name="list">The collection to check.</param>
    /// <returns>True if the collection has an odd number of elements.</returns>
    internal static bool HasOddNumberOfElements(string listName, ICollection list)
    {
        var function = Exceptions.HasOddNumberOfElements;
        return ThrowIsNotNull(function, listName, list);
    }

    /// <summary>
    /// Throws an ArgumentOutOfRangeException with the specified message.
    /// </summary>
    /// <param name="argumentName">The name of the argument that is out of range.</param>
    /// <param name="message">Additional details about the error.</param>
    /// <returns>True if an exception was thrown.</returns>
    internal static bool ArgumentOutOfRangeException(string argumentName, string message = "")
    { return ThrowIsNotNull(Exceptions.ArgumentOutOfRangeException(FullNameOfExecutedCode(), argumentName, message)); }

    /// <summary>
    /// Throws a custom exception based on the provided exception.
    /// </summary>
    /// <param name="exception">The exception to throw.</param>
    /// <param name="isReallyThrowing">Whether to actually throw or just return true.</param>
    /// <returns>True if an exception condition was detected.</returns>
    internal static bool Custom(Exception exception, bool isReallyThrowing = true)
    { return Custom(Exceptions.TextOfExceptions(exception), isReallyThrowing); }

    /// <summary>
    /// Throws a custom exception with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="isReallyThrowing">Whether to actually throw or just return true.</param>
    /// <param name="secondMessage">An additional message to append.</param>
    /// <returns>True if an exception condition was detected.</returns>
    internal static bool Custom(string message, bool isReallyThrowing = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? exceptionText = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(exceptionText, isReallyThrowing);
    }

    #region Other
    /// <summary>
    /// Gets the full name of the currently executed code location.
    /// </summary>
    /// <returns>The fully qualified name of the calling code.</returns>
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    /// <summary>
    /// Gets the full name of executed code from a type and method name.
    /// </summary>
    /// <param name="type">The type containing the method.</param>
    /// <param name="methodName">The method name.</param>
    /// <param name="isFromThrowEx">Whether the call originates from ThrowEx.</param>
    /// <returns>The fully qualified name.</returns>
    static string FullNameOfExecutedCode(object type, string methodName, bool isFromThrowEx = false)
    {
        if (methodName == null)
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

    /// <summary>
    /// Throws an exception if the provided message is not null.
    /// </summary>
    /// <param name="exceptionMessage">The exception message to check.</param>
    /// <param name="isReallyThrowing">Whether to actually throw or just return true.</param>
    /// <returns>True if the message was not null.</returns>
    internal static bool ThrowIsNotNull(string? exceptionMessage, bool isReallyThrowing = true)
    {
        if (exceptionMessage != null)
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
