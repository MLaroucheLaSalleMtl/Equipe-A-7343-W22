using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Unarmed, AssaultRifle, Pistol }

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

    [SerializeField] private WeaponScriptableObject _currentWeapon;
    //[SerializeField] private WeaponClassScriptableObject _currentWeaponClass;
    /*[SerializeField]*/ /*private Animator weaponAnim;*/
    RigidBodyFPSController FPSController;
    private int currDMG;
    private int currMagAmmo;
    private int currAvailableAmmo;

    public GameObject[] Weapons;
    //[SerializeField] public RuntimeAnimatorController[] animators;

    [SerializeField] private WeaponType _currWeaponType;

    public WeaponType CurrWeaponType { get => _currWeaponType; set => _currWeaponType = value; }

    ////Static variables
    //public static int stcWeaponMinDMG;
    //public static int stcWeaponMaxDMG;

    // Start is called before the first frame update
    void Start()
    {
        _currentWeapon = GetComponentInChildren<WeaponScriptableObject>();
        CurrWeaponType = WeaponType.Unarmed;
        //weaponType = FindObjectOfType<WeaponTypeEnum>();
        //_currentWeapon = FindObjectOfType<WeaponScriptableObject>();
        //weaponType = _currentWeaponClass.weaponScriptableObject.weaponType;
        //_currentWeaponClass.weaponScriptableObject = _currentWeapon;
        //for (int i = 0; i < animators.Length; i++)
        //{
        //    animators[i] = FPSController.anim.runtimeAnimatorController;
        //}
        //animators[0] = FPSController.anim.runtimeAnimatorController;
        //animators[1] = FPSController.anim.runtimeAnimatorController;
        //animators[2] = FPSController.anim.runtimeAnimatorController;
        //weaponType = weaponClassScriptableObject.WeaponType;
        //weaponScriptableObjet = ScriptableObject.CreateInstance<WeaponScriptableObject>();
        currDMG = Mathf.Clamp(currDMG, _currentWeapon.WeaponMinDMG, _currentWeapon.WeaponMaxDMG);
        currMagAmmo = _currentWeapon.WeaponMagazineAmmo;
        currAvailableAmmo = _currentWeapon.WeaponMaxAmmo;
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
        currMagAmmo = _currentWeapon.WeaponMagazineAmmo;
    }

    //private Vector3 SpawnWeapon() 
    //{

    //}

    void WeaponClass(WeaponType weaponClass) 
    {
        switch (weaponClass)
        {
            case WeaponType.Unarmed:
                Debug.Log("Unarmed");        
                break;
            case WeaponType.AssaultRifle:
                Debug.Log("Assault Rifle");            
                break;
            case WeaponType.Pistol:
                Debug.Log("Pistol");
                break;
            default:
                Debug.Log("Unarmed");        
                break;
        }

        //if (weaponClass != WeaponType.Unarmed)
        //{
        //    if (weaponClass == WeaponType.AssaultRifle)            
        //        Debug.Log("Assault Rifle");            
        //    else if (weaponClass == WeaponType.Pistol)            
        //        Debug.Log("Pistol");            
        //}
        //else
        //{
        //    CurrWeaponType = WeaponType.Unarmed;
        //    Debug.Log("Unarmed");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //CurrWeaponType = _currentWeapon.WeaponType;

        WeaponClass(CurrWeaponType);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrWeaponType = WeaponType.Unarmed;
            WeaponClass(CurrWeaponType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrWeaponType = _currentWeapon.WeaponType;
            //_currentWeapon = Resources.Load<WeaponScriptableObject>("Scripts/ScriptableObjects/Weapons/M416");
            WeaponClass(CurrWeaponType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrWeaponType = _currentWeapon.WeaponType;
            //_currentWeapon = Resources.Load<WeaponScriptableObject>("Scripts/ScriptableObjects/Weapons/M9_Baretta");
            WeaponClass(CurrWeaponType);
        }
        //else 
        //{
        //    CurrWeaponType = WeaponType.Unarmed;
        //    WeaponClass(CurrWeaponType);
        //}

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
