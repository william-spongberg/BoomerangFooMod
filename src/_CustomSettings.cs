using BoomerangFoo.Powerups;
using System;
using System.IO;
using System.Linq;

namespace BoomerangFoo
{
    public static class _CustomSettings
    {
        public static int MaxPowerUps;

        public static int ShieldCounter;

        public static int MultiBumerangSplit;

        public static int MaxDecoyCount;

        public static bool EnableDuplicatedCharacters;

        public static bool RapidPowerUpSpawning;

        public static PowerupType StartupPowerUps;

        public static float SuddenDeathTimeLimit;

        public static float BoomerangBouncinessMultiplier;

        public static PowerupType BoomerangBouncinessMultiplierTiedToPowerUp;

        public static int MatchScoreLimit;

        public static bool TeamGoldenBoomerang;

        public static float TeamGoldenBoomerangTimeLimit;

        public static float BoomerangSize;

        public static int[] LevelPicker;

        public static bool ReviveAsDecoy;

        public static float MoveFasterAttackCooldown;

        public static float MoveFasterDashForceMultiplier;

        public static float MoveFasterDashDurationMultiplier;

        public static float MoveFasterDashCooldownMultiplier;

        public static float MoveFasterMoveSpeedMultiplier;

        public static float MoveFasterTurnSpeed;

        public static bool ReverseInputsImmunity;

        public static float ReverseInputsDuration;

        public static PowerupType FlyingPowerUp;

        public static float FlyingDuration;

        public static float FirePowerBurnDuration;

        public static bool FirePowerImmunityToFire;

        public static bool ShrinkingBoundsKicksOnly;

        public static float ShrinkingBoundsFinalTimer;

        public static float ExplosiveRadiusMultiplier;

        public static float ExplosiveFreezingRadiusMultiplier;

        public static float ExplosiveMiniRadiusMultiplier;

        public static int CameraFlip;

        public static bool CameraFlipRandom;

        public static eGameMode GameMode;

        public static bool PowerDrainLoseAll;

        public static bool RamboHulkKeepSwapping;

        public static PowerupType RamboHulkPowerups;

        public static PowerupType RamboHulkOthersPowerups;

        public static bool RamboHulkEnableRevive;

        public static float SurviveTillDawnTimer;

        public static float SurviveTillDawnKillGainTime;

        public static bool SurviveTillDawnRespawning;

        public static PowerupType SurviveTillDawnHunterPowerups;

        public static PowerupType SurviveTillDawnOthersPowerups;

        public static string ModAuthor => "wq13sk, FuzzyJeffTheory";

        public static string ModVersion => "0.2";

        public static string ModReleaseDate => "2024-10-06";

        public static string FileLocation => "customConfig.ini";

