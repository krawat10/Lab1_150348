namespace Lab1_150348
{
    public static class StringExtensions
    {
        public static string Repeat(this string value, int repeat)
        {
            var result = string.Empty;

            for (int i = 0; i < repeat; i++)
            {
                result += value;
            }

            return result;
        }
    }
}