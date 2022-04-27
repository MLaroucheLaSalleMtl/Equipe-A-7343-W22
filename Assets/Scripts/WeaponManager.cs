using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType { Unarmed, Pistol, AssaultRifle }
public enum WeaponFireMode { None, Auto, Semi_Auto, Both }

public class WeaponManager : MonoBehaviour
{
    public Animator ArmsAnim;
    private Weapon _weaponScript;
    public WeaponShoot weaponShoot;
    private RigidBodyFPSController FPSController;

    [SerializeField] private Transform weaponSocket;

    public GameObject defaultWeapon;
    public GameObject M9Weapon;
    public GameObject M416Weapon;

    [SerializeField] private GameObject[] _weaponPrefabs;

    public List<GameObject> weaponPickUpList = new List<GameObject>();
    public List<GameObject> keyItemPickUpList = new List<GameObject>();

    #region Singleton
    public static WeaponManager instance = null;

    private void Awake()
    {
        FPSController = GetComponentInParent<RigidBodyFPSController>();

        _weaponScript = GetComponentInChildren<Weapon>();
        ArmsAnim = GetComponentInChildren<Animator>();

        if (weaponPickUpList.Count > 0)
        {
            foreach (GameObject weapon in weaponPickUpList)
            {
                Instantiate(weapon, this.transform);
                weapon.SetActive(true);
                WeaponPrefabs = GameObject.FindGameObjectsWithTag("Weapon");                
            }
        }
    }
    #endregion

    [SerializeField] private KeyCode[] keys;
    [SerializeField] private Vector2 _mouseScrollWheel;
    public int weaponSelect;

    public IState _currentWeaponState;

    [SerializeField] public RuntimeAnimatorController[] animators;

    public WeaponScriptableObject _currentWeapon;   

    [SerializeField] private WeaponType _currentWeaponType;
    [SerializeField] private WeaponFireMode _currentWeaponFireMode;

    public WeaponType CurrentWeaponType { get => _currentWeaponType; set => _currentWeaponType = value; }
    public WeaponFireMode CurrentWeaponFireMode { get => _currentWeaponFireMode; set => _currentWeaponFireMode = value; }
    public GameObject[] WeaponPrefabs { get => _weaponPrefabs; set => _weaponPrefabs = value; }
    public IState CurrentWeaponState { get => _currentWeaponState; set => _currentWeaponState = value; }

    public void UpdateWeaponList()
    {
        for (int i = 0; i < WeaponPrefabs.Length; i++)
        {
            WeaponPrefabs = GameObject.FindGameObjectsWithTag("Weapon"); 
        }       
    }

    public void AddToList(GameObject wpPrefab)
    {
        weaponPickUpList.Add(wpPrefab);
        weaponPickUpList = new List<GameObject>();
        _weaponPrefabs = weaponPickUpList.ToArray();
    }

    void Start()
    {
        PlayerUIManager.munitionUpdate?.Invoke();

        CurrentWeaponType = WeaponType.Unarmed;
        WeaponSpawnByClass((int)CurrentWeaponType);
        GetCurrentWeaponState((int)CurrentWeaponType);
    }     

    private IState GetCurrentWeaponState(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                CurrentWeaponState = Normal_State.GetInstance();
                break;
            case 1:
                CurrentWeaponState = AssaultRifle_State.GetInstance();
                break;
        }

        Debug.Log(CurrentWeaponState.ToString());