        static _CustomSettings()
        {
            MaxPowerUps = 3;
            ShieldCounter = ShieldPowerup.ShieldHits;
            MultiBumerangSplit = MultiboomerangPowerup.MultiBoomerangSplit;
            MaxDecoyCount = DecoyPowerup.MaxDecoyCount;
            EnableDuplicatedCharacters = true;
            RapidPowerUpSpawning = false;
            StartupPowerUps = PowerupType.None;
            SuddenDeathTimeLimit = 0f;
            BoomerangBouncinessMultiplier = 1f;
            BoomerangBouncinessMultiplierTiedToPowerUp = PowerupType.None;
            MatchScoreLimit = 0;
            TeamGoldenBoomerang = false;
            TeamGoldenBoomerangTimeLimit = 30f;
            BoomerangSize = 0f;
            LevelPicker = new int[0];
            ReviveAsDecoy = DecoyPowerup.ReviveAsDecoy;
            MoveFasterAttackCooldown = MoveFasterPowerup.AttackCooldown;
            MoveFasterDashForceMultiplier = MoveFasterPowerup.DashForceMultiplier;
            MoveFasterDashDurationMultiplier = MoveFasterPowerup.DashDurationMultiplier;
            MoveFasterDashCooldownMultiplier = MoveFasterPowerup.DashCooldownMultiplier;
            MoveFasterMoveSpeedMultiplier = MoveFasterPowerup.MoveSpeedMultiplier;
            MoveFasterTurnSpeed = MoveFasterPowerup.TurnSpeed;
            ReverseInputsImmunity = BamboozlePowerup.Immunity;
            ReverseInputsDuration = BamboozlePowerup.Duration;
            FlyingPowerUp = PowerupType.DashThroughWalls;
            FlyingDuration = FlyingPowerup.Duration;
            FirePowerBurnDuration = FirePowerup.BurnDuration;
            FirePowerImmunityToFire = true;
            ShrinkingBoundsKicksOnly = false;
            ShrinkingBoundsFinalTimer = 0.5f;
            ExplosiveRadiusMultiplier = ExplosivePowerup.RadiusMultiplier;
            ExplosiveFreezingRadiusMultiplier = ExplosivePowerup.FreezingRadiusMultiplier;
            ExplosiveMiniRadiusMultiplier = ExplosivePowerup.MiniRadiusMultiplier;
            // TODO camera flip
            CameraFlip = 0;
            CameraFlipRandom = false;
            GameMode = eGameMode.Standard;
            PowerDrainLoseAll = false;
            RamboHulkKeepSwapping = true;
            RamboHulkPowerups = PowerupType.MoveFaster | PowerupType.DashThroughWalls | PowerupType.ExtraDisc;
            RamboHulkOthersPowerups = PowerupType.Shield | PowerupType.Disguise;
            RamboHulkEnableRevive = false;
            SurviveTillDawnTimer = 20f;
            SurviveTillDawnKillGainTime = 5f;
            SurviveTillDawnRespawning = false;
            SurviveTillDawnHunterPowerups = PowerupType.DashThroughWalls | PowerupType.ExtraDisc;
            SurviveTillDawnOthersPowerups = PowerupType.Disguise | PowerupType.Decoy;
            if (File.Exists(FileLocation))
            {
                LoadSettings();
            }
            else
            {
                SaveSettings();
            }
        }

        public static void Reset()
        {
            LoadSettings();
        }

