// gp = Slot
[IKVM.Attributes.NoPackagePrefix]
public class gp
{
    public int a; // slotIndex
    public int b; // xDisplayPosition
    public int c; // yDisplayPosition
    public iz? _stack; // backing stack, synced from C# ScreenHandler

    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual iz func_getStack() => _stack;

    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual bool func_getHasStack() => _stack != null;

    [IKVM.Attributes.NameSigAttribute("c", "")]
    public virtual void func_putStack(iz stack) { _stack = stack; }

    // e() -> getBackgroundIconIndex (no clash)
    public virtual int e() => -1;
}
