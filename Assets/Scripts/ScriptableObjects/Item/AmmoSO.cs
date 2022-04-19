using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/AmmoPickableItems")]
public class AmmoSO : ScriptableObject
{    
    WeaponShoot weaponShoot;
    int _ammoPackCapacity = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        weaponShoot.currAvailableAmmo += _ammoPackCapacity;
    }
}