        private static void LoadSettings()
        {
            string text = File.ReadAllText(FileLocation);
            string[] array = text.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
            string[] array2 = array;
            foreach (string text2 in array2)
            {
                try
                {
                    string text3 = text2.Trim();
                    if (text3 == string.Empty || text3.StartsWith("//") || text3.StartsWith("[") || !text3.Contains("="))
                    {
                        continue;
                    }
                    string text4 = text3.Split('=')[0];
                    string text5 = text3.Split('=')[1].Split('/')[0].Trim();
                    switch (text4)
                    {
                        case "MaxPowerUps":
                            MaxPowerUps = Convert.ToInt32(text5);
                            break;
                        case "ShieldCounter":
                            ShieldCounter = Convert.ToInt32(text5);
                            ShieldPowerup.ShieldHits = ShieldCounter;
                            break;
                        case "MultiBumerangSplit":
                            MultiBumerangSplit = Convert.ToInt32(text5);
                            MultiboomerangPowerup.MultiBoomerangSplit = MultiBumerangSplit;
                            break;
                        case "MaxDecoyCount":
                            MaxDecoyCount = Convert.ToInt32(text5);
                            DecoyPowerup.MaxDecoyCount = MaxDecoyCount;
                            break;
                        case "MoveFasterAttackCooldown":
                            MoveFasterAttackCooldown = float.Parse(text5);
                            MoveFasterPowerup.AttackCooldown = MoveFasterAttackCooldown;
                            break;
                        case "MoveFasterDashForceMultiplier":
                            MoveFasterDashForceMultiplier = float.Parse(text5);
                            MoveFasterPowerup.DashForceMultiplier = MoveFasterDashForceMultiplier;
                            break;
                        case "MoveFasterDashDurationMultiplier":
                            MoveFasterDashDurationMultiplier = float.Parse(text5);
                            MoveFasterPowerup.DashDurationMultiplier = MoveFasterDashDurationMultiplier;
                            break;
                        case "MoveFasterDashCooldownMultiplier":
                            MoveFasterDashCooldownMultiplier = float.Parse(text5);
                            MoveFasterPowerup.DashCooldownMultiplier = MoveFasterDashCooldownMultiplier;
                            break;
                        case "MoveFasterMoveSpeedMultiplier":
                            MoveFasterMoveSpeedMultiplier = float.Parse(text5);
                            MoveFasterPowerup.MoveSpeedMultiplier = MoveFasterMoveSpeedMultiplier;
                            break;
                        case "MoveFasterTurnSpeed":
                            MoveFasterTurnSpeed = float.Parse(text5);
                            MoveFasterPowerup.TurnSpeed = MoveFasterTurnSpeed;
                            break;
                        case "ReverseInputsImmunity":
                            ReverseInputsImmunity = Convert.ToBoolean(text5);
                            BamboozlePowerup.Immunity = ReverseInputsImmunity;
                            break;
                        case "ReverseInputsDuration":
                            ReverseInputsDuration = float.Parse(text5);
                            BamboozlePowerup.Duration = ReverseInputsDuration;
                            break;
                        case "FlyingPowerUp":
                            FlyingPowerUp = (PowerupType)Convert.ToInt32(text5);
                            break;
                        case "FlyingDuration":
                            FlyingDuration = float.Parse(text5);
                            FlyingPowerup.Duration = FlyingDuration;
                            break;
                        case "FirePowerBurnDuration":
                            FirePowerBurnDuration = float.Parse(text5);
                            FirePowerup.BurnDuration = FirePowerBurnDuration;
                            break;
                        case "FirePowerImmunityToFire":
                            FirePowerImmunityToFire = Convert.ToBoolean(text5);
                            break;
                        case "GameMode":
                            GameMode = (eGameMode)Convert.ToInt32(text5);
                            break;
                        case "PowerDrainLoseAll":
                            PowerDrainLoseAll = Convert.ToBoolean(text5);
                            break;
                        case "RamboHulkKeepSwapping":
                            RamboHulkKeepSwapping = Convert.ToBoolean(text5);
                            break;
                        case "RamboHulkEnableRevive":
                            RamboHulkEnableRevive = Convert.ToBoolean(text5);
                            break;
                        case "RamboHulkPowerups":
                            RamboHulkPowerups = (PowerupType)Convert.ToInt32(text5);
                            break;
                        case "RamboHulkOthersPowerups":
                            RamboHulkOthersPowerups = (PowerupType)Convert.ToInt32(text5);
                            break;
                        case "ShrinkingBoundsKicksOnly":
                            ShrinkingBoundsKicksOnly = Convert.ToBoolean(text5);
                            break;
                        case "ShrinkingBoundsFinalTimer":
                            ShrinkingBoundsFinalTimer = float.Parse(text5);
                            break;
                        case "EnableDuplicatedCharacters":
                            EnableDuplicatedCharacters = Convert.ToBoolean(text5);
                            break;
                        case "RapidPowerUpSpawning":
                            RapidPowerUpSpawning = Convert.ToBoolean(text5);
                            break;
                        case "StartupPowerUps":
                            StartupPowerUps = (PowerupType)Convert.ToInt32(text5);
                            break;
                        case "ExplosiveRadiusMultiplier":
                            ExplosiveRadiusMultiplier = float.Parse(text5);
                            ExplosivePowerup.RadiusMultiplier = ExplosiveRadiusMultiplier;
                            break;
                        case "ExplosiveFreezingRadiusMultiplier":
                            ExplosiveFreezingRadiusMultiplier = float.Parse(text5);
                            ExplosivePowerup.FreezingRadiusMultiplier = ExplosiveFreezingRadiusMultiplier;
                            break;
                        case "ExplosiveMiniRadiusMultiplier":
                            ExplosiveMiniRadiusMultiplier = float.Parse(text5);
                            ExplosivePowerup.MiniRadiusMultiplier = ExplosiveMiniRadiusMultiplier;
                            break;
                        case "SuddenDeathTimeLimit":
                            SuddenDeathTimeLimit = float.Parse(text5);
                            break;
                        case "BoomerangBouncinessMultiplier":
                            BoomerangBouncinessMultiplier = float.Parse(text5);
                            break;
                        case "BoomerangBouncinessMultiplierTiedToPowerUp":
                            BoomerangBouncinessMultiplierTiedToPowerUp = (PowerupType)Convert.ToInt32(text5);
                            break;
                        case "TeamGoldenBoomerang":
                            TeamGoldenBoomerang = Convert.ToBoolean(text5);
                            break;
                        case "TeamGoldenBoomerangTimeLimit":
                            TeamGoldenBoomerangTimeLimit = float.Parse(text5);
                            break;
                        case "MatchScoreLimit":
                            MatchScoreLimit = int.Parse(text5);
                            break;
                        case "CameraFlip":
                            CameraFlip = int.Parse(text5);
                            break;
                        case "CameraFlipRandom":
                            CameraFlipRandom = Convert.ToBoolean(text5);
                            break;
                        case "BoomerangSize":
                            BoomerangSize = float.Parse(text5);
                            break;
                        case "SurviveTillDawnTimer":
                            SurviveTillDawnTimer = float.Parse(text5);
                            break;
                        case "SurviveTillDawnHunterPowerups":
                            SurviveTillDawnHunterPowerups = (PowerupType)Convert.ToInt32(text5);
                            break;
                        case "SurviveTillDawnOthersPowerups":
                            SurviveTillDawnOthersPowerups = (PowerupType)Convert.ToInt32(text5);
                            break;
                        case "SurviveTillDawnKillGainTime":
                            SurviveTillDawnKillGainTime = float.Parse(text5);
                            break;
                        case "SurviveTillDawnRespawning":
                            SurviveTillDawnRespawning = Convert.ToBoolean(text5);
                            break;
                        case "LevelPicker":
                            LevelPicker = (from i in text5.Replace("[", string.Empty).Replace("]", string.Empty).Split(',')
                                           select int.Parse(i)).ToArray();
                            break;
                        case "ReviveAsDecoy":
                            ReviveAsDecoy = Convert.ToBoolean(text5);
                            DecoyPowerup.ReviveAsDecoy = ReviveAsDecoy;
                            break;
                    }
                }
                catch
                {
                }
            }
        }

