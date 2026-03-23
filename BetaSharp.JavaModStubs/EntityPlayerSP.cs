// gs = EntityPlayer (base), dc = EntityPlayerSP (client)

[IKVM.Attributes.NoPackagePrefix]
public class gs
{
    public ix c;       // inventory (InventoryPlayer)
    public dw d;       // inventorySlots (Container)
    public dw e;       // craftingInventory (Container)
    public string l;   // username

    public bool be;    // isDead

    // r() -> closeScreen
    public static Action? CloseScreenDelegate;
    public virtual void r() { CloseScreenDelegate?.Invoke(); }

    // W() -> isEntityAlive
    public virtual bool W() => !be;

    // a(String) -> sendChatMessage
    public static Action<string>? SendChatDelegate;
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_sendChat(string message) { SendChatDelegate?.Invoke(message); }
}

[IKVM.Attributes.NoPackagePrefix]
public class dc : gs
{
}
