# Dinkord

### Description

Dinkum mod which adds support for Discord Rich Presence.

### ⚠ Important notice ⚠

Mod is currently in development and not all features are present in the current build. At the moment, there's only support for displaying your current
biome and where you are currently: in the main menu or on the island.

### Installation

- Install [BepInEx](https://builds.bepinex.dev/projects/bepinex_be/576/BepInEx_UnityMono_x64_e473f08_6.0.0-be.576.zip) by following these [steps](https://docs.bepinex.dev/master/articles/user_guide/installation/unity_mono.html?tabs=tabid-win).
- Download [Discord Game sdk](https://dl-game-sdk.discordapp.net/2.5.6/discord_game_sdk.zip)
    - Go to `discord_game_sdk/lib/x86_64` and place `discord_game_sdk.dll` into the game folder where `Dinkum.exe` is located.
- Extract `dev.funlennysub.dinkord.dll` into `(Dinkum folder)/BepInEx/plugins/`

### Troubleshooting

- If you don't see `Dinkum` activity in your profile:
    - Check your Discord settings (`Settings -> Activity Privacy`) and make sure `Display current activity as a status message` is enabled.
    - Make sure that other mods work, if not - make sure that you [installed](#Installation) BepInEx correctly
 