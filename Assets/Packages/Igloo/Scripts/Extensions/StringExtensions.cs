using System;
using System.Globalization;
using System.Text;

namespace Packages.Igloo.Scripts.Extensions
{
    public static class StringExtensions
    {
        /// <summary>Eg MY_INT_VALUE =&gt; MyIntValue</summary>
        public static string ToTitleCase(this string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < input.Length; ++index)
            {
                char ch = input[index];
                if (ch == '_' && index + 1 < input.Length)
                {
                    char upper = input[index + 1];
                    if (char.IsLower(upper))
                        upper = char.ToUpper(upper, CultureInfo.InvariantCulture);
                    stringBuilder.Append(upper);
                    ++index;
                }
                else
                    stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns whether or not the specified string is contained with this string
        /// </summary>
        public static bool Contains(
            this string source,
            string toCheck,
            StringComparison comparisonType)
        {
            return source.IndexOf(toCheck, comparisonType) >= 0;
        }

        /// <summary>Ex: "thisIsCamelCase" -&gt; "This Is Camel Case"</summary>
        public static string SplitPascalCase(this string input)
        {
            switch (input)
            {
                case "":
                case null:
                    return input;
                default:
                    StringBuilder stringBuilder = new StringBuilder(input.Length);
                    if (char.IsLetter(input[0]))
                        stringBuilder.Append(char.ToUpper(input[0]));
                    else
                        stringBuilder.Append(input[0]);
                    for (int index = 1; index < input.Length; ++index)
                    {
                        char c = input[index];
                        if (char.IsUpper(c) && !char.IsUpper(input[index - 1]))
                            stringBuilder.Append(' ');
                        stringBuilder.Append(c);
                    }

                    return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Returns true if this string is null, empty, or contains only whitespace.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns><c>true</c> if this string is null, empty, or contains only whitespace; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (int index = 0; index < str.Length; ++index)
                {
                    if (!char.IsWhiteSpace(str[index]))
                        return false;
                }
            }

            return true;
        }
    }
}