# Docking-Sounds (DPSoundFX)
## Synopsis

Adds docking and undocking sound effects to stock docking ports in Kerbal Space Program(tm).  

## Motivation

Simply put, I wanted to hear something when 2 ships docked.  I was so used to it in Orbiter (an open source space simulator) that I sorely missed it.  So much so that I jumped into coding (again) to make it happen.

My effects are based on my perception that Jeb wouldn't dock so much as "participate in a controlled collision"

## Installation

**Be sure to have the latest version of [ModuleManger](http://forum.kerbalspaceprogram.com/index.php?/topic/50533-110-module-manager-2622-april-19th-with-even-more-sha-and-less-bug/) installed!**

Upzip it into your GameData folder, as with most other mods.

## API Reference

Using a ModuleManager config, like below, you can specify your own sounds for any docking port in the game.

```
@PART[someDockingPort]:NEEDS[FP_DPSoundFX]
{
	MODULE
	{
		name = DPSoundFX
		sound_docking = CustomExample/crash
		sound_undocking = CustomExample/radio
		internalSoundsOnly = false		
	}
}
```

## How-To-Use

Just listen, when docking or undocking of course!!

## Contributors

Guru's who answered my questions:

- Snjo
- CrzyRndm

Authors whose plugins were studied for clues and code:

- Pizzaoverhead

## License

GNU GENERAL PUBLIC LICENSE V3 dated 6/29/2007

