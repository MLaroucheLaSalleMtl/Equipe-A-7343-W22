using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponScriptableObject;
    RigidBodyFPSController FPSController;
    private int currDMG;
    private int currMagAmmo;
    private int currAvailableAmmo;

    ////Static variables
    //public static int stcWeaponMinDMG;
    //public static int stcWeaponMaxDMG;       

    // Start is called before the first frame update
    void Start()
    {
        //weaponScriptableObjet = ScriptableObject.CreateInstance<WeaponScriptableObject>();
        currDMG = Mathf.Clamp(currDMG, weaponScriptableObject.weaponMinDMG, weaponScriptableObject.weaponMaxDMG);
        currMagAmmo = weaponScriptableObject.weaponMagazineAmmo;
        currAvailableAmmo = weaponScriptableObject.weaponMaxAmmo;
    }

    public void WeaponFire()
    {
        //currDMG = new RNG().GetInstance().Next();
        currMagAmmo--;

        Debug.Log("Current DMG : " + currDMG + 
                  " , Current Mag Ammo : " + currMagAmmo + 
                  " , Current Ammo : " + currAvailableAmmo);
    }

    public void WeaponReload()
    {
        currAvailableAmmo -= currMagAmmo;
        currMagAmmo = weaponScriptableObject.weaponMagazineAmmo;
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
