# TennisFightingGame
A fighting game with none of the fighting and all of the tennis.

## Requirements
* [.NET framework](https://dotnet.microsoft.com/download/dotnet-framework/) 3 or above
  * Or [Mono](https://www.mono-project.com/download/stable/) on Linux, alternatively

## Controllers
This game only supports XInput devices, for the time being, at least. However, XInput emulators do exist, such as [x360ce](https://www.x360ce.com/) which supports most controllers out there.

## Configuration
A TennisFightingGame.ini file can be found alongside the game's executable, allowing you to modify most game settings.

### Input devices
Each player can have a different input device asociated with it, set by InputMethod under each player's [PX Settings]. 0 is keyboard, 1 is XInput device.

Only keyboard and XInput input devices are supported, for the time being, at least. However, XInput emulators do exist, such as [x360ce](https://www.x360ce.com/), which allow you to essentially use any controller.

### Controls mapping
Each player has a set of *actions* which can be mapped to a key or button each, all found under each player's [PX Controls] section.

Comments in the configuration file explain how to find out values for keyboard keys or XInput device buttons.

### Video settings (unimplemented)
The game will be rendered at a base resolution of 960x405 ("21:9" aspect ratio) and be scalable.

It's possible that a alternative base resolution of 16:9 aspect ratio will be implemented but it's unlikely, since the game doesn't play that well in most 16:9 resolutions due to the comparatively low width.

None of this is implemented yet. Only fullscreen and custom resolutions are, which are found in the [Video] section.
