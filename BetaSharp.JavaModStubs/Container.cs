// dw = Container
[IKVM.Attributes.NoPackagePrefix]
public class dw
{
    public java.util.List d = new java.util.ArrayList(); // inventoryItemStacks
    public java.util.List e = new java.util.ArrayList(); // slots (List<Slot/gp>)
    public int f;            // windowId

    /// <summary>
    /// Delegate set by BetaSharp.Client to sync slot data from the real ScreenHandler.
    /// Called during UpdateFields() to keep Java slot list in sync.
    /// </summary>
    public static Action<dw>? SyncSlotsDelegate;

    public void SyncSlots() => SyncSlotsDelegate?.Invoke(this);

    // a(Container, slotId, mouseButton, shift, EntityPlayer) -> slotClick
    public virtual iz a(dw container, int slotId, int mouseButton, bool shift, gs player)
    {
        return null;
    }
}
