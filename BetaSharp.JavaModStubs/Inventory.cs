// ix = InventoryPlayer
[IKVM.Attributes.NoPackagePrefix]
public class ix
{
    public iz[] a;   // mainInventory
    public iz[] b;   // armorInventory
    public int c;    // currentItem

    public iz? _cursorStack; // item being dragged by cursor (null when not dragging)

    // i() -> getItemStack (cursor/dragging item, NOT hotbar item)
    public virtual iz i() => _cursorStack;

    // a(ItemStack) -> addItemStackToInventory -> boolean
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual bool func_addItemStackToInventory(iz stack)
    {
        AddItemDelegate?.Invoke(stack);
        return true;
    }

    // Delegate set by BetaSharp.Client
    public static Action<iz>? AddItemDelegate;

    // b(ItemStack) -> setItemStack
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual void func_setItemStack(iz stack) { }
}
