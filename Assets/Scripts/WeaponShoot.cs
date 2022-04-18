using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.EventSystems;

using System.Threading.Tasks;
using UnityEngine.Events;
using System;

public class WeaponShoot : MonoBehaviour/*, PlayerInputActions.IWeaponActions*/
{
    public delegate void TestOnFireAutoDelegate(InputAction.CallbackContext context);
    public static TestOnFireAutoDelegate testOnFireAutoDelegate;

    public delegate void OnFireAutoDelegate();
    public static OnFireAutoDelegate onFireAutoDelegate;

    public delegate void OnFireSemiDelegate();
    public static OnFireSemiDelegate onFireSemiDelegate;

    public delegate void OnReloadDelegate();
    public static OnReloadDelegate onReloadDelegate;

    PlayerInputActions playerInputActions;
    PlayerInputActions.PlayerActions playerActions;

    //Temp Variables
    private float m_reloadTime;

    private Weapon _weapon;
    public WeaponManager _wpManager;
    public RigidBodyFPSController FPSController;
    private MuzzleScript muzzleFX;
    private Transform muzzleLocation;

    WaitForSeconds reloadWaitTime;
    WaitForSeconds autoFireWaitTime;
    WaitForSeconds autoADSFireWaitTime;

    //public InputActionAsset _inputActionsAsset;

    //private PlayerInput m_playerInput;
    //private InputActionMap WeaponControlMap;

    private PlayerInputActions.IWeaponActions m_weaponActions;

    [HideInInspector] public int currDMG, currMagAmmo, currAvailableAmmo;
    [SerializeField] private AudioSource dryFireAudioSource;
    [SerializeField] private AudioSource fireStartAudioSource;
    [SerializeField] private AudioSource fireEndAudioSource;
    [SerializeField] private AudioSource bulletImpactAudioSource;

    //[SerializeField] private GameObject bulletHole;
    //[SerializeField] private Transform WeaponMuzzleLocation;

    [SerializeField] private Animator _weaponAnim;
    [SerializeField] private Animator _armsAnim;
    [SerializeField] private GameObject[] bulletHole;
    [SerializeField] private GameObject bulletObject;
    
    private RuntimeAnimatorController WP_runtimeAnimatorController;
    //private WeaponScriptableObject _currentWP = null;

    WeaponFireMode _weaponFireMode;

    private bool _canFire = true;
    private bool _canReload = true;

    private bool _assaultRiffleIsFiring = false;
    private bool _isFiring = false;
    private bool _pistolIsFiring = false;
    private bool _isAiming = false;

    [SerializeField] private bool _isReloading = false;

    public bool autoFire;

    private float nextFireRate = 0.0f;

    public Animator WeaponAnim { get => _weaponAnim; set => _weaponAnim = value; }
    public bool AssaultRiffleIsFiring { get => _assaultRiffleIsFiring; set => _assaultRiffleIsFiring = value; }
    public bool IsFiring { get => _isFiring; set => _isFiring = value; }
    public bool PistolIsFiring { get => _pistolIsFiring; set => _pistolIsFiring = value; }
    public bool IsAiming { get => _isAiming; set => _isAiming = value; }
    public bool CanFire { get => _canFire; set => _canFire = value; }
    public bool IsReloading { get => _isReloading; set => _isReloading = value; }
    public bool CanReload { get => _canReload; set => _canReload = value; }   

    #region  - IEnumerators -
    public IEnumerator Firing()
    {
        if (CanShoot())
        {
            if (FPSController.IsAiming)
            {
                //if (CanShoot())
                //{
                    FireEvent();
                    _isFiring = false;
                    if (autoFire)
                    {
                        while (CanShoot())
                        {
                            yield return autoADSFireWaitTime;
                            FireEvent();
                        }
                    }
                //}
            }
            else
            {
                FireEvent();
                _isFiring = false;
                if (autoFire)
                {
                    while (CanShoot())
                    {
                        yield return autoFireWaitTime;
                        FireEvent();
                    }
                }

            }
        }
        else
        {
            if (_wpManager._currentWeapon.weaponDryFire != null)
            {
                //Play Dry Fire SFX Here instead of inside the FireEvent Funcion
                dryFireAudioSource.PlayOneShot(_wpManager._currentWeapon.weaponDryFire);
            }
        }
    }

