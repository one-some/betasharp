extern alias JavaModStubs;

using BetaSharp.Client.Guis;

namespace BetaSharp.Client.Modding.Java;

/// <summary>
/// C# GuiScreen that wraps a Java da (GuiScreen) instance loaded via IKVM.
/// Forwards all screen lifecycle calls to the Java screen.
/// </summary>
public class JavaScreenAdapter : GuiScreen
{
    private readonly JavaModStubs::da _javaScreen;
    private readonly JavaModStubs::net.minecraft.client.Minecraft _mcStub;
    internal static int FrameCount;

    public override bool PausesGame => false; // Inventory screens don't pause

    public JavaScreenAdapter(JavaModStubs::da javaScreen, JavaModStubs::net.minecraft.client.Minecraft mcStub)
    {
        _javaScreen = javaScreen;
        _mcStub = mcStub;
        AllowUserInput = true;
    }

    public override void InitGui()
    {
        base.InitGui();

        // Sync C# screen dimensions → Java stub fields
        _javaScreen.b = _mcStub;
        _javaScreen.c = Width;
        _javaScreen.d = Height;
        _javaScreen.g ??= new JavaModStubs::sj();

        // Call Java initGui
        _javaScreen.func_initGui();
    }

    public override void Render(int mouseX, int mouseY, float partialTicks)
    {
        FrameCount++;
        try
        {
            _javaScreen.func_render(mouseX, mouseY, partialTicks);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[JavaScreenAdapter] Render exception: {ex}");
            Console.Error.Flush();
        }
    }

    protected override void MouseClicked(int mouseX, int mouseY, int button)
    {
        _javaScreen.func_mouseClicked(mouseX, mouseY, button);
    }

    protected override void KeyTyped(char eventChar, int eventKey)
    {
        _javaScreen.func_keyTyped(eventChar, eventKey);
    }

    protected override void MouseMovedOrUp(int x, int y, int button)
    {
        _javaScreen.func_mouseMovedOrUp(x, y, button);
    }

    public override void UpdateScreen()
    {
        _javaScreen.func_updateScreen();
    }

    public override void OnGuiClosed()
    {
        _javaScreen.h();
        base.OnGuiClosed();
    }
}
