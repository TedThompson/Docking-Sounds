DockingSounds 
-------------
Thanks for downloading! 

This is version 1.2.1!



LICENSE
-------
DockingSounds is licensed under a Creative Commons Attribution-ShareAlike 4.0 International (CC BY-SA 4.0)
https://creativecommons.org/licenses/by-sa/4.0/



INSTALLATION
------------
Copy the "GameData" folder from this archive into the installation folder of KSP.  This will result in a new folder within GameData, GameData/DPSoundFX and it's subdirectories.

This mod requires Module Manager to work!!

If you wish, you can specify custom sounds for any docking port by adding the following code (via Module Manager or manually) to the part's cfg file:

	MODULE
	{
		name = DPSoundFX
		sound_docking = DPSoundFX/Sounds/dock
		sound_undocking = DPSoundFX/Sounds/undock
	}

sound_docking and sound_undocking should be the path/filename of your sound file WITHOUT and extention.  Note that WAV, MP3, OOG and any other format supported by KSP/Unity should work fine here.

An example "CustomExample" is included in the archive.

CHANGELOG
---------

1.0 - Inital Release

1.1 - Fixed bug where KIS container mounting would trigger the docking sound.

1.1.0.1 - Fixed a config error

1.2 - Checked for 1.0.4 compatibility. Added IVA only setting, corrected bug where sounds would not always come from the ports that were docking, Added code so effect volume tracks changes in Spacecraft volume setting.

1.2.1 - Rerelease for new MOD repo.  Changed folder structure.
