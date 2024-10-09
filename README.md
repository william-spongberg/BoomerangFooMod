BoomerangFoo - Custom Boomerang Fu Mod
======================================

This is a mod for the 2020 couch PvP party game *Boomerang Fu* by Cranky Watermelon. It provides custom settings, powerup modifiers, and (soon) gamemodes. It is built with the [BepInEx](https://github.com/BepInEx/BepInEx/) runtime patching framework. The mod was originally created by @ws13sk and adapted to BepInEx by @Jeffjewett27. 

## Installation

1. Download `BoomerangFu.zip` in the releases.
2. Unzip the files into `C:\Program Files (x86)\Steam\steamapps\common\Boomerang Fu`. The BepInEx folder should be in the same folder as `Boomerang Fu.exe`.
3. Open the game to initialize the files and configs.

## Features

All the features in this mod are controlled by the `customConfig.ini` file in the *Boomerang Fu* installation, which will be generated after you open the game.

### General Settings
- MaxPowerups: Change the maximum number of powerups (default 3)
- EnableDuplicatedCharacters: If true, then multiple players can be the same character (default True)
- StartupPowers: Select the codes for powerups that will be given to players at the start of a match (default None)
- RapidPowerupSpawning: If true, then powerups will spawn much more often (default False)
- SuddenDeathTimeLimit: The amount of seconds before the boundary closes in. If 0, then it uses the default time limit (default 0)
- ShrinkingBoundsFinalTimer: Once the boundary has shrunk, this is the amount of time you can survive outside (default 3)
- MatchScoreLimit: The number of points needed to win the match. If 0, then it uses the default score limit (default 0)
- BoomerangSize: Changes the size of boomerangs. If 0, then it uses the default boomerang size of 0.5 (default 0)
- LevelPicker: Select a list of level IDs to use in the match. These will be shuffled. If none specified, then it will use the default level playlist (default None)

### Powerup Modifiers

- Shield
  - ShieldCounter: The amount of hits that a shield will tank before disappearing (default 1)
- Decoy
  - MaxDecoyCount: The number of times you can dash and create a decoy (default 1)
- Caffeinated 
  - MoveFasterAttackCooldown: When Caffeinated, this changes the time between melee attacks (default 0.66)
  - MoveFasterDashForceMultiplier: When Caffeinated, this multiplies the speed of the dash (default 1)
  - MoveFasterDashDurationMultiplier: When Caffeinated, this multiplies the duration/length of the dash (default 1)
  - MoveFasterDashCooldownMultiplier: When Caffeinated, this multiplies the time between dashes (default 1)
  - MoveFasterMoveSpeedMultiplier: When Caffeinated, this multiplies general move speed (default 1)
  - MoveFasterTurnSpeed: When Caffeinated, this sets the speed at which you change direction (default 12)
- Bamboozled
  - ReverseInputsImmunity: If true, you are invincible while bamboozled. Kind of broken, ngl (default False)
  - ReverseInputsDuration: The number of seconds that bamboozled lasts (default 6)
- Fire
  - FirePowerBurnDuration: The number of seconds you will survive while on fire (default 2.5)
    - Note: This does not control how long you are on fire. That is affected by how much you are moving around.
- Explosive
  - ExplosiveRadiusMultiplier: This multiplies the size of regular and fire explosions (default 1)
  - ExplosiveFreezingRadiusMultiplier: This multiplies the size of ice explosions (default 1)
  - ExplosiveMiniRadiusMultiplier: This multiplies the size of mini explosions from multishot. This stacks on top of ExplosiveRadiusMultiplier (default 1)

There are also some custom modifiers you can apply to any powerup.
- Flying: Gives you temporary flight when over a water or a hazard.
  - FlyingPowerUp: The ID of the powerup which should grant flying (default Dash-Through-Walls)
  - FlyingDuration: The amount of time you can fly (default 0)
- Bounciness: Changes how fast boomerangs bounce off walls.
  - BoomerangBouncinessMultiplierTiedToPowerup: The ID of the powerup which should grant bounciness. If 0, then it is applied to all powerups (default 0)
  - BoomerangBouncinessMultiplier: The multiplier of how much the boomerang should bounce (default 1)

### Unused Settings
I am adapting this mod from an existing mod, and so not all the features have been ported. A lot of the settings do nothing currently.

- GameMode
- PowerDrainLoseAll
- RamboHulkKeepSwapping
- RamboHulkEnableRevive
- RamboHulkPowerups
- RamboHulkOthersPowerups
- SurviveTillDawnTimer
- SurviveTillDawnKillGainTime
- SurviveTillDawnHunterPowerups
- SurviveTillDawnOthersPowerups
- SurviveTillDawnRespawning
- TeamGoldenBoomerang
- TeamGoldenBoomerangTimeLimit
- CameraFlip
- CameraFlipRandom
- MultiBumerangSplit
- ShrinkingBoundsKicksOnly

## Disclaimer
This mod is unaffiliated with Cranky Watermelon and contains no *Boomerang Fu* assets.

## License
Distributed under the MIT [license](https://github.com/Jeffjewett27/BoomerangFoo/blob/master/LICENSE.txt).