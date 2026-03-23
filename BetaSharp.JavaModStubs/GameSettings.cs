// kv = GameSettings
// TMI accesses this.b.z.r.b for inventory key code

[IKVM.Attributes.NoPackagePrefix]
public class kv
{
    public qb r; // keyBindInventory

    public kv()
    {
        r = new qb();
    }
}

// qb = KeyBinding
[IKVM.Attributes.NoPackagePrefix]
public class qb
{
    public int b; // keyCode
}
