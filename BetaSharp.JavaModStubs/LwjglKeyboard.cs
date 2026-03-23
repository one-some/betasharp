// Stub for org.lwjgl.input.Keyboard — forwards to a delegate set by BetaSharp.Client
namespace org.lwjgl.input;

[IKVM.Attributes.NoPackagePrefix]
public class Keyboard
{
    // Set by BetaSharp.Client at startup to forward to the real Keyboard.isKeyDown
    public static Func<int, bool>? IsKeyDownDelegate;

    public static bool isKeyDown(int key) => IsKeyDownDelegate?.Invoke(key) ?? false;
}
