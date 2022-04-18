using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponClassScriptableObject", menuName = "ScriptableObjects/WeaponClass", order = 0)]
public class WeaponClassScriptableObject : ScriptableObject
{
    [SerializeField] private WeaponType weaponType;     
    [SerializeField] private WeaponFireMode weaponFireMode;
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public WeaponFireMode WeaponFireMode { get => weaponFireMode; set => weaponFireMode = value; }
}
