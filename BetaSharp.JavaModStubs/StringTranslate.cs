// nh = StringTranslate
[IKVM.Attributes.NoPackagePrefix]
public class nh
{
    private static nh _instance = new();

    // Delegate set by BetaSharp.Client for translation
    public static Func<string, string>? TranslateDelegate;

    // a() -> getInstance
    public static nh a() => _instance;
    // b(String) -> translateNamedKey
    public virtual string b(string key)
    {
        if (TranslateDelegate != null)
            return TranslateDelegate(key);
        return key;
    }
}
