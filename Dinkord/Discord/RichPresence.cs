using System;
using System.ComponentModel;
using Dinkord.Discord.Core;
using Dinkord.Game;
using Dinkord.Utils;

namespace Dinkord.Discord;

public class RichPresence
{
    public enum SmallImageFeatures
    {
        [Description("Display static image of \"defaultSmallKey\"")] DEFAULT,
        [Description("Display images based on time of the world")] DAY_CYCLE
    }

    private const long clientId = 1002736046578073743;
    private readonly ActivityManager _activityManager;
    private readonly Core.Discord _discord;

    private readonly long _startTime;

    private readonly State _state;
    private Activity _activity;

    public RichPresence()
    {
        _state = new State();

        _startTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        _discord = new Core.Discord(clientId, (ulong)CreateFlags.Default);
        _discord.SetLogHook(LogLevel.Error, (level, message) => Plugin.Log.LogError($"[{level.ToString()}]\n" + message));

        _activityManager = _discord.GetActivityManager();
        SetActivity();
    }

    private void SetActivity()
    {
        _activity = new Activity
        {
            Details = "In Main Menu",
            Timestamps = { Start = _startTime },
            Assets =
            {
                LargeImage = PresenceConfig.LargeKey.GetKeyName(),
                LargeText = null,
                SmallImage = PresenceConfig.SmallKey.GetKeyName(),
                SmallText = "placeholder"
            }
        };
    }

    private void SetActivity(State state)
    {
        _activity.Details = state.Location.GetEnumDescription();
        _activity.Timestamps.Start = _startTime;
        _activity.Assets.LargeImage = LargeKeys.BASE_DEFAULT.GetKeyName();
        _activity.Assets.SmallText = PresenceConfig.EnabledSmallImageFeature switch
        {
            SmallImageFeatures.DEFAULT => "watermelon",
            SmallImageFeatures.DAY_CYCLE => _state.Location is Location.Island or Location.Mines
                ? $"Current time: {_state.WorldHours:D2}:{_state.WorldMinutes:D2}"
                : "watermelon",
            _ => _state.Biome
        };
        _activity.Assets.SmallImage = PresenceConfig.EnabledSmallImageFeature switch
        {
            SmallImageFeatures.DEFAULT => SmallKeys.BASE_DEFAULT.GetKeyName(),
            SmallImageFeatures.DAY_CYCLE => _state.Location is Location.Island or Location.Mines
                ? _state.WorldHours switch
                {
                    >= 7 and <= 13 => SmallKeys.CYCLE_SUNRISE.GetKeyName(),
                    >= 13 and <= 19 => SmallKeys.CYCLE_DAYTIME.GetKeyName(),
                    _ => SmallKeys.CYCLE_NIGHT.GetKeyName()
                }
                : SmallKeys.BASE_DEFAULT.GetKeyName(),
            _ => SmallKeys.BASE_DEFAULT.GetKeyName()
        };
    }

    public void Update()
    {
        _state.UpdateState();

        switch (_state.Location)
        {
            case Location.Menu:
                _state.Location = Location.Menu;
                break;
            case Location.Mines:
                _state.Location = Location.Mines;
                _state.Biome = "Underground";
                break;
            case Location.Island:
                var pos = _state.GetPlayerPosition();
                var biome = State.GetBiomeName((int)pos.x, (int)pos.y);

                _state.Location = Location.Island;
                _state.Biome = biome;
                break;
            default:
                _state.Location = Location.Menu;
                break;
        }

        SetActivity(_state);
        _activityManager.UpdateActivity(_activity, _ => { });
        _discord.RunCallbacks();
    }

    public void Dispose()
    {
        _discord.Dispose();
    }
}