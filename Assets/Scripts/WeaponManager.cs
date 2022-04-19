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
    private RigidBodyFPSController rbController;

    [SerializeField] private Transform weaponSocket;
    [SerializeField] private WeaponScriptableObject[] defaultWeapon;
    [SerializeField] private GameObject[] _weaponPrefabs;

    public List<GameObject> weaponPickUpList = new List<GameObject>();

    //[SerializeField] private GameObject[] _pistolsPrefab;
    //private GameObject _currentWeaponInstance = null;

    #region Singleton
    public static WeaponManager instance = null;

    private void Awake()
    {
        rbController = GetComponentInParent<RigidBodyFPSController>();

        _weaponScript = /*FindObjectOfType*/GetComponentInChildren<Weapon>();
        ArmsAnim = GetComponentInChildren<Animator>();

        if (weaponPickUpList.Count > 0)
        {
            foreach (GameObject weapon in weaponPickUpList) 
            {                
                Instantiate(weapon, this.transform);
                weapon.SetActive(true);
                WeaponPrefabs = GameObject.FindGameObjectsWithTag("Weapon");
                //weapon.SetActive(false);
            }
        }

        //if (weaponShoot == null)
        //    weaponShoot.CanFire = false;

        //if (instance == null)
        //    instance = this;
        //else if (instance != this)
        //    Destroy(gameObject);
    }
    #endregion

    [SerializeField] private KeyCode[] keys;
    [SerializeField] private Vector2 _mouseScrollWheel;
    public int weaponSelect;

    public IState _currentWeaponState;

    /*[SerializeField] */
    [SerializeField] public RuntimeAnimatorController[] animators;

    public WeaponScriptableObject _currentWeapon/* = null*/;   

    [SerializeField] private WeaponType _currentWeaponType;
    [SerializeField] private WeaponFireMode _currentWeaponFireMode;

    public WeaponType CurrentWeaponType { get => _currentWeaponType; set => _currentWeaponType = value; }
    public WeaponFireMode CurrentWeaponFireMode { get => _currentWeaponFireMode; set => _currentWeaponFireMode = value; }
    public GameObject[] WeaponPrefabs { get => _weaponPrefabs; set => _weaponPrefabs = value; }
    public IState CurrentWeaponState { get => _currentWeaponState; set => _currentWeaponState = value; }

    //private void OnEnable()
    //{
    //    test.Play(_currentWeapon.ArmsRaiseAnim.ToString());
    //}

    //private void OnDisable()
    //{
    //    test.Play(_currentWeapon.ArmsLowerAnim.ToString());   
    //}

    // Start is called before the first frame update
    void Start()
    {        
        CurrentWeaponType = WeaponType.Unarmed;
        WeaponSpawnByClass(CurrentWeaponType);
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
                CurrentWeaponState = Pistol_State.GetInstance();
                break;
            case 2:
                CurrentWeaponState = AssaultRifle_State.GetInstance();
                break;
        }

        Debug.Log(CurrentWeaponState.ToString());

        return CurrentWeaponState;
    }

    public void WeaponSpawnByClass(WeaponType weaponClass) 
    {
        for (int i = 0; i < _weaponPrefabs.Length; i++)
        {
            //if (ArmsAnim.runtimeAnimatorController.name == animators[0].name)                
            //    WeaponPrefabs[i].gameObject.SetActive(i == (int)weaponClass);
            //else
            //{
            //    ArmsAnim.SetTrigger("WeaponSwap");
            WeaponPrefabs[i].gameObject.SetActive(i == (int)weaponClass);
            //}
            //if (i != (int)weaponClass)
            //{
            //    GameObject.FindGameObjectsWithTag("Weapon");
            //    _weaponPrefabs[i].gameObject.SetActive(false/*i != (int)weaponClass*/);
            //}

            if (WeaponPrefabs[(int)weaponClass].activeInHierarchy)
            {
                _currentWeapon = GetComponentInChildren<Weapon>().WeaponSO;
                rbController.currentPlayerState = _currentWeapon.weaponState;
                CurrentWeaponFireMode = _currentWeapon.WeaponFireMode;
                ArmsAnim = GetComponentInChildren<Animator>();

                if (WeaponPrefabs[(int)weaponClass] != WeaponPrefabs[0])
                {
                    weaponShoot = WeaponPrefabs[(int)weaponClass].GetComponentInChildren<WeaponShoot>();
                }
                else
                    weaponShoot = null;

                //FindObjectOfType<WeaponShoot>().playerInputActions.Weapon.Enable();
                //PlayerInputActions.PlayerActions playerActions = new PlayerInputActions.PlayerActions();
                //playerActions.Fire.Enable();
                //PlayerUIManager.onPlayerSprint += 

                if (ArmsAnim.runtimeAnimatorController != animators[0])
                {
                    ArmsAnim.Play(_currentWeapon.ArmsRaiseAnim.name);
                }  
                
                //Debug.Log(GetCurrentWeaponState(i).ToString());
            }
        }    
    }

    public void OnMouseWeaponChange(InputAction.CallbackContext context)
    {
        _mouseScrollWheel.y = context.ReadValue<float>();
        
        Debug.Log("Scroll Wheel Y Axis Value : " + _mouseScrollWheel.y);
        
        int previousWeaponSelect = weaponSelect;

        _mouseScrollWheel.y = Mathf.Clamp(_mouseScrollWheel.y, -120f, 120f);

        if (_weaponPrefabs.Length != 0)
        {
            if (_mouseScrollWheel.y > 0f)
            {               
                rbController.IsAiming = false;
                ArmsAnim = GetComponentInChildren<Animator>();
                //if (ArmsAnim.runtimeAnimatorController != animators[0])
                //{
                //    ArmsAnim.Play(_currentWeapon.ArmsLowerAnim.name);
                //}
                //if(i == 1)
                weaponSelect = Mathf.Clamp(weaponSelect, 0, _weaponPrefabs.Length - 2);

                weaponSelect++;      

                GetCurrentWeaponState(weaponSelect);

                if (previousWeaponSelect != weaponSelect)                
                        WeaponSpawnByClass(CurrentWeaponType = (WeaponType)weaponSelect);               
            }

            if (_mouseScrollWheel.y < 0f)
            {
                rbController.IsAiming = false;
                ArmsAnim = GetComponentInChildren<Animator>();               

                if(weaponSelect > 0)
                    weaponSelect--;

                //if (ArmsAnim.runtimeAnimatorController != animators[0] && previousWeaponSelect != weaponSelect)
                //{
                //    ArmsAnim.Play(_currentWeapon.ArmsLowerAnim.name);
                //    ArmsAnim.SetTrigger("WeaponSwap");
                //    WeaponSpawnByClass(CurrentWeaponType = (WeaponType)weaponSelect);
                //}

                //if (ArmsAnim.runtimeAnimatorController != animators[0])
                //{
                //    ArmsAnim.Play(_currentWeapon.ArmsLowerAnim.name);
                //}

                GetCurrentWeaponState(weaponSelect);

                if (previousWeaponSelect != weaponSelect)                
                        WeaponSpawnByClass(CurrentWeaponType = (WeaponType)weaponSelect);
            }
        }        
    }

    public void OnKeyboardWeaponChange(InputAction.CallbackContext ctx)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            int previousWeaponSelect = weaponSelect;

            if (Input.GetKeyDown(keys[i]))
            {
                rbController.IsAiming = false;
                ArmsAnim = GetComponentInChildren<Animator>();
                if (ArmsAnim.runtimeAnimatorController != animators[0])
                {
                    ArmsAnim.Play(_currentWeapon.ArmsLowerAnim.name);
                }
                weaponSelect = i;
            }

            if (previousWeaponSelect != weaponSelect)
                WeaponSpawnByClass(CurrentWeaponType = (WeaponType)weaponSelect);
        }
    }

    public void OnControllerWeaponChange(InputAction.CallbackContext context)
    {

    }

    // Update is called once per frame
    void Update()
    {
        //
        //for (int i = 0; i < keys.Length; i++)
        //{
        //    int previousWeaponSelect = weaponSelect;

        //    if (Input.GetKeyDown(keys[i]))
        //    {
        //        rbController.IsAiming = false;
        //        ArmsAnim = GetComponentInChildren<Animator>();
        //        if (ArmsAnim.runtimeAnimatorController != animators[0])
        //        {
        //            ArmsAnim.Play(_currentWeapon.ArmsLowerAnim.name);
        //        }                
        //        weaponSelect = i;
        //    }

        //    if (previousWeaponSelect != weaponSelect)                           
        //        WeaponSpawnByClass(CurrentWeaponType = (WeaponType)weaponSelect);
        //}
    }
}
