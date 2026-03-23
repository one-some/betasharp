using BetaSharp;

[IKVM.Attributes.NoPackagePrefix]
public class fd
{
    private IGame game;

    // isMultiplayer
    public bool B = false;

    public fd(IGame game)
    {
        this.game = game;
    }
}