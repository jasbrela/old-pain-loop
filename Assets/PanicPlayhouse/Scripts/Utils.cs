namespace PanicPlayhouse.Scripts
{
    public static class Utils
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Color(this string str, string color) => $"<color={color}>{str}</color>";
    }
}
