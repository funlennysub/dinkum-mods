using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Dinkord.Discord;

namespace Dinkord;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    public static ManualLogSource Log;

    private static ConfigEntry<int> _nexusID;
    private static RichPresence _richPresence;

    private void Awake()
    {
        Log = Logger;

        var (largeKey, smallKey, smallImageFeature) = PresenceConfig.GetConfig(Config);
        var _ = new PresenceConfig(largeKey, smallKey, smallImageFeature);
        _nexusID = Config.Bind("Other", "NexusID", 40, "Nexus Mod ID. You can find it on the mod's page on nexusmods.com");
        Logger.LogInfo($"Plugin {Id} is loaded!");
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