using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Dinkord.Discord;

namespace Dinkord;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ManualLogSource Log;

    private static ConfigEntry<int> _nexusID;
    private static RichPresence _richPresence;

    public Plugin()
    {
        _nexusID = Config.Bind("Other", "NexusID", 40, "Nexus Mod ID. You can find it on the mod's page on nexusmods.com");
    }

    private void Awake()
    {
        Log = Logger;

        var (largeKey, smallKey, smallImageFeature) = PresenceConfig.GetConfig(Config);

        var _ = new PresenceConfig(largeKey, smallKey, smallImageFeature);
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
    }

    private void Start()
    {
        _richPresence = new RichPresence();
    }

    private void Update()
    {
        _richPresence.Update();
    }

    private void OnDestroy()
    {
        _richPresence.Dispose();
    }
}