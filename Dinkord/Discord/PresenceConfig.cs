using BepInEx.Configuration;
using Dinkord.Utils;

namespace Dinkord.Discord;

public class PresenceConfig
{
	public static LargeKeys LargeKey;
	public static SmallKeys SmallKey;
	public static RichPresence.SmallImageFeatures EnabledSmallImageFeature;

	public PresenceConfig(LargeKeys largeKey, SmallKeys smallKey, RichPresence.SmallImageFeatures enabledSmallImageFeature)
	{
		LargeKey = largeKey;
		SmallKey = smallKey;
		EnabledSmallImageFeature = enabledSmallImageFeature;
	}

	public static (LargeKeys, SmallKeys, RichPresence.SmallImageFeatures) GetConfig(ConfigFile config)
	{
		var defaultLargeKey = config.Bind(
			"Image Keys", "defaultLargeKey",
			LargeKeys.BASE_DEFAULT, ConfigUtils.GenerateConfigDescription(LargeKeys.BASE_DEFAULT)
		);
		var defaultSmallKey = config.Bind(
			"Image Keys", "defaultSmallKey",
			SmallKeys.BASE_DEFAULT, ConfigUtils.GenerateConfigDescription(SmallKeys.BASE_DEFAULT)
		);
		var smallImgFeature = config.Bind(
			"Image Keys", "smallKeyFeature",
			RichPresence.SmallImageFeatures.DEFAULT,
			ConfigUtils.GenerateConfigDescription(RichPresence.SmallImageFeatures.DEFAULT, RichPresence.SmallImageFeatures.DAY_CYCLE)
		);

		return (defaultLargeKey.Value, defaultSmallKey.Value, smallImgFeature.Value);
	}
}