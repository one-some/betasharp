using BetaSharp;

namespace net.minecraft.client;

[IKVM.Attributes.NoPackagePrefix]
public class Minecraft
{
    private IGame game;

    // World
    public fd f;
    public fd? theWorld => game?.world != null ? f : null;

    public static Minecraft a;
    public dc h;
    public uq v; // currentScreen (sig must be Luq; to match original bytecode)
    public ji p; // renderEngine
    public ob c; // playerController
    public kv z; // gameSettings

    public void UpdateFields()
    {
        var player = game?.player;
        if (player != null)
        {
            h ??= new dc();
            h.c ??= new ix();
            h.d ??= new dw(); // inventorySlots
            h.e ??= new dw(); // craftingInventory
            h.l = player.name ?? "";
            h.c.c = player.inventory?.selectedSlot ?? 0;

            var main = player.inventory?.main;
            if (main != null)
            {
                if (h.c.a == null || h.c.a.Length != main.Length)
                    h.c.a = new iz[main.Length];

                for (int i = 0; i < main.Length; i++)
                {
                    if (main[i] != null)
                    {
                        h.c.a[i] ??= new iz();
                        h.c.a[i].a = main[i].count;    // a = stackSize
                        h.c.a[i].c = main[i].itemId;   // c = itemID
                        h.c.a[i].d = main[i].getDamage(); // d = itemDamage
                    }
                    else
                    {
                        h.c.a[i] = null;
                    }
                }
            }

            // Sync cursor stack (item being dragged)
            var cursor = player.inventory?.getCursorStack();
            if (cursor != null)
            {
                h.c._cursorStack ??= new iz();
                h.c._cursorStack.c = cursor.itemId;
                h.c._cursorStack.a = cursor.count;
                h.c._cursorStack.d = cursor.getDamage();
            }
            else
            {
                h.c._cursorStack = null;
            }

            // Sync slot data from real ScreenHandler
            h.d.SyncSlots();
            h.e.SyncSlots();
        }

        f.B = game?.isExternalMultiplayer ?? false;
    }

    public Minecraft(IGame game)
    {
        this.game = game;
        f = new(game);
        v = new uq();
        p = new ji();
        c = new ob();
        z = new kv();
    }

    /// <summary>
    /// Delegate set by BetaSharp.Client to return the game working directory.
    /// </summary>
    public static Func<string>? GetGameDirDelegate;

    [IKVM.Attributes.NameSigAttribute("a", "(Ljava/lang/String;)Ljava/io/File;")]
    public static java.io.File func_6264_a(string s)
    {
        var dir = GetGameDirDelegate?.Invoke() ?? ".";
        return new java.io.File(dir);
    }
}