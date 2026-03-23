extern alias JavaModStubs;

using BetaSharp.Client.Modding.Java;
using java.lang;
using java.net;

public class MCClassLoader : URLClassLoader
{
    private static readonly Dictionary<string, Type> _stubs = new()
    {
        ["BaseMod"] = typeof(JavaModStubs::BaseMod),
        ["ModLoader"] = typeof(JavaModStubs::ModLoader),
        ["net.minecraft.client.Minecraft"] = typeof(JavaModStubs::net.minecraft.client.Minecraft),
        ["iz"] = typeof(JavaModStubs::iz),
        ["fd"] = typeof(JavaModStubs::fd),
        ["dc"] = typeof(JavaModStubs::dc),
        ["gs"] = typeof(JavaModStubs::gs),
        ["ix"] = typeof(JavaModStubs::ix),
        ["gm"] = typeof(JavaModStubs::gm),
        ["gp"] = typeof(JavaModStubs::gp),
        ["dw"] = typeof(JavaModStubs::dw),
        ["nh"] = typeof(JavaModStubs::nh),
        ["ub"] = typeof(JavaModStubs::ub),
        ["uq"] = typeof(JavaModStubs::uq),
        ["da"] = typeof(JavaModStubs::da),
        ["id"] = typeof(JavaModStubs::id),
        ["sj"] = typeof(JavaModStubs::sj),
        ["bb"] = typeof(JavaModStubs::bb),
        ["u"] = typeof(JavaModStubs::u),
        ["ji"] = typeof(JavaModStubs::ji),
        ["ob"] = typeof(JavaModStubs::ob),
        ["kv"] = typeof(JavaModStubs::kv),
        ["qb"] = typeof(JavaModStubs::qb),
        ["cd"] = typeof(JavaModStubs::cd),
        ["aa"] = typeof(JavaModStubs::aa),
        ["it"] = typeof(JavaModStubs::it),
        ["uu"] = typeof(JavaModStubs::uu),
        ["yq"] = typeof(JavaModStubs::yq),
        ["org.lwjgl.input.Keyboard"] = typeof(JavaModStubs::org.lwjgl.input.Keyboard),
        ["org.lwjgl.opengl.GL11"] = typeof(JavaModStubs::org.lwjgl.opengl.GL11),
    };

    /// <summary>
    /// Names of classes that need bytecode-generated concrete subclasses.
    /// Maps the concrete class name to the abstract parent it extends.
    /// </summary>
    private static readonly Dictionary<string, (string superClass, string ctorDescriptor)> _generatedClasses = new()
    {
        // ue (GuiInventory) extends id (GuiContainer) with constructor(dw)
        ["ue"] = ("id", "(Ldw;)V"),
    };

    /// <summary>
    /// Set of class names that were loaded from mod jars (not stubs).
    /// </summary>
    public HashSet<string> ModProvidedClasses { get; } = [];

    public MCClassLoader() : base([]) {}

    public void AddJar(string path)
    {
        addURL(new URL($"file:{path}"));
    }

    /// <summary>
    /// Override loadClass to ensure our stubs are always loaded through this class loader,
    /// not auto-discovered from loaded .NET assemblies by the parent class loader.
    /// </summary>
    public override Class loadClass(string name)
    {
        var loaded = findLoadedClass(name);
        if (loaded != null) return loaded;

        // If we have a stub or generated class, handle it ourselves (skip parent delegation)
        if (_stubs.ContainsKey(name) || _generatedClasses.ContainsKey(name))
            return findClass(name);

        return base.loadClass(name);
    }

