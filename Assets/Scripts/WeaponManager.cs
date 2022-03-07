using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponScriptableObjet;
    RigidBodyFPSController FPSController;
    private int currDMG;
    private int currMagAmmo;
    private int currAvailableAmmo;

    // Start is called before the first frame update
    void Start()
    {
        weaponScriptableObjet = GetComponent<WeaponScriptableObject>();
        currDMG = weaponScriptableObjet.weaponDMG;
        currMagAmmo = weaponScriptableObjet.weaponMagazineAmmo;
        currAvailableAmmo = weaponScriptableObjet.weaponMaxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        //if (true)
        //{
        //    if (FPSController.fireTrigger || FPSController.fireBool || FPSController.isFiring)
        //    {
                  
        //    }            
        //}
    }
}
