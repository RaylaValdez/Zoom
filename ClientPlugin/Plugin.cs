using System;
using System.IO;
using System.Reflection;
using ClientPlugin.Config;
using ClientPlugin.GUI;
using ClientPlugin.Logging;
using HarmonyLib;
using Sandbox.Graphics.GUI;
using VRage.FileSystem;
using VRage.Plugins;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin, IDisposable
    {
        public const string Name = "Zoom";
        public static Plugin Instance { get; private set; }

        public IPluginLogger Log => Logger;
        private static readonly IPluginLogger Logger = new PluginLogger(Name);

        public IPluginConfig Config => config?.Data;
        private PersistentConfig<PluginConfig> config;
        private static readonly string ConfigFileName = $"{Name}.cfg";

        public ScrollableFOV Zoom;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;

            if (!Directory.Exists(Path.Combine(MyFileSystem.UserDataPath, "Storage\\PluginData")))
            {
                Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, "Storage\\PluginData"));
            }

            var configPath = Path.Combine(MyFileSystem.UserDataPath, "Storage\\PluginData", ConfigFileName);
            config = PersistentConfig<PluginConfig>.Load(Log, configPath);

            // TODO: Put your one time initialization code here.
            Harmony harmony = new Harmony(Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Zoom = new ScrollableFOV(harmony);
            Zoom.Init(gameInstance);
        }

        public void Dispose()
        {
            // TODO: Save state and close resources here, called when the game exits (not guaranteed!)
            // IMPORTANT: Do NOT call harmony.UnpatchAll() here! It may break other plugins.

            Instance = null;
        }

        public void Update()
        {
            Zoom.Update();
        }

        // TODO: Uncomment and use this method to create a plugin configuration dialog
        // ReSharper disable once UnusedMember.Global
        public void OpenConfigDialog()
        {
            MyGuiSandbox.AddScreen(new MyPluginConfigDialog());
        }

        //TODO: Uncomment and use this method to load asset files
        /*public void LoadAssets(string folder)
        {

        }*/
    }
}