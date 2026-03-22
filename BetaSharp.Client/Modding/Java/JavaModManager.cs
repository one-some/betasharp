extern alias JavaModStubs;

using java.net;
namespace BetaSharp.Client.Modding.Java;

public class JavaModManager
{
    private readonly BetaSharp _game;
    private readonly DirectoryInfo _modDirectory;

    public JavaModManager(BetaSharp game, DirectoryInfo modDirectory)
    {
        _game = game;
        _modDirectory = modDirectory;

        if (!modDirectory.Exists)
        {
            modDirectory.Create();
        }
    }
    public void LoadMods()
    {
        var loader = new MCClassLoader();

        Console.WriteLine($"Loading mods from {_modDirectory}");
        foreach (FileInfo file in _modDirectory.GetFiles("*.zip"))
        {
            loader.AddJar(file.FullName);

            Console.WriteLine($"{file.FullName} ...");
            var modClass = loader.loadClass("mod_TooManyItems");
            var modInstance = (JavaModStubs::BaseMod)modClass.newInstance();
            Console.WriteLine($"{file.FullName} OK!");
            Console.WriteLine($"Version: {modInstance.Version()}");
        }
    }
}