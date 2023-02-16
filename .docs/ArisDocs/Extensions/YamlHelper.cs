/* -----------------------------------------------------------------------------
Copyright 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */

namespace ArisDocs;

internal static class YamlHelper
{
    private const char INVALID_CHAR_REPLACEMENT = '_';
    private const char REPLACEMENT_CHAR_REPLACEMENT = '-';

    private static char[] s_invalidChars = new[] { '?', '{', '}', '"', '<', '>', '*' };
    private static char[] s_replaceChars = new[] { ':', '`', '.' };

    internal static string SanitizeString(string value)
    {
        for (int i = 0; i < s_invalidChars.Length; i++)
        {
            value = value.Replace(s_invalidChars[i], INVALID_CHAR_REPLACEMENT);
        }

        for (int i = 0; i < s_replaceChars.Length; i++)
        {
            value = value.Replace(s_replaceChars[i], REPLACEMENT_CHAR_REPLACEMENT);
        }

        return value;
    }
}