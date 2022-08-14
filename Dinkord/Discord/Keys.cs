using Dinkord.Utils;

namespace Dinkord.Discord;

public enum SmallKeys
{
    [Key("small_base_default", "An image of a watermelon [Constant]")] BASE_DEFAULT,
    [Key("small_cycle_sunrise", "An image representing sunrise, changes depending on world time")] CYCLE_SUNRISE,
    [Key("small_cycle_daytime", "An image representing daytime, changes depending on world time")] CYCLE_DAYTIME,
    [Key("small_cycle_night", "An image representing night, changes depending on world time")] CYCLE_NIGHT
}

public enum LargeKeys
{
    [Key("large_base_default", "An image of the game's name.")] BASE_DEFAULT
}