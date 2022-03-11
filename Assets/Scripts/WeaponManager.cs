using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Unarmed, AssaultRifle, Pistol }

public class WeaponManager : MonoBehaviour
{
    private Weapon weaponScript = null;
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

    /*[SerializeField] */public WeaponScriptableObject _currentWeapon = null;    
    
    //[SerializeField] public RuntimeAnimatorController[] animators;

    [SerializeField] private WeaponType _currentWeaponType;

    public WeaponType CurrentWeaponType { get => _currentWeaponType; set => _currentWeaponType = value; }    

    // Start is called before the first frame update
    void Start()
    {
        weaponScript = Weapon.instance;
        CurrentWeaponType = WeaponType.Unarmed;        
        WeaponSpwanByClass(CurrentWeaponType);
    }     

    void WeaponSpwanByClass(WeaponType weaponClass) 
    {
        Destroy(_currentWeaponInstance);

        if (weaponClass is WeaponType.Unarmed)
        {
            Destroy(_currentWeaponInstance);

            _currentWeaponInstance = null;
            _currentWeapon = null;

            //WeaponSpwanByClass(weaponClass);
            Debug.Log("Unarmed");
        }
        if (weaponClass is WeaponType.AssaultRifle)
        {
            Destroy(_currentWeaponInstance);

            _currentWeaponInstance = Instantiate(
                                        _assaultRiflesPrefab[0],
                                        //weaponSocket.position,
                                        //Quaternion.identity,
                                        GameObject.Find("WeaponLocation").transform
                                     );

            weaponScript = FindObjectOfType<Weapon>();
            _currentWeapon = weaponScript._weapon;
            CurrentWeaponType = _currentWeapon.WeaponType;            
            Debug.Log("Assault Rifle");
        }
        if (weaponClass is WeaponType.Pistol)
        {
            Destroy(_currentWeaponInstance);

            _currentWeaponInstance = Instantiate(
                                        _pistolsPrefab[0],
                                        //weaponSocket.position,
                                        //Quaternion.identity,
                                        GameObject.Find("WeaponLocation").transform
                                     );

            weaponScript = FindObjectOfType<Weapon>();
            _currentWeapon = weaponScript._weapon;
            CurrentWeaponType = _currentWeapon.WeaponType;            
            Debug.Log("Pistol");
        }           
    }

    // Update is called once per frame
    void Update()
    {
        //CurrentWeaponType = _currentWeapon.WeaponType;

        //WeaponClass(CurrentWeaponType);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(_currentWeaponInstance);
            CurrentWeaponType = WeaponType.Unarmed;
            //NewWeapon();            
            WeaponSpwanByClass(CurrentWeaponType);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(_currentWeaponInstance);
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
            Destroy(_currentWeaponInstance);
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
