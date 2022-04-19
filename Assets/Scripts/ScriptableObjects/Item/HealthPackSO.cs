using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/HealthPackPickableItems")]
public class HealthPackSO : ScriptableObject
{
    public static HealthPackSO instance;
    RigidBodyFPSController FPSController;
    float _healthCapacity = 50;

    public void OnCollect()
    {
        FPSController.playerStatsSO.PlayerHealth += _healthCapacity;
    }
}
