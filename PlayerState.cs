using BoomerangFoo.Powerups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoomerangFoo
{
    public class PlayerState : MonoBehaviour
    {
        // Shield
        public int shieldHits = ShieldPowerup.ShieldHits;

        // Multiboomerang
        public int multiBoomerangSplit = MultiboomerangPowerup.MultiBoomerangSplit;

        // Decoy
        public int maxDecoyCount = DecoyPowerup.MaxDecoyCount;
        public bool reviveAsDecoy = DecoyPowerup.ReviveAsDecoy;

        public int decoyCounter = 0;

        // MoveFaster
        public float moveFasterAttackCooldown = MoveFasterPowerup.AttackCooldown;
        public float moveFasterDashForceMultiplier = MoveFasterPowerup.DashForceMultiplier;
        public float moveFasterDashDurationMultiplier = MoveFasterPowerup.DashDurationMultiplier;
        public float moveFasterDashCooldownMultiplier = MoveFasterPowerup.DashCooldownMultiplier;
        public float moveFasterMoveSpeedMultiplier = MoveFasterPowerup.MoveSpeedMultiplier;
        public float moveFasterTurnSpeed = MoveFasterPowerup.TurnSpeed;

        public float originalAttackCooldownDuration = 0.66f;
        public float originalLungeCooldownDuration = 0.5f;
        public float originalDashSpeed = 660f;
        public float originalDashCooldown = 0.55f;
        public float originalDashDuration = 0.55f;
        public float originalTurningSpeed = 12f;
        public float originalWalkSpeed = 200f;
        public float originalWalkSpeedUnarmed = 230f;

        // Bamboozle
        public bool reverseInputsImmunity = BamboozlePowerup.Immunity;
        public float reverseInputsDuration = BamboozlePowerup.Duration;

        // Flying
        public float flyingDuration = FlyingPowerup.Duration;
        public bool resetOnGround = true;

        public float flyingTimeLimit;
        public float flyingTimer;
        public bool isFlyingTimerOn;
        public bool isFlying;
        public bool flyingForceFall;

        // Explosive
        public float explosiveRadiusMultiplier = 1f;
    }
}
