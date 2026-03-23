// ob = PlayerController (base)
// TMI calls this.b.c.a(windowId, slotId, mouseButton, shift, player) for slot clicks
// and this.b.c.a(windowId, player) for container close

[IKVM.Attributes.NoPackagePrefix]
public class ob
{
    // Delegates set by BetaSharp.Client
    public static Action<int, int, int, bool, gs>? SlotClickDelegate;
    public static Action<int, gs>? CloseContainerDelegate;

    // slotClick — Java name: a(IIIZ gs)
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual iz func_slotClick(int windowId, int slotId, int mouseButton, bool shift, gs player)
    {
        SlotClickDelegate?.Invoke(windowId, slotId, mouseButton, shift, player);
        return null;
    }

    // closeContainer — Java name: a(I gs)
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_closeContainer(int windowId, gs player)
    {
        CloseContainerDelegate?.Invoke(windowId, player);
    }
}
