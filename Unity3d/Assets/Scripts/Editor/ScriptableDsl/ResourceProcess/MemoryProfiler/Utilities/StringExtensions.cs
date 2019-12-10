using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.MemoryProfilerForExtension.Editor.Extensions.String
{
    public static class StringExtensions
    {
        /// https://www.researchgate.net/publication/224960000_A_Fast_String_Searching_Algorithm
        /// <summary>
        ///IndexOf extension using the Boyer-Moore string search algorithm,
        ///returns the index to the start of the first occurrence of the provided pattern.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int IndexOf(this string src, int startIndex, string pattern)
        {
            if (pattern == null)
            {
                throw new System.ArgumentNullException("pattern can not be null");
            }
            if (startIndex < 0 || startIndex > src.Length - 1)
            {
                throw new System.IndexOutOfRangeException("startIndex is out of range");
            }
            if (src.Length - startIndex < pattern.Length)
            {
                throw new System.ArgumentException("pattern length can not exceed the length of the string being queried.\nPattern length: " + pattern.Length + ", queried string length: " + src.Length);
            }
            if (pattern.Length == 0 || src.Length == 0)
            {
                return -1;
            }

            unsafe
            {
                fixed(char* patternPtr = pattern)
                {
                    fixed(char* srcPtr = src)
                    {
                        int patternLengthOffset = pattern.Length - 1;
                        Int16* srcIter = (Int16*)srcPtr + startIndex + patternLengthOffset;
                        Int16* srcEnd = (Int16*)srcPtr + src.Length;
                        Int16* lastCharInPattern = (Int16*)patternPtr + patternLengthOffset;

                        if (patternLengthOffset == 0)
                        {
                            while (srcIter < srcEnd)
                            {
                                if (*patternPtr == *srcIter)
                                {
                                    return (int)(srcIter - (Int16*)srcPtr);
                                }
                                ++srcIter;
                            }
                        }
                        else
                        {
                            while (srcIter < srcEnd)
                            {
                                Int16* patternRevStart = lastCharInPattern;
                                Int16* patternRevEnd = (Int16*)patternPtr - 1;

                                while (*patternRevStart == *srcIter)
                                {
                                    --patternRevStart;
                                    --srcIter;

                                    if (patternRevStart == patternRevEnd)
                                    {
                                        return (int)((srcIter + 1) - (Int16*)srcPtr);
                                    }
                                }

                                while (patternRevStart != patternRevEnd)
                                {
                                    if (*patternRevStart == *srcIter)
                                    {
                                        break;
                                    }
                                    --patternRevStart;
                                }
                                srcIter += lastCharInPattern - patternRevStart;
                            }
                        }
                        return -1; //Did not find any occurrence of the pattern
                    }
                }
            }
        }
    }
}
