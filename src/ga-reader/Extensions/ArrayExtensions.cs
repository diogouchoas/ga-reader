namespace ga_reader.Extensions
{
    public static class ArrayExtensions
    {
        public static string GetByIndex(this string[] that, int index)
        {
            return index > that.Length - 1 ? null : that[index];
        }
    }
}