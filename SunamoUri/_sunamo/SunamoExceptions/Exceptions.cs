namespace SunamoUri._sunamo.SunamoExceptions;

/// <summary>
/// Provides exception message formatting and stack trace analysis utilities.
/// </summary>
internal sealed partial class Exceptions
{
    /// <summary>
    /// Checks if a collection has an odd number of elements and returns an error message.
    /// </summary>
    /// <param name="before">Context prefix for the error message.</param>
    /// <param name="listName">The name of the list being checked.</param>
    /// <param name="list">The collection to check.</param>
    /// <returns>An error message if the count is odd, otherwise null.</returns>
    internal static string? HasOddNumberOfElements(string before, string listName, ICollection list)
    {
        return list.Count % 2 == 1 ? CheckBefore(before) + listName + " has odd number of elements " + list.Count : null;
    }

    #region Other
    /// <summary>
    /// Formats a context prefix for error messages.
    /// </summary>
    /// <param name="before">The context string.</param>
    /// <returns>The formatted prefix string.</returns>
    internal static string CheckBefore(string before)
    {
        return string.IsNullOrWhiteSpace(before) ? string.Empty : before + ": ";
    }

    /// <summary>
    /// Extracts all exception messages including inner exceptions into a single string.
    /// </summary>
    /// <param name="exception">The exception to extract messages from.</param>
    /// <param name="isIncludingInner">Whether to include inner exception messages.</param>
    /// <returns>A string containing all exception messages.</returns>
    internal static string TextOfExceptions(Exception exception, bool isIncludingInner = true)
    {
        if (exception == null) return string.Empty;
        StringBuilder stringBuilder = new();
        stringBuilder.Append("Exception:");
        stringBuilder.AppendLine(exception.Message);
        if (isIncludingInner)
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                stringBuilder.AppendLine(exception.Message);
            }
        var result = stringBuilder.ToString();
        return result;
    }

    /// <summary>
    /// Gets the type, method name, and stack trace of the calling code.
    /// </summary>
    /// <param name="isFillAlsoFirstTwo">Whether to fill type and method name from the first non-ThrowEx frame.</param>
    /// <returns>A tuple containing type name, method name, and formatted stack trace.</returns>
    internal static Tuple<string, string, string> PlaceOfException(bool isFillAlsoFirstTwo = true)
    {
        StackTrace stackTrace = new();
        var stackTraceText = stackTrace.ToString();
        var lines = stackTraceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        lines.RemoveAt(0);
        var i = 0;
        string type = string.Empty;
        string methodName = string.Empty;
        for (; i < lines.Count; i++)
        {
            var line = lines[i];
            if (isFillAlsoFirstTwo)
                if (!line.StartsWith("   at ThrowEx"))
                {
                    TypeAndMethodName(line, out type, out methodName);
                    isFillAlsoFirstTwo = false;
                }
            if (line.StartsWith("at System."))
            {
                lines.Add(string.Empty);
                lines.Add(string.Empty);
                break;
            }
        }
        return new Tuple<string, string, string>(type, methodName, string.Join(Environment.NewLine, lines));
    }

    /// <summary>
    /// Extracts the type and method name from a stack trace line.
    /// </summary>
    /// <param name="line">A single stack trace line.</param>
    /// <param name="type">The extracted type name.</param>
    /// <param name="methodName">The extracted method name.</param>
    internal static void TypeAndMethodName(string line, out string type, out string methodName)
    {
        var afterAt = line.Split("at ")[1].Trim();
        var text = afterAt.Split("(")[0];
        var parts = text.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = parts[^1];
        parts.RemoveAt(parts.Count - 1);
        type = string.Join(".", parts);
    }

    /// <summary>
    /// Gets the name of the calling method at the specified stack depth.
    /// </summary>
    /// <param name="depth">The stack frame depth to retrieve.</param>
    /// <returns>The method name.</returns>
    internal static string CallingMethod(int depth = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(depth)?.GetMethod();
        if (methodBase == null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }
    #endregion

    #region OnlyReturnString
    /// <summary>
    /// Creates an argument out of range error message.
    /// </summary>
    /// <param name="before">Context prefix for the error message.</param>
    /// <param name="parameterName">The name of the parameter that is out of range.</param>
    /// <param name="message">Additional information about the error.</param>
    /// <returns>The formatted error message.</returns>
    internal static string? ArgumentOutOfRangeException(string before, string parameterName, string message)
    {
        return CheckBefore(before) + $"{parameterName} is out of range, another info: {message}";
    }

    /// <summary>
    /// Creates a custom error message.
    /// </summary>
    /// <param name="before">Context prefix for the error message.</param>
    /// <param name="message">The error message.</param>
    /// <returns>The formatted error message.</returns>
    internal static string? Custom(string before, string message)
    {
        return CheckBefore(before) + message;
    }
    #endregion
}