        return CurrentWeaponState;
    }

    public void WeaponSpawnByClass(int weaponClass) 
    {
        for (int i = 0; i < _weaponPrefabs.Length; i++)
        {
            WeaponPrefabs[i].gameObject.SetActive(i == (int)weaponClass);            

            if (WeaponPrefabs[(int)weaponClass].activeInHierarchy)
            {
                _currentWeapon = GetComponentInChildren<Weapon>().WeaponSO;
                FPSController.currentPlayerState = _currentWeapon.weaponState;
                CurrentWeaponFireMode = _currentWeapon.WeaponFireMode;
                CurrentWeaponType = _currentWeapon.WeaponType;
                ArmsAnim = GetComponentInChildren<Animator>();

                if (WeaponPrefabs.Length > 1)
                {
                    if (WeaponPrefabs[(int)weaponClass] == WeaponPrefabs[1])
                    {
                        weaponShoot = WeaponPrefabs[(int)weaponClass].GetComponentInChildren<WeaponShoot>();

                        WeaponPrefabs[1].GetComponentInChildren<WeaponShoot>().enabled = true;
                    }
                    else
                    {
                        WeaponPrefabs[1].GetComponentInChildren<WeaponShoot>().enabled = false;

                        weaponShoot = null;
                    }
                }
                //if (WeaponPrefabs[(int)weaponClass] == WeaponPrefabs[1])
                //{
                //    weaponShoot = WeaponPrefabs[(int)weaponClass].GetComponentInChildren<WeaponShoot>();
                    
                //    WeaponPrefabs[1].GetComponentInChildren<WeaponShoot>().enabled = true;                    
                //}
                //else
                //{
                //    WeaponPrefabs[1].GetComponentInChildren<WeaponShoot>().enabled = false;

                //    weaponShoot = null;
                //}

                if (ArmsAnim.runtimeAnimatorController != animators[0])
                {
                    ArmsAnim.Play(_currentWeapon.ArmsRaiseAnim.name);
                }  
            }
        }    
    }

    public void OnMouseWeaponChange(InputAction.CallbackContext context)
    {
        _mouseScrollWheel.y = context.ReadValue<float>();

        Debug.Log("Scroll Wheel Y Axis Value : " + _mouseScrollWheel.y);

        int previousWeaponSelect = weaponSelect;

        _mouseScrollWheel.y = Mathf.Clamp(_mouseScrollWheel.y, -120f, 120f);

        if (_weaponPrefabs.Length > 1)
        {
            if (_mouseScrollWheel.y > 0f)
            {               
                FPSController.IsAiming = false;
                ArmsAnim = GetComponentInChildren<Animator>();
                
                weaponSelect = Mathf.Clamp(weaponSelect, 0, _weaponPrefabs.Length - 2);

                weaponSelect++;

                GetCurrentWeaponState(weaponSelect);

                if (previousWeaponSelect != weaponSelect)
                {
                    if (weaponShoot != null)
                    {
                        WeaponPrefabs[1].GetComponentInChildren<WeaponShoot>().enabled = false;
                        WeaponPrefabs[1].GetComponentInChildren<WeaponShoot>().StopAllCoroutines();
                        WeaponSpawnByClass(weaponSelect);     
                    }
                    else
                        WeaponSpawnByClass(weaponSelect);
                }
                PlayerUIManager.munitionUpdate?.Invoke();
            }

            if (_mouseScrollWheel.y < 0f)
            {
                FPSController.IsAiming = false;
                ArmsAnim = GetComponentInChildren<Animator>();               

                if(weaponSelect > 0)
                    weaponSelect--;

                GetCurrentWeaponState(weaponSelect);
                
                if (previousWeaponSelect != weaponSelect)
                {
                    if (weaponShoot != null)
                    {
                        WeaponPrefabs[1].GetComponentInChildren<WeaponShoot>().enabled = false;

                        WeaponSpawnByClass(weaponSelect);                        
                    }
                    else
                        WeaponSpawnByClass(weaponSelect);
                }
                PlayerUIManager.munitionUpdate?.Invoke();
            }
        }    
    }

    public void OnKeyboardWeaponChange()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            int previousWeaponSelect = weaponSelect;

            if (Input.GetKeyDown(keys[i]))
            {
                FPSController.IsAiming = false;
                ArmsAnim = GetComponentInChildren<Animator>();
                //if (ArmsAnim.runtimeAnimatorController != animators[0])
                //{
                //    ArmsAnim.Play(_currentWeapon.ArmsLowerAnim.name);
                //}
                weaponSelect = i;
            }

            if (previousWeaponSelect != weaponSelect)
                WeaponSpawnByClass(weaponSelect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (WeaponPrefabs.Length > 2)
        //{
        //OnKeyboardWeaponChange();
        //}

        //if (weaponPickUpList.Count > 1)
        //{
        // WeaponPrefabs = GameObject.FindGameObjectsWithTag("Weapon");
        //}

        //_weaponPrefabs = weaponPickUpList.ToArray();

        //
        //for (int i = 0; i < keys.Length; i++)
        //{
        //    int previousWeaponSelect = weaponSelect;

        //    if (Input.GetKeyDown(keys[i]))
        //    {
        //        FPSController.IsAiming = false;
        //        ArmsAnim = GetComponentInChildren<Animator>();
        //        if (ArmsAnim.runtimeAnimatorController != animators[0])
        //        {
        //            ArmsAnim.Play(_currentWeapon.ArmsLowerAnim.name);
        //        }
        //        weaponSelect = i;
        //    }

        //    if (previousWeaponSelect != weaponSelect)
        //        WeaponSpawnByClass((int)weaponSelect);
        //}
    }
}
