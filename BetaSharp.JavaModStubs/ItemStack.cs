[IKVM.Attributes.NoPackagePrefix]
public class iz
{
    public int a; // stackSize
    public int b; // animationsToGo
    public int c; // itemID
    public int d; // itemDamage

    public iz() { }

    // iz(int itemID, int stackSize, int damage)
    public iz(int itemID, int stackSize, int damage)
    {
        c = itemID;
        a = stackSize;
        d = damage;
    }

    // iz(Item, int stackSize, int damage)
    public iz(gm item, int stackSize, int damage)
    {
        c = item?.bf ?? 0;
        a = stackSize;
        d = damage;
    }

    // iz(Block) — creates stack of 1 block
    public iz(uu block)
    {
        c = block?.bf ?? 0;
        a = 1;
        d = 0;
    }

    // a(int) -> splitStack / copy with count
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual iz func_splitStack(int count)
    {
        a -= count;
        return new iz(c, count, d);
    }

    // i() -> getItemDamage
    public virtual int i() => d;
    // l() -> getItemName — returns translation key + ".name" (like vanilla MC)
    public virtual string l()
    {
        if (c >= 0 && c < gm.c.Length && gm.c[c] != null)
            return gm.c[c].a(this) + ".name";
        return "";
    }
}