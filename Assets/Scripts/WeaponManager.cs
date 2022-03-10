using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Unarmed, AssaultRifle, Pistol }

public class WeaponManager : MonoBehaviour
{
    private Weapon weaponScript;
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private GameObject[] _assaultRiflesPrefab;
    [SerializeField] private GameObject[] _pistolsPrefab;
    private GameObject _currentWeaponInstance = null;

    #region Singleton
    public static WeaponManager instance = null;

    private void Awake()
    {
        if (instance == null)        
            instance = this;        
        else if (instance != this)
            Destroy(gameObject);
    }
    #endregion

    [SerializeField] private WeaponScriptableObject _currentWeapon = null;    
    /*[SerializeField]*/ /*private Animator weaponAnim;*/
    RigidBodyFPSController FPSController;
    private int currDMG;
    private int currMagAmmo;
    private int currAvailableAmmo;    

    //[SerializeField] public RuntimeAnimatorController[] animators;

    [SerializeField] private WeaponType _currentWeaponType;

    public WeaponType CurrentWeaponType { get => _currentWeaponType; set => _currentWeaponType = value; }

    ////Static variables
    //public static int stcWeaponMinDMG;
    //public static int stcWeaponMaxDMG;

    // Start is called before the first frame update
    void Start()
    {
        CurrentWeaponType = WeaponType.Unarmed;        
        WeaponSpwanByClass(CurrentWeaponType);
        
        if (CurrentWeaponType != WeaponType.Unarmed)
        {
            currDMG = Mathf.Clamp(currDMG, _currentWeapon.WeaponMinDMG, _currentWeapon.WeaponMaxDMG);
            currMagAmmo = _currentWeapon.WeaponMagazineAmmo;
            currAvailableAmmo = _currentWeapon.WeaponMaxAmmo;
        }        
        
        //_currentWeaponObject.transform.parent = transform;
        //_currentWeaponObject = _currentWeapon.WeaponPrefab;
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
    }

    void NewWeapon(WeaponType weaponType)
    {
        Destroy(_currentWeaponInstance);

        if (weaponType == WeaponType.Unarmed)
        {
            Destroy(_currentWeaponInstance);

            _currentWeaponInstance = null;
            _currentWeapon = null;
        }
        if (weaponType == WeaponType.Pistol)
        {
            Destroy(_currentWeaponInstance);

            _currentWeaponInstance = Instantiate(
                                        _pistolsPrefab[0], 
                                        weaponSocket.position, 
                                        Quaternion.identity, 
                                        GameObject.Find("-- WeaponManager --").transform
                                     );

            weaponScript = GetComponentInChildren<Weapon>();
            _currentWeapon = weaponScript._weapon;
            CurrentWeaponType = _currentWeapon.WeaponType;
        }
        if (weaponType == WeaponType.AssaultRifle)
        {
            Destroy(_currentWeaponInstance);

            _currentWeaponInstance = Instantiate(
                                        _assaultRiflesPrefab[0],
                                        weaponSocket.position, 
                                        Quaternion.identity, 
                                        GameObject.Find("-- WeaponManager --").transform
                                     );

            weaponScript = GetComponentInChildren<Weapon>();
            _currentWeapon = weaponScript._weapon;
            CurrentWeaponType = _currentWeapon.WeaponType;
        }
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

    void WeaponSpwanByClass(WeaponType weaponClass) 
    {
        Destroy(_currentWeaponInstance);

        if (weaponClass is WeaponType.Unarmed)
        {
            //Destroy(_currentWeaponInstance);

            _currentWeaponInstance = null;
            _currentWeapon = null;

            NewWeapon(weaponClass);
            Debug.Log("Unarmed");
        }
        if (weaponClass is WeaponType.AssaultRifle)
        {
            //Destroy(_currentWeaponInstance);

            _currentWeaponInstance = Instantiate(
                                        _assaultRiflesPrefab[0],
                                        weaponSocket.position,
                                        Quaternion.identity,
                                        GameObject.Find("-- WeaponManager --").transform
                                     );

            weaponScript = GetComponentInChildren<Weapon>();
            _currentWeapon = weaponScript._weapon;
            CurrentWeaponType = _currentWeapon.WeaponType;            
            Debug.Log("Assault Rifle");
        }
        if (weaponClass is WeaponType.Pistol)
        {
            //Destroy(_currentWeaponInstance);

            _currentWeaponInstance = Instantiate(
                                        _pistolsPrefab[0],
                                        weaponSocket.position,
                                        Quaternion.identity,
                                        GameObject.Find("-- WeaponManager --").transform
                                     );

            weaponScript = GetComponentInChildren<Weapon>();
            _currentWeapon = weaponScript._weapon;
            CurrentWeaponType = _currentWeapon.WeaponType;            
            Debug.Log("Pistol");
        }
        //else
        //    Debug.Log("Unarmed");

        //switch (weaponClass)
        //{
        //    case WeaponType.Unarmed:
                   
        //        break;
        //    case WeaponType.AssaultRifle:
                
        //        break;
        //    case WeaponType.Pistol:
                
        //        break;
        //    default:
                      
        //        break;
        //}        
    }

    // Update is called once per frame
    void Update()
    {
        //CurrentWeaponType = _currentWeapon.WeaponType;

        //WeaponClass(CurrentWeaponType);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Destroy(_currentWeaponInstance);
            CurrentWeaponType = WeaponType.Unarmed;
            //NewWeapon();            
            WeaponSpwanByClass(CurrentWeaponType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Destroy(_currentWeaponInstance);
            CurrentWeaponType = WeaponType.AssaultRifle;
            //NewWeapon(CurrentWeaponType);
            //NewWeapon();
            //_currentWeapon = weaponScript._weapon;
            //_currentWeaponObject = _currentWeapon.WeaponPrefab;
            //CurrentWeaponType = _currentWeapon.WeaponType;
            //_currentWeapon = Resources.Load<WeaponScriptableObject>("Scripts/ScriptableObjects/Weapons/M416");
            WeaponSpwanByClass(CurrentWeaponType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Destroy(_currentWeaponInstance);
            CurrentWeaponType = WeaponType.Pistol;
            //NewWeapon(CurrentWeaponType);
            //NewWeapon();
            //_currentWeapon = weaponScript._weapon;
            //CurrentWeaponType = _currentWeapon.WeaponType;
            //_currentWeapon = Resources.Load<WeaponScriptableObject>("Scripts/ScriptableObjects/Weapons/M9_Baretta");
            WeaponSpwanByClass(CurrentWeaponType);
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
