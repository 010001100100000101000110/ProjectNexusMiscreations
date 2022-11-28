using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]

public class SOPlayerSettings : ScriptableObject
{
    public float PlayerSpeed;
    public float PlayerSpeedInCrouch;
    public float PlayerSpeedInAir;
    public float PlayerSpeedInAttackState;
    public float JumpForce;
    public float DashDurationInSeconds;
    public float DashForce;
    public float DashPowerConsumption;
    public float DashCooldown;
    public float PlayerTurningSpeed;

    public float MaxHealth;
    public float MaxPower;

    public float PowerItemSSize;
    public float PowerItemMSize;
    public float PowerItemLSize;

    public float Attack1DamageDelay;
    public float Attack2DamageDelay;
    public float Attack3DamageDelay;

    public float Attack1GruntDelay;
    public float Attack2GruntDelay;
    public float Attack3GruntDelay;
}
