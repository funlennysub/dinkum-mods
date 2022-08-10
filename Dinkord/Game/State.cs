using System.ComponentModel;
using Dinkord.Utils;
using UnityEngine;

namespace Dinkord.Game;

public enum Location
{
	[Description("In main menu")] Menu,
	[Description("In the mines")] Mines,
	[Description("On the island")] Island
}

public class State
{
	private bool _isInMainMenu;
	private bool _isUnderground;

	private NetworkMapSharer _networkMapSharer;
	private RealWorldTimeLight _realWorldTimeLight;

	public string Biome;
	public Location Location;
	public int MaxPlayers;
	public int NumOfPlayer;
	public Vector2 PlayerPos;
	public int WorldHours;
	public int WorldMinutes;

	public State(
		string biome = "Beach", bool isInMainMenu = true,
		bool isUnderground = false, Location location = Location.Menu,
		int maxPlayers = 4, int numOfPlayer = 1,
		Vector2 playerPos = default,
		int worldHours = 0, int worldMinutes = 0
	)
	{
		_networkMapSharer = NetworkMapSharer.share;
		_realWorldTimeLight = RealWorldTimeLight.time;

		Biome = biome;
		_isInMainMenu = isInMainMenu;
		_isUnderground = isUnderground;
		Location = location;
		MaxPlayers = maxPlayers;
		NumOfPlayer = numOfPlayer;
		PlayerPos = playerPos;
		WorldHours = worldHours;
		WorldMinutes = worldMinutes;
	}

	public void UpdateState()
	{
		_networkMapSharer = NetworkMapSharer.share;
		_realWorldTimeLight = RealWorldTimeLight.time;

		_isInMainMenu = IsInMainMenu();
		_isUnderground = IsUnderground();
		Location = WherePlayer();
		WorldHours = _realWorldTimeLight.currentHour;
		WorldMinutes = _realWorldTimeLight.currentMinute;
	}

	private bool IsInMainMenu() => _networkMapSharer.localChar is null;

	private bool IsUnderground() => _realWorldTimeLight.underGround;

	private Location WherePlayer() => IsInMainMenu() ? Location.Menu : IsUnderground() ? Location.Mines : Location.Island;

	
	// @TODO: Make this method not throw NRE 2 times each time you leave an island
	public Vector2 GetPlayerPosition()
	{
		if (_isInMainMenu) return new Vector2(0, 0);
		var pos = _networkMapSharer.localChar.transform.position;
		return new Vector2(pos.x, pos.z);
	}

	public static string GetBiomeName(int x, int y)
	{
		var biome = GenerateMap.generate.checkBiomType(Mathf.RoundToInt(x / 2f), Mathf.RoundToInt(y / 2f));
		return GenerateMap.generate.getBiomeNameById(biome).ToTitleCase();
	}
}