    public IEnumerator Reloading()
    {
        CanFire = false;

        StopCoroutine(Firing());

        if (currAvailableAmmo == 0)
            yield return null;

        Debug.Log("- Reloading Weapon -");

        this.IsReloading = true;
        this._canReload = false;
        this._isFiring = false;
        FPSController.IsAiming = false;

        _wpManager.ArmsAnim.SetTrigger("Reload"); _weaponAnim.SetTrigger("Reload");

        yield return reloadWaitTime;

        Debug.Log("- Reloading Complete -");

        this._canFire = true;
        this._canReload = true;
        this.IsReloading = false;

        Mathf.Clamp(currAvailableAmmo, 0, _wpManager._currentWeapon.WeaponMaxAmmo);

        int _fullMagazine = _wpManager._currentWeapon.WeaponMagazineAmmo - currMagAmmo;

        if (currAvailableAmmo > _weapon.WeaponSO.WeaponMagazineAmmo)
        {
            currAvailableAmmo -= _fullMagazine;
            currMagAmmo += _fullMagazine;
        }
        else if (currAvailableAmmo <= _fullMagazine && currAvailableAmmo > 0)
        {
            int _tmpAmmo = currAvailableAmmo + currMagAmmo;
            currMagAmmo += _tmpAmmo;
            currAvailableAmmo -= currAvailableAmmo;
        }

        Debug.Log("Current Mag Ammo : " + currMagAmmo + " , Current Ammo : " + currAvailableAmmo);
    }
    #endregion

    bool CantShoot()
    {        
        return CanFire = false;
    }

    bool CanShoot()
    {
        bool canShoot = currMagAmmo > 0 && CanFire;
        return canShoot;
    }

    private void Awake()
    {
        //playerInputActions = new PlayerInputActions();
        //playerActions = playerInputActions.Player;

        //playerActions.Fire.performed += ctx => _isFiring = ctx.performed;

        _weapon = GetComponent<Weapon>();

        WeaponAnim = GetComponent<Animator>();        
        FPSController = GetComponentInParent<RigidBodyFPSController>();

        _wpManager = GetComponentInParent<WeaponManager>();

        muzzleFX = GetComponentInChildren<MuzzleScript>();

        //dryFireAudioSource = GetComponent<AudioSource>();

        muzzleLocation = muzzleFX.WeaponMuzzleLocation;                  
    }

    //Start is called before the first frame update
    void Start()
    {
        if (_wpManager.CurrentWeaponType != WeaponType.Unarmed)
        {
            currDMG = Mathf.Clamp(currDMG, _weapon.WeaponSO.WeaponMinDMG, _weapon.WeaponSO.WeaponMaxDMG);
            currMagAmmo = _weapon.WeaponSO.WeaponMagazineAmmo;
            currAvailableAmmo = _weapon.WeaponSO.WeaponMaxAmmo;
            currAvailableAmmo = Mathf.Clamp(currAvailableAmmo, 0, _weapon.WeaponSO.WeaponMaxAmmo);
        }

        if (_weapon.WeaponSO.WeaponFireMode is WeaponFireMode.Auto)
        {
            autoFire = true;
        }

        autoFireWaitTime = new WaitForSeconds(0.02f / _wpManager._currentWeapon.ArmsFireAnim.length);
        reloadWaitTime = new WaitForSeconds(_weapon.WeaponSO.ArmsReloadAnim.length);
        autoADSFireWaitTime = new WaitForSeconds(0.02f / _wpManager._currentWeapon.ArmsADSFireAnim.length);
        
    }

    private void OnEnable()
    {
        _canFire = true;
        _canReload = true;
        
        _armsAnim = _weapon.WeaponSO.ArmsAnimator;

        onFireAutoDelegate += AutoFireEvent;
        onFireSemiDelegate += FireEvent;

        onReloadDelegate += OnReload;

        testOnFireAutoDelegate += OnFireAssaultRifles;
    }