        private static void SaveSettings()
        {
            string contents = string.Format("//in case of any questions - discord: {0}\r\n//version {1} {2}\r\n\r\n\r\n//Power up codes:\r\n//2 - Caffeinated; 4 - DashThroughWalls; 8 - Bamboozled; 16 - Shield; 32 - Explosive; 64 - Multi; 128 - Teleport; 256 - Fire; 512 - Extra; 1024 - Freeze; 2048 - Disguise; 8192 - Telekinesis; 32768 - Decoy\r\n\r\n//0 - Standard\r\n//1 - Power Drain mode - no spawning of power ups; you kill, you gain a power up; you die, you lose power up (1 or all)\r\n//2 - RamboHulk mode - powered up player vs the rest\r\n//3 - Survive Till Dawn mode - one player with boomerang, others without; after certain time, the boomerang switches to a different player\r\n{3}={4}\r\n\r\n[Power Drain options]\r\n{5}={6}\r\n\r\n[RamboHulk options]\r\n{7}={8}\r\n{9}={10}\r\n{11}={12}//flag values - add up the power up codes, which you want to assign\r\n{13}={14}//flag values\r\n\r\n[SurviveTillDawn options]\r\n{15}={16}\r\n{17}={18}\r\n{19}={20}//flag values - add up the power up codes, which you want to assign\r\n{21}={22}//flag values\r\n{23}={24}\r\n\r\n[Game Options]\r\n{25}={26}\r\n{27}={28}\r\n{29}={30}//flag values\r\n{31}={32}\r\n{33}={34}// 0 - default; otherwise the value is in seconds\r\n{35}={36}// 0 - default by match length\r\n{37}={38}\r\n{39}={40}\r\n{41}={42}//0-default\r\n{43}=[]//pick by level ids, i.e. [1,2,3,3,3], max level for base game is id=35, keep empty for regular rotation\r\n\r\n[CameraOptions]\r\n{44}={45}// values 0-3\r\n{46}={47}\r\n\r\n[Powerup modifiers]\r\n{48}={49}\r\n{50}=null //into how many should be split; null - default\r\n{51}={52}\r\n{53}={54} //when killed, replace existing decoy\r\n{55}={56}\r\n{57}={58}\r\n{59}={60}\r\n{61}={62}\r\n{63}={64}\r\n{65}={66}\r\n{67}={68}\r\n{69}={70}\r\n{71}={72} //pick to which power up should the flying be tied - enter code from the list above\r\n{73}={74}\r\n{75}={76}\r\n{77}={78}\r\n{79}={80}\r\n{81}={82}\r\n{83}={84}\r\n{85}={86}\r\n{87}={88}// larger the value, more it will bounce\r\n{89}={90}// 0 - for all; if power up code, then only for specific powerup\r\n\r\n[Experimantal]\r\n{91}={92}// has bugs\r\n", ModAuthor, ModVersion, ModReleaseDate, "GameMode", (int)GameMode, "PowerDrainLoseAll", PowerDrainLoseAll, "RamboHulkKeepSwapping", RamboHulkKeepSwapping, "RamboHulkEnableRevive", RamboHulkEnableRevive, "RamboHulkPowerups", (int)RamboHulkPowerups, "RamboHulkOthersPowerups", (int)RamboHulkOthersPowerups, "SurviveTillDawnTimer", SurviveTillDawnTimer, "SurviveTillDawnKillGainTime", SurviveTillDawnKillGainTime, "SurviveTillDawnHunterPowerups", (int)SurviveTillDawnHunterPowerups, "SurviveTillDawnOthersPowerups", (int)SurviveTillDawnOthersPowerups, "SurviveTillDawnRespawning", SurviveTillDawnRespawning, "MaxPowerUps", MaxPowerUps, "EnableDuplicatedCharacters", EnableDuplicatedCharacters, "StartupPowerUps", (int)StartupPowerUps, "RapidPowerUpSpawning", RapidPowerUpSpawning, "SuddenDeathTimeLimit", SuddenDeathTimeLimit, "MatchScoreLimit", MatchScoreLimit, "TeamGoldenBoomerang", TeamGoldenBoomerang, "TeamGoldenBoomerangTimeLimit", TeamGoldenBoomerangTimeLimit, "BoomerangSize", BoomerangSize, "LevelPicker", "CameraFlip", CameraFlip, "CameraFlipRandom", CameraFlipRandom, "ShieldCounter", ShieldCounter, "MultiBumerangSplit", "MaxDecoyCount", MaxDecoyCount, "ReviveAsDecoy", ReviveAsDecoy, "MoveFasterAttackCooldown", MoveFasterAttackCooldown, "MoveFasterDashForceMultiplier", MoveFasterDashForceMultiplier, "MoveFasterDashDurationMultiplier", MoveFasterDashDurationMultiplier, "MoveFasterDashCooldownMultiplier", MoveFasterDashCooldownMultiplier, "MoveFasterMoveSpeedMultiplier", MoveFasterMoveSpeedMultiplier, "MoveFasterTurnSpeed", MoveFasterTurnSpeed, "ReverseInputsImmunity", ReverseInputsImmunity, "ReverseInputsDuration", ReverseInputsDuration, "FlyingPowerUp", (int)FlyingPowerUp, "FlyingDuration", FlyingDuration, "FirePowerBurnDuration", FirePowerBurnDuration, "FirePowerImmunityToFire", FirePowerImmunityToFire, "ShrinkingBoundsFinalTimer", ShrinkingBoundsFinalTimer, "ExplosiveRadiusMultiplier", ExplosiveRadiusMultiplier, "ExplosiveFreezingRadiusMultiplier", ExplosiveFreezingRadiusMultiplier, "ExplosiveMiniRadiusMultiplier", ExplosiveMiniRadiusMultiplier, "BoomerangBouncinessMultiplier", BoomerangBouncinessMultiplier, "BoomerangBouncinessMultiplierTiedToPowerUp", (int)BoomerangBouncinessMultiplierTiedToPowerUp, "ShrinkingBoundsKicksOnly", ShrinkingBoundsKicksOnly);
            File.WriteAllText(FileLocation, contents);
        }
    }

}