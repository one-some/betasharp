// Stub for org.lwjgl.opengl.GL11 — forwards to delegates set by BetaSharp.Client
namespace org.lwjgl.opengl;

[IKVM.Attributes.NoPackagePrefix]
public class GL11
{
    // Delegates set by BetaSharp.Client at startup
    public static Action<int>? EnableDelegate;
    public static Action<int>? DisableDelegate;
    public static Action? PushMatrixDelegate;
    public static Action? PopMatrixDelegate;
    public static Action<float, float, float>? TranslatefDelegate;
    public static Action<float, float, float, float>? RotatefDelegate;
    public static Action<float, float, float>? ScalefDelegate;
    public static Action<float, float, float, float>? Color4fDelegate;
    public static Action<int, int>? BlendFuncDelegate;

    // GL constants TMI uses
    public const int GL_LIGHTING = 2896;
    public const int GL_DEPTH_TEST = 2929;
    public const int GL_RESCALE_NORMAL = 32826;
    public const int GL_BLEND = 3042;
    public const int GL_TEXTURE_2D = 3553;
    public const int GL_SRC_ALPHA = 770;
    public const int GL_ONE_MINUS_SRC_ALPHA = 771;

    public static void glEnable(int cap) => EnableDelegate?.Invoke(cap);
    public static void glDisable(int cap) => DisableDelegate?.Invoke(cap);
    public static void glPushMatrix() => PushMatrixDelegate?.Invoke();
    public static void glPopMatrix() => PopMatrixDelegate?.Invoke();
    public static void glTranslatef(float x, float y, float z) => TranslatefDelegate?.Invoke(x, y, z);
    public static void glRotatef(float angle, float x, float y, float z) => RotatefDelegate?.Invoke(angle, x, y, z);
    public static void glScalef(float x, float y, float z) => ScalefDelegate?.Invoke(x, y, z);
    public static void glColor4f(float r, float g, float b, float a) => Color4fDelegate?.Invoke(r, g, b, a);
    public static void glBlendFunc(int sfactor, int dfactor) => BlendFuncDelegate?.Invoke(sfactor, dfactor);
}