    private void OnDisable()
    {
        _canFire = false;
        _canReload = false;
        IsReloading = false;
        _isFiring = false;

        onFireAutoDelegate -= AutoFireEvent;
        onFireSemiDelegate -= FireEvent;

        onReloadDelegate -= OnReload;

        testOnFireAutoDelegate -= OnFireAssaultRifles;
    }
    
    public void OnFirePistols(InputAction.CallbackContext context)
    {
        if (_canFire && context.interaction is TapInteraction)
        {
            //IsFiring = context.performed;
            FireEvent();
        } 

        IsFiring = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (_canFire && !IsReloading)
        {
            switch (context.phase)
            {
                //case InputActionPhase.Disabled:
                //    break;
                //case InputActionPhase.Waiting:
                //    break;
                case InputActionPhase.Performed:
                    if (context.interaction is HoldInteraction)
                    {
                        AutoFireEvent();
                    }
                    else
                    {
                        FireEvent();
                    }
                    IsFiring = false;
                    break;

                case InputActionPhase.Started:
                    if (context.interaction is HoldInteraction)
                        IsFiring = true;
                    break;

                case InputActionPhase.Canceled:
                    IsFiring = false;
                    break;
            }
        }
    }

    public void OnFireAssaultRifles(InputAction.CallbackContext context)
    {
        if (_canFire)
        {
            //inputActions.Player.Fire.performed += context => AssaultRiffleIsFiring = context.ReadValueAsButton();
            //if (context.ReadValueAsButton())
            //{
            //    AssaultRiffleIsFiring = true;
            //}           
            //if (context.action.triggered)
            //{
            //    AssaultRiffleIsFiring = true;
            //    SemiFireEvent();
            //    AssaultRiffleIsFiring = false;
            //}
            if (context.performed)
            {
                _isFiring = true;
                AutoFireEvent();
            }
            if (context.canceled)
            {
                _isFiring = false;
            }
        }
    }    

