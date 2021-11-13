using System;

namespace Utilities.Extensions;

public static class TypeConversionExtensions
{
    /// <summary>
    /// Convert yes, 1, true in string to boolean true
    /// </summary>
    public static bool ToBool(this string value) => 
        value.Trim().Equals("yes",           StringComparison.CurrentCultureIgnoreCase) ||
        value.Trim().Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase) || 
        value.Trim().Equals("1");
}