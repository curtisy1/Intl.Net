namespace Localization
{
    using System.Globalization;
    using Localizati18n.ResourceManager;

    public static class Localization
    {
        private static JsonResourceManager? s_ResourceManager;
        public static JsonResourceManager ResourceManager => s_ResourceManager ??= new JsonResourceManager("Localization.Localization", typeof(Localization).Assembly, "Localization");
        public static CultureInfo? ResourceCulture { get; set; }

        public static string Greeting => ResourceManager.GetString(nameof(Greeting), ResourceCulture)!;
        public static string Thanks => ResourceManager.GetString(nameof(Thanks), ResourceCulture)!;
        public static string Farewell => ResourceManager.GetString(nameof(Farewell), ResourceCulture)!;
        public static string SampleSentence => ResourceManager.GetString(nameof(SampleSentence), ResourceCulture)!;
        public static string MultiLineSentence => ResourceManager.GetString(nameof(MultiLineSentence), ResourceCulture)!;
    }
}