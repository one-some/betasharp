extern alias JavaModStubs;

using java.net;
namespace BetaSharp.Client.Modding.Java;

public class JavaModManager
{
    private readonly BetaSharp _game;
    private readonly DirectoryInfo _modDirectory;

    public MCClassLoader? ClassLoader { get; private set; }

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
        ClassLoader = new MCClassLoader();

        Console.WriteLine($"Loading mods from {_modDirectory}");
        foreach (FileInfo file in _modDirectory.GetFiles("*.zip"))
        {
            ClassLoader.AddJar(file.FullName);

            Console.WriteLine($"{file.FullName} ...");
            var modClass = ClassLoader.loadClass("mod_TooManyItems");
            var modInstance = (JavaModStubs::BaseMod)modClass.newInstance();
            Console.WriteLine($"{file.FullName} OK!");
            Console.WriteLine($"Version: {modInstance.Version()}");
        }
    }

    /// <summary>
    /// Try to create a Java GuiContainer (id) screen from a mod.
    /// Since TMI's id.class is abstract, we load a bytecode-generated concrete
    /// subclass "ue" (GuiInventory) that extends TMI's id via the class loader.
    /// Returns null if no mod provides id.class.
    /// </summary>
    public JavaModStubs::da? TryCreateJavaGuiContainer(JavaModStubs::dw container,
        JavaModStubs::net.minecraft.client.Minecraft mcStub)
    {
        if (ClassLoader == null) return null;

        try
        {
            // Ensure id is loaded first (triggers mod-provided detection)
            ClassLoader.loadClass("id");

            if (!ClassLoader.ModProvidedClasses.Contains("id"))
                return null;

            // Load ue (concrete subclass of id, generated as bytecode by MCClassLoader)
            var ueClass = ClassLoader.loadClass("ue");

            // Instantiate via Java reflection: new ue(dw)
            var dwClass = ikvm.runtime.Util.getFriendlyClassFromType(typeof(JavaModStubs::dw));
            var constructor = ueClass.getConstructor(dwClass);
            var instance = constructor.newInstance(container);
            Console.WriteLine($"[JavaModManager] Created Java GuiContainer: {instance.GetType().Name}");
            return (JavaModStubs::da)instance;
        }
        catch (Exception ex)
        {
            var inner = ex;
            while (inner.InnerException != null) inner = inner.InnerException;
            Console.WriteLine($"[JavaModManager] Failed to create Java GuiContainer: {inner.GetType().Name}: {inner.Message}");
            Console.WriteLine(inner.StackTrace);
            return null;
        }
    }
}
