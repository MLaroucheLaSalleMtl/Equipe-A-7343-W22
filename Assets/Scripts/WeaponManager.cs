using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { AssaultRifle, Pistol, Unarmed }

public class WeaponManager : MonoBehaviour
{
    #region Singleton
    public static WeaponManager instance = null;  

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] private WeaponScriptableObject weaponScriptableObject;
    [SerializeField] private WeaponClassScriptableObject weaponClassScriptableObject;
    /*[SerializeField]*/ /*private Animator weaponAnim;*/
    RigidBodyFPSController FPSController;
    private int currDMG;
    private int currMagAmmo;
    private int currAvailableAmmo;

    [SerializeField] public GameObject[] Weapons;
    //[SerializeField] public RuntimeAnimatorController[] animators;
    
    //private WeaponType weaponType;

    ////Static variables
    //public static int stcWeaponMinDMG;
    //public static int stcWeaponMaxDMG;       

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < animators.Length; i++)
        //{
        //    animators[i] = FPSController.anim.runtimeAnimatorController;
        //}
        //animators[0] = FPSController.anim.runtimeAnimatorController;
        //animators[1] = FPSController.anim.runtimeAnimatorController;
        //animators[2] = FPSController.anim.runtimeAnimatorController;
        //weaponType = weaponClassScriptableObject.WeaponType;
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

    //private Vector3 SpawnWeapon() 
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        //switch (weaponClassScriptableObject.WeaponType)
        //{
        //    case WeaponType.AssaultRifle:
        //        weaponClassScriptableObject.Current_State = AssaultRifle_State.GetInstance();
        //        //FPSController.anim.runtimeAnimatorController = animators[1];
        //        //Debug.Log(weaponClassScriptableObject.Current_State.ToString());
        //        break;
        //    case WeaponType.Pistol:
        //        weaponClassScriptableObject.Current_State = Pistol_State.GetInstance();
        //        //FPSController.anim.runtimeAnimatorController = animators[2];
        //        //Debug.Log(weaponClassScriptableObject.Current_State.ToString());
        //        break;
        //    case WeaponType.UnArmed:
        //        weaponClassScriptableObject.Current_State = Normal_State.GetInstance();
        //        //FPSController.anim.runtimeAnimatorController = animators[0];
        //        //Debug.Log(weaponClassScriptableObject.Current_State.ToString());
        //        break;
        //    //default:
        //    //    break;
        //}

        //if (weaponClassScriptableObject.Current_State is Normal_State)
        //{
        //}
        //else if (weaponClassScriptableObject.Current_State is AssaultRifle_State)
        //{
        //}
        //else if (weaponClassScriptableObject.Current_State is Pistol_State)
        //{
            
        //}

        //if (true)
        //{
        //    if (FPSController.fireTrigger || FPSController.fireBool || FPSController.isFiring)
        //    {

        //    }            
        //}
    }
}
