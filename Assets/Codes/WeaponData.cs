using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/WeaponData")]
public class WeaponData : ScriptableObject
{

    public enum ItemType
    {
        Component,
    };

    [Header("# Main Info ")]
    public ItemType itemType;

    [Header("# Level Info ")]
    public float basePower;
    public float baseSpeed;
    public float baseIntervalTime;
    public float[] powers;
    public float[] speeds;
    public float[] intervalTimes;

    [Header("# Weapon ")]
    public GameObject projectile;

}
