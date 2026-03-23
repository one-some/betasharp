// gm = Item
[IKVM.Attributes.NoPackagePrefix]
public class gm
{
    public int bf;             // itemID
    public int _maxStackSize = 64;
    public int _iconIndex;
    public string? _itemName; // translation key e.g. "item.shovelIron" or "tile.stone"
    public bool _hasSubtypes;
    public static gm[] c = new gm[32000]; // itemsList (static array of all items)

    // d() -> getMaxStackSize
    public virtual int d() => _maxStackSize;
    // b(ItemStack) -> getIconIndex
    public virtual int b(iz stack) => _iconIndex;
    // a(iz) -> getItemNameIS (returns translation key for given stack)
    public virtual string a(iz stack) => _itemName ?? "";
}
