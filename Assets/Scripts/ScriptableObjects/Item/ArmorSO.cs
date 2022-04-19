using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/ArmorPickableItems")]
public class ArmorSO : ScriptableObject
{
    public static ArmorSO instance;
    PlayerStatsSO playerStatsSO;
    float _armorCapacity = 50;

    public void OnCollect()
    {
        playerStatsSO.PlayerArmor += _armorCapacity;
    }
}
