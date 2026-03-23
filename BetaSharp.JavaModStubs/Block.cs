// uu = Block
[IKVM.Attributes.NoPackagePrefix]
public class uu
{
    public int bf;  // blockID
    public static yq @as = new yq { bf = 51 }; // fire (BlockFire), sig must be Lyq;

    // blocksList — static array of all blocks (parallels gm.c for items)
    public static uu[] bm = new uu[256];
}

// yq = BlockFire (extends Block)
[IKVM.Attributes.NoPackagePrefix]
public class yq : uu
{
}
