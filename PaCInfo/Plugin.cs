using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Mirror;
using Random = UnityEngine.Random;

namespace PaCInfo;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<WeightDisplayType> WeightDisplayType;
    public static ConfigEntry<PriceDisplayType> PriceDisplayType;
    public static ConfigEntry<InfoDisplayType> InfoDisplayType;

    private static Harmony _harmony;

    private void Awake()
    {
        WeightDisplayType = Config.Bind("General", "WeightDisplayType", PaCInfo.WeightDisplayType.Exact, "Weight display type");
        PriceDisplayType = Config.Bind("General", "PriceDisplayType", PaCInfo.PriceDisplayType.Exact, "Price display type");
        InfoDisplayType = Config.Bind("General", "InfoDisplayType", PaCInfo.InfoDisplayType.Both, "Info display type");

        _harmony = Harmony.CreateAndPatchAll(typeof(PickupPatch));
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }
}

[HarmonyPatch]
internal class PickupPatch
{
    private static double GetEstPrice(SellByWeight item, double weight)
    {
        var kg = weight * item.rewardPerKG;
        var num1 = (int)kg;
        var num2 = (int)(kg / 10.0 * LicenceManager.manage.allLicences[8].getCurrentLevel());
        return num1 + num2;
    }

    private static double GetPrice(SellByWeight item, PriceDisplayType type)
    {
        return type switch
        {
            PriceDisplayType.Estimate => GetEstPrice(item, GetWeight(item, WeightDisplayType.Estimate)),
            PriceDisplayType.Exact => item.getPrice(),
            _ => 0
        };
    }

    private static double GetWeight(SellByWeight item, WeightDisplayType type)
    {
        return type switch
        {
            WeightDisplayType.Estimate => Math.Round(item.myWeight - Random.Range(-0.3f, 0.3f), 2, MidpointRounding.AwayFromZero),
            WeightDisplayType.Exact => Math.Round(item.myWeight, 2, MidpointRounding.AwayFromZero),
            _ => 0
        };
    }

    private static string ReturnMessage(SellByWeight item, WeightDisplayType wType, PriceDisplayType pType, InfoDisplayType iType)
    {
        var weight = GetWeight(item, wType);
        var price = GetPrice(item, pType);
        return iType switch
        {
            InfoDisplayType.Weight => $"{item.itemName}: {EstSign(wType)}{weight}kg",
            InfoDisplayType.Price => $"{item.itemName}: {EstSign(pType)}<sprite=11>{price:N0}",
            InfoDisplayType.Both =>
                $"{item.itemName}: {EstSign(wType)}{weight}kg, {EstSign(pType)}<sprite=11>{price:N0}",
            _ => "shrug"
        };
    }

    private static string EstSign(WeightDisplayType wType)
    {
        return wType == WeightDisplayType.Estimate ? "~" : "";
    }

    private static string EstSign(PriceDisplayType pType)
    {
        return pType == PriceDisplayType.Estimate ? "~" : "";
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharPickUp), nameof(CharPickUp.CmdPickUpObject))]
    private static void PickUpPatch(CharPickUp __instance, uint pickUpObject)
    {
        var spawned = NetworkIdentity.spawned[pickUpObject];
        if (spawned is null) return;

        var carriable = spawned.gameObject;

        var sellByWeight = carriable.GetComponent<SellByWeight>();
        if (sellByWeight is null) return;

        NotificationManager.manage.createChatNotification(
            ReturnMessage(sellByWeight, Plugin.WeightDisplayType.Value, Plugin.PriceDisplayType.Value, Plugin.InfoDisplayType.Value)
        );
    }
}

public enum WeightDisplayType
{
    Estimate,
    Exact
}

public enum PriceDisplayType
{
    Estimate,
    Exact
}

public enum InfoDisplayType
{
    Weight,
    Price,
    Both
}