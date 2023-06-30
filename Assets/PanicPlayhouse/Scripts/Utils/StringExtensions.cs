namespace PanicPlayhouse.Scripts
{
    public static class StringExtensions
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Color(this string str, string color) => $"<color={color}>{str}</color>";
    }
}
