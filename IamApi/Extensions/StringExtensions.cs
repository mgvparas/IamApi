﻿namespace IamApi.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string targetString) => string.IsNullOrWhiteSpace(targetString);
    }
}
