using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollect : MonoBehaviour
{
    //public delegate void OnAmmoCollect();
    //public static OnAmmoCollect onAmmoCollect;

    //[SerializeField] private AmmoSO ammoSO;    

    WeaponShoot weaponShoot;
    int _ammoPackCapacity = 50;

    private void Awake()
    {
        weaponShoot = FindObjectOfType<WeaponShoot>();
    }

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
        Destroy(this.gameObject);
    }
}
