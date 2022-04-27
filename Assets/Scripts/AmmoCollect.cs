using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollect : MonoBehaviour
{
    RigidBodyFPSController FPSController;
    [SerializeField] WeaponShoot weaponShoot;
    int _ammoPackCapacity;

    private void Start()
    {
        //weaponShoot = FindObjectOfType<RigidBodyFPSController>().GetComponentInChildren<WeaponManager>().weaponShoot;
        //if (WeaponManager.instance.CurrentWeaponType != WeaponType.Unarmed)
        //{
        //    _ammoPackCapacity = weaponShoot._weapon.WeaponSO.WeaponMaxAmmo;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RigidBodyFPSController>())
        {
            Collect();            
        }
    }

    public void Collect()
    {
        weaponShoot = FindObjectOfType<RigidBodyFPSController>().GetComponentInChildren<WeaponManager>().weaponShoot;

        if (weaponShoot != null)
        {
            _ammoPackCapacity = weaponShoot._weapon.WeaponSO.WeaponMagazineAmmo;
            if (weaponShoot.currAvailableAmmo < weaponShoot._weapon.WeaponSO.WeaponMaxAmmo * 2)
            {
                weaponShoot.currAvailableAmmo += _ammoPackCapacity;
                Destroy(this.gameObject);
            }
            else if (weaponShoot.currAvailableAmmo >= weaponShoot._weapon.WeaponSO.WeaponMaxAmmo)
            {
                print("Max Ammo Capacity Reached");
            }
        }       

        PlayerUIManager.munitionUpdate?.Invoke();
    }
}
