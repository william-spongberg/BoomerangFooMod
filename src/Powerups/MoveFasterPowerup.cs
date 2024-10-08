using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace BoomerangFoo.Powerups
{
    class MoveFasterPowerup
    {
        public static PowerupType PowerupBitmask = PowerupType.MoveFaster;

        public static float AttackCooldown = 0.8f;
        public static float DashForceMultiplier = 1.5f;
        public static float DashDurationMultiplier = 0.1f;
        public static float DashCooldownMultiplier = 0.4f;
        public static float MoveSpeedMultiplier = 1.9f;
        public static float TurnSpeed = 24f;

        public static readonly FieldInfo playerAttackCooldownDuration = typeof(Player).GetField("attackCooldownDuration", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalAttackCooldownDuration = 0.66f;
        public static readonly FieldInfo playerLungeCooldownDuration = typeof(Player).GetField("lungeCooldownDuration", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalLungeCooldownDuration = 0.5f;
        public static readonly FieldInfo playerDashSpeed = typeof(Player).GetField("dashSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalDashSpeed = 660f;
        public static readonly FieldInfo playerDashCooldown = typeof(Player).GetField("dashCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalDashCooldown = 0.55f;
        public static readonly FieldInfo playerDashDuration = typeof(Player).GetField("dashDuration", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalDashDuration = 0.55f;
        public static readonly FieldInfo playerTurningSpeed = typeof(Player).GetField("turningSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalTurningSpeed = 12f;
        public static readonly FieldInfo playerWalkSpeed = typeof(Player).GetField("walkSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalWalkSpeed = 200f;
        public static readonly FieldInfo playerWalkSpeedUnarmed = typeof(Player).GetField("walkSpeedUnarmed", BindingFlags.NonPublic | BindingFlags.Instance);
        public static float originalWalkSpeedUnarmed = 230f;

        public static void Register()
        {
            PowerupManager.OnAcquirePowerup += AcquirePowerup;
            PowerupManager.OnRemovePowerup += RemovePowerup;
        }

        public static void AcquirePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup)
        {
            var previousActive = CommonFunctions.PreviousActive(powerupHistory);

            if (!newPowerup.HasPowerup(PowerupBitmask) || previousActive.HasPowerup(PowerupBitmask)) return;

            PlayerState playerState = CommonFunctions.GetPlayerState(player);

            playerState.originalAttackCooldownDuration = (float)playerAttackCooldownDuration.GetValue(player);
            playerAttackCooldownDuration.SetValue(player, playerState.moveFasterAttackCooldown);
            playerState.originalLungeCooldownDuration = (float)playerLungeCooldownDuration.GetValue(player);
            playerLungeCooldownDuration.SetValue(player, playerState.moveFasterAttackCooldown);
            playerState.originalDashSpeed = (float)playerDashSpeed.GetValue(player);
            playerDashSpeed.SetValue(player, (float)playerDashSpeed.GetValue(player) * playerState.moveFasterDashForceMultiplier);
            playerState.originalDashCooldown = (float)playerDashCooldown.GetValue(player);
            playerDashCooldown.SetValue(player, (float)playerDashCooldown.GetValue(player) * playerState.moveFasterDashCooldownMultiplier);
            playerState.originalDashDuration = (float)playerDashDuration.GetValue(player);
            playerDashDuration.SetValue(player, (float)playerDashDuration.GetValue(player) * playerState.moveFasterDashDurationMultiplier);
            playerState.originalTurningSpeed = (float)playerTurningSpeed.GetValue(player);
            playerTurningSpeed.SetValue(player, playerState.moveFasterTurnSpeed);
            playerState.originalWalkSpeed = (float)playerWalkSpeed.GetValue(player);
            playerWalkSpeed.SetValue(player, (float)playerWalkSpeed.GetValue(player) * playerState.moveFasterMoveSpeedMultiplier);
            playerState.originalWalkSpeedUnarmed = (float)playerWalkSpeedUnarmed.GetValue(player);
            playerWalkSpeedUnarmed.SetValue(player, (float)playerWalkSpeedUnarmed.GetValue(player) * playerState.moveFasterMoveSpeedMultiplier);
        }

        public static void RemovePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup)
        {
            if (!newPowerup.HasPowerup(PowerupBitmask) || !player.activePowerup.HasPowerup(PowerupBitmask)) return;

            PlayerState playerState = CommonFunctions.GetPlayerState(player);

            playerAttackCooldownDuration.SetValue(player, playerState.originalAttackCooldownDuration);
            playerLungeCooldownDuration.SetValue(player, playerState.originalAttackCooldownDuration);
            playerDashSpeed.SetValue(player, playerState.originalDashSpeed);
            playerDashCooldown.SetValue(player, playerState.originalDashCooldown);
            playerDashDuration.SetValue(player, playerState.originalDashDuration);
            playerTurningSpeed.SetValue(player, playerState.originalTurningSpeed);
            playerWalkSpeed.SetValue(player, playerState.originalWalkSpeed);
            playerWalkSpeedUnarmed.SetValue(player, playerState.originalWalkSpeedUnarmed);
        }
    }
}
