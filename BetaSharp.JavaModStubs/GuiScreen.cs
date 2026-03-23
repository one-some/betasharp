// ub = Gui (base drawing helpers)
// da = GuiScreen extends Gui
// uq = GuiIngame extends Gui (HUD overlay, NOT a screen)
//
// Field/method name collisions (Java allows, C# doesn't):
//   da.b (Minecraft field) vs inherited b(...) methods → use NameSig on methods
//   id.a (xSize field) vs inherited a(...) methods → use NameSig on methods
//   da.c (width field) vs c() method → use NameSig on method

// ub = Gui (base class for all GUI rendering)
[IKVM.Attributes.NoPackagePrefix]
public class ub
{
    // Delegates set by BetaSharp.Client
    public static Action<int, int, int, int, int, int>? DrawGradientRectDelegate;
    public static Action<int, int, int, int, int, int>? DrawTexturedModalRectDelegate;
    public static Action? DrawDefaultBackgroundDelegate;

    // drawGradientRect — Java name: a(IIIIII)V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_drawGradientRect(int x1, int y1, int x2, int y2, int color1, int color2)
    {
        DrawGradientRectDelegate?.Invoke(x1, y1, x2, y2, color1, color2);
    }

    // drawTexturedModalRect — Java name: b(IIIIII)V
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual void func_drawTexturedModalRect(int x, int y, int u, int v, int width, int height)
    {
        DrawTexturedModalRectDelegate?.Invoke(x, y, u, v, width, height);
    }
}

// uq = GuiIngame (HUD overlay, extends Gui)
// Minecraft.v is this type — TMI calls v.a(String) to show chat messages
[IKVM.Attributes.NoPackagePrefix]
public class uq : ub
{
    public static Action<string>? AddChatMessageDelegate;

    // addChatMessage — Java name: a(Ljava/lang/String;)V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_addChatMessage(string message)
    {
        AddChatMessageDelegate?.Invoke(message);
    }
}

// da = GuiScreen (extends Gui, base for all menu screens)
[IKVM.Attributes.NoPackagePrefix]
public class da : ub
{
    public net.minecraft.client.Minecraft b; // mc instance
    public int c; // width
    public int d; // height
    public sj g; // fontRenderer

    // initGui — Java name: b()V (collides with field b)
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual void func_initGui() { }

    // drawDefaultBackground — Java name: i()V
    public virtual void i()
    {
        DrawDefaultBackgroundDelegate?.Invoke();
    }

    // render — Java name: a(IIF)V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_render(int mouseX, int mouseY, float partialTicks) { }

    // mouseClicked — Java name: a(III)V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_mouseClicked(int mouseX, int mouseY, int button) { }

    // keyTyped — Java name: a(CI)V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_keyTyped(char c, int key) { }

    // updateScreen — Java name: a()V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_updateScreen() { }

    // doesGuiPauseGame — Java name: c()Z (collides with field c)
    [IKVM.Attributes.NameSigAttribute("c", "")]
    public virtual bool func_doesGuiPauseGame() => true;

    // onGuiClosed — Java name: h()V
    public virtual void h() { }

    // Static helper called from generated ue.a(float) bytecode to draw inventory background
    public static Action? DrawInventoryBackgroundDelegate;
    public static void drawInventoryBg() { DrawInventoryBackgroundDelegate?.Invoke(); }

    // mouseMovedOrUp — Java name: b(III)V (collides with field b)
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual void func_mouseMovedOrUp(int x, int y, int button) { }
}
