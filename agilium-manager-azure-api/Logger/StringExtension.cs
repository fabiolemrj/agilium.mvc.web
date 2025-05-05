using System.Collections.Generic;
using System;
using System.Linq;

namespace agilium.api.manager.Logger
{
    public static class StringExtension
    {
        public static bool ContainsAny(this string input, IEnumerable<string> containsKeywords, StringComparison comparisonType)
        {
            return containsKeywords.Any(keyword => input.IndexOf(keyword, comparisonType) >= 0);
        }
    }
}