    protected override Class findClass(string name)
    {
        Console.WriteLine($"[MCClassLoader] looking for {name}");

        // Check if this is a class we need to generate bytecode for
        if (_generatedClasses.TryGetValue(name, out var genInfo))
        {
            Console.WriteLine($"[MCClassLoader] generating bytecode for {name} (extends {genInfo.superClass})");
            byte[] bytecode = GenerateConcreteSubclass(name, genInfo.superClass, genInfo.ctorDescriptor);
            return defineClass(name, bytecode, 0, bytecode.Length);
        }

        if (_stubs.TryGetValue(name, out var nativeType))
        {
            // Try loading from jar first — if a mod provides this class, use it
            try
            {
                var fromJar = base.findClass(name);
                Console.WriteLine($"[MCClassLoader] mod provides {name}");
                ModProvidedClasses.Add(name);
                return fromJar;
            }
            catch
            {
                // Not in jar, use our stub
            }

            var proxyType = JavaProxyFactory.CreateProxy(name, nativeType);
            return ikvm.runtime.Util.getFriendlyClassFromType(proxyType);
        }

        return base.findClass(name);
    }

    /// <summary>
    /// Generates minimal Java bytecode for a concrete class that extends an abstract parent.
    /// The generated class has a constructor that delegates to super(param) and
    /// implements all abstract methods as no-ops (return void).
    /// </summary>
    private static byte[] GenerateConcreteSubclass(string className, string superClassName, string ctorDescriptor)
    {
        // We need to figure out what abstract methods exist and implement them.
        // For GuiContainer (id), the only abstract method is: protected void a(float) → "(F)V"
        // We hardcode this for now since we know the TMI case.
        var abstractMethods = new List<(string name, string descriptor, byte accessFlags)>
        {
            ("a", "(F)V", 0x04), // protected void a(float)
        };

        using var ms = new System.IO.MemoryStream();
        using var w = new System.IO.BinaryWriter(ms);

        // Helper to write big-endian
        void WriteU2(ushort v) { w.Write((byte)(v >> 8)); w.Write((byte)v); }
        void WriteU4(uint v) { w.Write((byte)(v >> 24)); w.Write((byte)(v >> 16)); w.Write((byte)(v >> 8)); w.Write((byte)v); }

        // Build constant pool
        var pool = new List<byte[]>();
        pool.Add([]); // index 0 is unused

        int AddUtf8(string s)
        {
            using var ms2 = new System.IO.MemoryStream();
            ms2.WriteByte(0x01); // CONSTANT_Utf8
            var bytes = System.Text.Encoding.UTF8.GetBytes(s);
            ms2.WriteByte((byte)(bytes.Length >> 8));
            ms2.WriteByte((byte)bytes.Length);
            ms2.Write(bytes);
            pool.Add(ms2.ToArray());
            return pool.Count - 1;
        }

        int AddClass(int nameIndex)
        {
            pool.Add([0x07, (byte)(nameIndex >> 8), (byte)nameIndex]);
            return pool.Count - 1;
        }

        int AddNameAndType(int nameIndex, int descriptorIndex)
        {
            pool.Add([0x0C, (byte)(nameIndex >> 8), (byte)nameIndex,
                       (byte)(descriptorIndex >> 8), (byte)descriptorIndex]);
            return pool.Count - 1;
        }

        int AddMethodref(int classIndex, int natIndex)
        {
            pool.Add([0x0A, (byte)(classIndex >> 8), (byte)classIndex,
                       (byte)(natIndex >> 8), (byte)natIndex]);
            return pool.Count - 1;
        }

        // Build pool entries
        int classNameIdx = AddUtf8(className);        // #1
        int superNameIdx = AddUtf8(superClassName);    // #2
        int thisClassIdx = AddClass(classNameIdx);     // #3
        int superClassIdx = AddClass(superNameIdx);    // #4
        int initNameIdx = AddUtf8("<init>");           // #5
        int ctorDescIdx = AddUtf8(ctorDescriptor);     // #6
        int codeIdx = AddUtf8("Code");                 // #7
        int initNatIdx = AddNameAndType(initNameIdx, ctorDescIdx); // #8
        int superInitRef = AddMethodref(superClassIdx, initNatIdx); // #9

        // Add constant pool entries for da.drawInventoryBg()V (invokestatic target)
        int daClassNameIdx = AddUtf8("da");
        int daClassIdx = AddClass(daClassNameIdx);
        int drawBgMethodNameIdx = AddUtf8("drawInventoryBg");
        int drawBgMethodDescIdx = AddUtf8("()V");
        int drawBgNatIdx = AddNameAndType(drawBgMethodNameIdx, drawBgMethodDescIdx);
        int drawBgMethodRef = AddMethodref(daClassIdx, drawBgNatIdx);

        // Add abstract method name/descriptor entries
        var methodPoolIndices = new List<(int nameIdx, int descIdx)>();
        foreach (var m in abstractMethods)
        {
            int mNameIdx = AddUtf8(m.name);
            int mDescIdx = AddUtf8(m.descriptor);
            methodPoolIndices.Add((mNameIdx, mDescIdx));
        }

        // Write class file
        WriteU4(0xCAFEBABE);   // magic
        WriteU2(0);             // minor version
        WriteU2(49);            // major version (Java 5)
        WriteU2((ushort)pool.Count); // constant_pool_count

        // Write constant pool (skip index 0)
        for (int i = 1; i < pool.Count; i++)
            w.Write(pool[i]);

        WriteU2(0x0021);                 // access_flags: PUBLIC | SUPER
        WriteU2((ushort)thisClassIdx);   // this_class
        WriteU2((ushort)superClassIdx);  // super_class
        WriteU2(0);                      // interfaces_count
        WriteU2(0);                      // fields_count

        // Methods
        int methodCount = 1 + abstractMethods.Count; // constructor + abstract implementations
        WriteU2((ushort)methodCount);

        // Constructor: calls super(param)
        // Figure out parameter count from descriptor for max_locals
        // For "(Ldw;)V" → 1 object param + this = 2 locals
        int ctorParamCount = ctorDescriptor.Split(';').Length; // rough count of object params
        int ctorMaxLocals = 1 + ctorParamCount;

        WriteU2(0x0001);                 // access_flags: PUBLIC
        WriteU2((ushort)initNameIdx);    // name_index
        WriteU2((ushort)ctorDescIdx);    // descriptor_index
        WriteU2(1);                      // attributes_count (Code)

        // Code attribute for constructor
        // Bytecode: aload_0, aload_1, invokespecial #superInitRef, return
        byte[] ctorCode = [0x2A, 0x2B, 0xB7, (byte)(superInitRef >> 8), (byte)superInitRef, 0xB1];
        WriteU2((ushort)codeIdx);
        WriteU4((uint)(2 + 2 + 4 + ctorCode.Length + 2 + 2)); // attribute_length
        WriteU2(2);                      // max_stack
        WriteU2((ushort)ctorMaxLocals);  // max_locals
        WriteU4((uint)ctorCode.Length);  // code_length
        w.Write(ctorCode);
        WriteU2(0);                      // exception_table_length
        WriteU2(0);                      // code attributes_count

        // Abstract method implementations (all return void)
        for (int i = 0; i < abstractMethods.Count; i++)
        {
            var m = abstractMethods[i];
            var (mNameIdx, mDescIdx) = methodPoolIndices[i];

            WriteU2(m.accessFlags);          // access_flags (protected)
            WriteU2((ushort)mNameIdx);       // name_index
            WriteU2((ushort)mDescIdx);       // descriptor_index
            WriteU2(1);                      // attributes_count (Code)

            // Code attribute: call da.drawInventoryBg() then return
            // invokestatic #drawBgMethodRef, return
            byte[] methodCode = [
                0xB8, (byte)(drawBgMethodRef >> 8), (byte)drawBgMethodRef, // invokestatic
                0xB1  // return
            ];
            WriteU2((ushort)codeIdx);
            WriteU4((uint)(2 + 2 + 4 + methodCode.Length + 2 + 2));
            WriteU2(1);                  // max_stack (for invokestatic, no args on stack)
            WriteU2(2);                  // max_locals (this + float param)
            WriteU4((uint)methodCode.Length);
            w.Write(methodCode);
            WriteU2(0);                  // exception_table_length
            WriteU2(0);                  // code attributes_count
        }

        WriteU2(0); // class attributes_count

        return ms.ToArray();
    }
}