    #region FireEvents
    public void FireEvent()
    {
        //IsFiring = true;

        if (IsReloading)
            StopCoroutine(Firing());      

        if (currMagAmmo > 0)
        {
            _weapon.WeaponSO.PlayFireSFX(fireStartAudioSource, fireEndAudioSource);

            //IsFiring = true;

            currMagAmmo--;

            if (FPSController.IsAiming)
            {
                WeaponAnim.SetTrigger("IsFiring&Aiming"); _wpManager.ArmsAnim.SetTrigger("IsFiring&Aiming");
            }
            else
            {
                WeaponAnim.SetTrigger("IsFiring"); _wpManager.ArmsAnim.SetTrigger("IsFiring");
            }

            _isFiring = false;

            //if (!FPSController.IsAiming)

            RaycastHit hit;
            if (Physics.Raycast(muzzleLocation.transform.position, muzzleLocation.transform.forward, out hit, _weapon.WeaponSO.MaxFireRange))
            {
                Debug.DrawRay(muzzleLocation.transform.position, muzzleLocation.transform.forward * hit.distance, Color.yellow);
                muzzleFX.StartEmit(hit.point);

                GameObject bulletOBJCopy = /*Instantiate(bulletObject, muzzleLocation.transform.position, muzzleLocation.transform.rotation)*/ BulletPool.SharedBulletInstance.GetPooledBullet();

                if (bulletOBJCopy != null)
                {
                    bulletOBJCopy.transform.position = muzzleLocation.transform.position;
                    bulletOBJCopy.transform.rotation = muzzleLocation.transform.rotation;
                    bulletOBJCopy.SetActive(true);
                    bulletOBJCopy.GetComponent<Rigidbody>().AddForce(muzzleLocation.transform.forward * _weapon.WeaponSO.FireRate/* / 60*/, ForceMode.Impulse);
                }
                if (hit.collider.CompareTag("Ground"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[0], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                    _weapon.WeaponSO.PlayBulletImpactSFX(hit.collider.tag, bulletImpactAudioSource);
                    Destroy(bullet, 2f);
                }
                if (hit.collider.CompareTag("Wood"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[1], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                    _weapon.WeaponSO.PlayBulletImpactSFX(hit.collider.tag, bulletImpactAudioSource);
                    Destroy(bullet, 2f);
                }
                if (hit.collider.CompareTag("Sand"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[2], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                    _weapon.WeaponSO.PlayBulletImpactSFX(hit.collider.tag, bulletImpactAudioSource);
                    Destroy(bullet, 2f);
                }
                if (hit.collider.CompareTag("Rocks"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[3], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                    _weapon.WeaponSO.PlayBulletImpactSFX(hit.collider.tag, bulletImpactAudioSource);
                    Destroy(bullet, 2f);
                }
                if (hit.collider.CompareTag("Zombies"))
                {
                    hit.collider.gameObject.GetComponent<EnemieController>().Hit();
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[4], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                    _weapon.WeaponSO.PlayBulletImpactSFX(hit.collider.tag, bulletImpactAudioSource);
                    Destroy(bullet, 2f);
                }
            }
            else
            {
                Debug.DrawRay(muzzleLocation.transform.position, muzzleLocation.transform.forward * _weapon.WeaponSO.MaxFireRange, Color.red);
                muzzleFX.StartEmit(_weapon.WeaponSO.MaxFireRange);
            }

            Debug.Log("Current DMG : " + currDMG +
                    " , Current Mag Ammo : " + currMagAmmo +
                    " , Current Ammo : " + currAvailableAmmo);
        }
        //if (IsFiring && IsAiming)
        //    GetComponentInParent<Animator>().SetBool("isAiming&Firing", true);
        else if (/*FPSController.fireBool || FPSController.*//*IsFiring*/ /*|| FPSController.fireTrigger*/ /*&&*/ currAvailableAmmo <= 0 || currMagAmmo <= 0 && !IsReloading)
        {
            if (currAvailableAmmo <= 0)
            {
                CanFire = false;
                print("!!! -- No more Available Ammo -- !!!");
            }

            if (currMagAmmo <= 0)
            {
                CanFire = false;
                print("!!! -- Magazine is Empty -- !!!");
            }

            //if (_weaponScript._weapon.weaponSoundFX[1] /*&& !manager.isPaused */ /* && currMagAmmo >= 0*/)
            //    _weaponScript._weapon.weaponSoundFX[1].PlayOneShot(_weaponScript._weapon.weaponSoundFX[0].clip);
            //FPSController.fireTrigger = false;
            //FPSController.fireBool = false;
            /*FPSController*/
            CanFire = false;
            print("!!! -- Out Of Ammo -- !!!");
            //IsFiring = false;
        }

        //IsFiring = false;
        //}
        //else
        //print("Payer is Unarmed !!!");

        //currDMG = new RNG().GetInstance().Next();
                
        //else if (IsAiming/* && IsFiring*/)

        //else
        //_weaponManager.ArmsAnim.SetBool("isFiring", IsFiring /*&& !IsAiming ||*/ /*_weaponManager._currentWeapon.ArmsFireAnim.name*/);
        //_weaponScript.WeaponAnim.SetBool("IsFiring", _isFiring);
        //}
        //}
    }

    IEnumerator DisposeDecal(GameObject bulletHole)
    {
        yield return new WaitForSeconds(2f);

        Destroy(bulletHole);
    }

    public void AutoFireEvent()
    {
        if (IsFiring && currMagAmmo > 0)
        {
            currMagAmmo--;
            WeaponAnim.SetBool("IsFiring", IsFiring);

            if(!FPSController.IsAiming)
                _wpManager.ArmsAnim.SetBool("isFiring", IsFiring && !FPSController.IsAiming/*_weaponManager._currentWeapon.ArmsFireAnim.name*/);
            
            RaycastHit hit;
            if (Physics.Raycast(muzzleLocation.transform.position, muzzleLocation.transform.forward, out hit, _weapon.WeaponSO.MaxFireRange))
            {
                Debug.DrawRay(muzzleLocation.transform.position, muzzleLocation.transform.forward * hit.distance, Color.yellow);
                muzzleFX.StartEmit(hit.point);

                GameObject bulletOBJCopy = /*Instantiate(bulletObject, muzzleLocation.transform.position, muzzleLocation.transform.rotation)*/ BulletPool.SharedBulletInstance.GetPooledBullet();

                if (bulletOBJCopy != null)
                {
                    bulletOBJCopy.transform.position = muzzleLocation.transform.position;
                    bulletOBJCopy.transform.rotation = muzzleLocation.transform.rotation;
                    //bulletOBJCopy.SetActive(true);
                    bulletOBJCopy.GetComponent<Rigidbody>().AddForce(muzzleLocation.transform.forward * _weapon.WeaponSO.FireRate/* / 60*/, ForceMode.Impulse);
                }                

                if (hit.collider.CompareTag("Ground"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[0], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                }
                if (hit.collider.CompareTag("Wood"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[1], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                }
                if (hit.collider.CompareTag("Sand"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[2], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                }
                if (hit.collider.CompareTag("Rocks"))
                {
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[3], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                }
                if (hit.collider.CompareTag("Zombies"))
                {
                    hit.collider.gameObject.GetComponent<EnemieController>().Hit();
                    Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    GameObject bullet = Instantiate(bulletHole[4], hit.point, rotFX);
                    bullet.transform.parent = hit.transform;
                    //bulletOBJCopy.SetActive(false);
                }
            }
            else
            {
                Debug.DrawRay(muzzleLocation.transform.position, muzzleLocation.transform.forward * _weapon.WeaponSO.MaxFireRange, Color.red);
                muzzleFX.StartEmit(_weapon.WeaponSO.MaxFireRange);
            }

            //IsFiring = false;
        }
        //if (IsFiring && IsAiming)
        //    GetComponentInParent<Animator>().SetBool("isAiming&Firing", true);
        else if (/*FPSController.fireBool || FPSController.*//*IsFiring*/ /*|| FPSController.fireTrigger*/ /*&&*/ currAvailableAmmo <= 0 || currMagAmmo <= 0 && !IsReloading)
        {
            //if (_weaponScript._weapon.weaponSoundFX[1] /*&& !manager.isPaused */ /* && currMagAmmo >= 0*/)
            //    _weaponScript._weapon.weaponSoundFX[1].PlayOneShot(_weaponScript._weapon.weaponSoundFX[0].clip);
            //FPSController.fireTrigger = false;
            //FPSController.fireBool = false;
            /*FPSController*/
            CanFire = false;
            print("!!! -- Out Of Ammo -- !!!");
            //IsFiring = false;
        }
        //}
        //else
        //print("Payer is Unarmed !!!");

        //currDMG = new RNG().GetInstance().Next();

        Debug.Log("Current DMG : " + currDMG +
                    " , Current Mag Ammo : " + currMagAmmo +
                    " , Current Ammo : " + currAvailableAmmo);

        //else if (IsAiming/* && IsFiring*/)

        //else
        //_weaponManager.ArmsAnim.SetBool("isFiring", IsFiring /*&& !IsAiming ||*/ /*_weaponManager._currentWeapon.ArmsFireAnim.name*/);
        //_weaponScript.WeaponAnim.SetBool("IsFiring", _isFiring);
        //}
        //}
    }
    #endregion

    bool AnimIsPlaying()
    {        
        return _wpManager.ArmsAnim.GetCurrentAnimatorStateInfo(0).length > _wpManager.ArmsAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimIsPlaying(string _stateTag)
    {
        return AnimIsPlaying() && _wpManager.ArmsAnim.GetCurrentAnimatorStateInfo(0).IsTag(_stateTag)/* && _weaponAnim.GetCurrentAnimatorStateInfo(0).IsTag(_stateTag))*/;
    }

    public void OnReload(/*InputAction.CallbackContext context*/)
    {
        this._canFire = false;

        if (currAvailableAmmo == 0)
            return;

        if (_canReload /*&& context.performed*/ /*&& AnimIsPlaying("Reload")*/)
        {
            //if (_isReloading)
            //{
            //    this._canReload = false;
            //    this._isFiring = false;
            //    FPSController.IsAiming = false;
            //}

            Debug.Log("- Reloading Weapon -");

            this.IsReloading = true;
            this._canReload = false;
            this._isFiring = false;
            FPSController.IsAiming = false;

            //_weaponAnim.SetBool("IsReloading", _isReloading); _wpManager.ArmsAnim.SetBool("IsReloading", _isReloading);

            _wpManager.ArmsAnim.SetTrigger("Reload"); _weaponAnim.SetTrigger("Reload");

            StartCoroutine(Reloading());
        }        
    }    

    void RateOfFire()
    {
        nextFireRate = _weapon.WeaponSO.FireRate - Time.deltaTime;
        nextFireRate = Mathf.Clamp01(nextFireRate);
        if (nextFireRate > 0f)       
            CanFire = false; 
        else if(nextFireRate == 0f)
            CanFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        

        //controls.Player.Fire.performed += _ => IsFiring = true;
        //controls.Player.Fire.canceled += _ => IsFiring = false;

        if (currAvailableAmmo == 0)
            CanReload = false;

        if (IsReloading)
        {
            this._canFire = false;
            this._canReload = false;
            FPSController.IsAiming = false;
        }
        else
        {
            this._canFire = true;
            this._canReload = true;
        }

        RateOfFire();

        //if (_isReloading == true)
        //{
        //    _canFire = false;
        //    _canReload = false;
        //    FPSController.IsAiming = false;
        //}


       
            //if (_wpManager.CurrentWeaponState is Pistol_State && IsFiring)
            //    SemiFireEvent();
            ///*else */if (/*_wpManager.CurrentWeaponState is AssaultRifle_State &&*/ IsFiring)
            //    AutoFireEvent();
        

        

        if (_wpManager.CurrentWeaponType != WeaponType.Unarmed)
        {
            //if (_canFire && _wpManager.CurrentWeaponState is Pistol_State && IsFiring)
            //{
            //if(!IsAiming && IsFiring)
            //WeaponAnim.SetBool("IsFiring", IsFiring);

            //_wpManager.ArmsAnim.SetBool("isFiring", IsFiring && !FPSController.IsAiming/*_weaponManager._currentWeapon.ArmsFireAnim.name*/);

                //if (_wpManager.ArmsAnim.runtimeAnimatorController != _wpManager.animators[0])
                //_armsAnim.SetBool("isFiring", IsFiring/* && !IsAiming*//*_weaponManager._currentWeapon.ArmsFireAnim.name*/);

            //}

            //if (_canFire && _wpManager.CurrentWeaponState is AssaultRifle_State && IsFiring)
            //{ 
            //    //if(!IsAiming && IsFiring)
            //    _wpManager.ArmsAnim.SetBool("isFiring", IsFiring && !FPSController.IsAiming/*_weaponManager._currentWeapon.ArmsFireAnim.name*/);

            //    //if (_wpManager.ArmsAnim.runtimeAnimatorController != _wpManager.animators[0])
            //    //_armsAnim.SetBool("isFiring", IsFiring/* && !IsAiming*//*_weaponManager._currentWeapon.ArmsFireAnim.name*/);

            //    WeaponAnim.SetBool("IsFiring", IsFiring);
            //}          
            
        }

        

        //switch (_isReloading)
        //{
        //    case true:
        //        this._canFire = false;
        //        this._canReload = false;
        //        FPSController.IsAiming = false;
        //        break;
        //    case false:
        //        this._canFire = true;
        //        this._canReload = true;
        //        break;
        //}


        switch (FPSController.IsSprinting)
        {
            case true:
                CanFire = false;
                break;
            case false:
                CanFire = true;
                break;
            //default:
            //    CanFire = true;
            //    break;
        }        
    }
}
