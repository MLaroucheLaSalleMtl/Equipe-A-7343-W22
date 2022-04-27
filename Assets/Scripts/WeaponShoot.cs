using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.EventSystems;

using System.Threading.Tasks;
using UnityEngine.Events;
using System;

public class WeaponShoot : MonoBehaviour
{
    //Temp Variables
    private float m_reloadTime;

    [HideInInspector] public Weapon _weapon;
    public WeaponManager _wpManager;
    public RigidBodyFPSController FPSController;
    private MuzzleScript muzzleFX;
    private Transform muzzleLocation;

    WaitForSeconds reloadWaitTime;
    WaitForSeconds autoFireWaitTime;
    WaitForSeconds autoADSFireWaitTime;    

    private PlayerInputActions.IWeaponActions m_weaponActions;

    [HideInInspector] public int currDMG, currMagAmmo, currAvailableAmmo;
    [SerializeField] private AudioSource dryFireAudioSource;
    [SerializeField] private AudioSource fireStartAudioSource;
    [SerializeField] private AudioSource fireEndAudioSource;
    //[SerializeField] private AudioSource bulletImpactAudioSource;

    [SerializeField] private Animator _weaponAnim;
    [SerializeField] private Animator _armsAnim;    
    //[SerializeField] private GameObject bulletObject;
    
    private RuntimeAnimatorController WP_runtimeAnimatorController;
    //private WeaponScriptableObject _currentWP = null;

    WeaponFireMode _weaponFireMode;

    private bool _canFire = true;
    private bool _canReload = true;

    
    private bool _isFiring = false;
    //private bool _pistolIsFiring = false;
    private bool _isAiming = false;

    private bool _isReloading = false;

    public bool autoFire;

    //private float nextFireRate = 0.0f;

    public Animator WeaponAnim { get => _weaponAnim; set => _weaponAnim = value; }

    public bool IsFiring { get => _isFiring; set => _isFiring = value; }    
    public bool IsAiming { get => _isAiming; set => _isAiming = value; }
    public bool CanFire { get => _canFire; set => _canFire = value; }
    public bool IsReloading { get => _isReloading; set => _isReloading = value; }
    public bool CanReload { get => _canReload; set => _canReload = value; }   

    #region  - IEnumerators -
    public IEnumerator Firing()
    {
        //if (this.isActiveAndEnabled == true)
        //{
            if (CanShoot())
            {
                if (FPSController.IsAiming)
                {
                    FireEvent();
                    IsFiring = false;
                    if (autoFire)
                    {
                        while (CanShoot())
                        {
                            yield return autoADSFireWaitTime;
                            FireEvent();
                        }
                    }
                }
                else
                {
                    FireEvent();
                    IsFiring = false;
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
                dryFireAudioSource.PlayOneShot(_wpManager._currentWeapon.weaponDryFire);
            }
        //}
    }

    public IEnumerator Reloading()
    {
        Mathf.Clamp(currAvailableAmmo, 0, _weapon.WeaponSO.WeaponMaxAmmo * 2);

        CanFire = false;

        StopCoroutine(Firing());

        if (currAvailableAmmo <= 0)
        {
            yield return null;
        }

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

        PlayerUIManager.munitionUpdate?.Invoke();

        Debug.Log("Current Mag Ammo : " + currMagAmmo + " , Current Ammo : " + currAvailableAmmo);
    }
    #endregion

    public bool CantShoot()
    {        
        return CanFire = false;
    }

    bool CanShoot()
    {
        bool canShoot = (currMagAmmo > 0 || currAvailableAmmo > 0) && CanFire == true;
        return canShoot;
    }

    private void Awake()
    {
        _isFiring = false;
        StopCoroutine(Firing());

        _weapon = GetComponent<Weapon>();

        WeaponAnim = GetComponent<Animator>();        
        FPSController = GetComponentInParent<RigidBodyFPSController>();

        _wpManager = GetComponentInParent<WeaponManager>();

        muzzleFX = GetComponentInChildren<MuzzleScript>();

        muzzleLocation = muzzleFX.WeaponMuzzleLocation;                  
    }

    //Start is called before the first frame update
    void Start()
    {
        StopCoroutine(Firing());

        if (_wpManager.CurrentWeaponType != WeaponType.Unarmed)
        {
            currDMG = Mathf.Clamp(currDMG, _weapon.WeaponSO.WeaponMinDMG, _weapon.WeaponSO.WeaponMaxDMG);
            currMagAmmo = _weapon.WeaponSO.WeaponMagazineAmmo;
            currAvailableAmmo = _weapon.WeaponSO.WeaponMaxAmmo;
            currAvailableAmmo = Mathf.Clamp(currAvailableAmmo, 0, _weapon.WeaponSO.WeaponMaxAmmo);
            PlayerUIManager.munitionUpdate?.Invoke();
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
        _isFiring = false;
        StopCoroutine(Firing());

        _armsAnim = _weapon.WeaponSO.ArmsAnimator;
    }

    private void OnDisable()
    {
        _canFire = false;
        _canReload = false;
        IsReloading = false;
        _isFiring = false;
    }    

    #region FireEvents
    private void FireEvent()
    {
        //IsFiring = true;
        if (IsReloading)
            StopCoroutine(Firing());

        if (currMagAmmo > 0 && this.isActiveAndEnabled == true && CanShoot())
        {
            _weapon.WeaponSO.PlayFireSFX(fireStartAudioSource, fireEndAudioSource);

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

            Debug.DrawRay(muzzleLocation.transform.position, muzzleLocation.transform.forward * _weapon.WeaponSO.MaxFireRange, Color.yellow);
            muzzleFX.StartEmit(_weapon.WeaponSO.MaxFireRange);

            if (_weapon.WeaponSO.WeaponType is WeaponType.AssaultRifle)
            {
                M416BulletPool.SharedBulletInstance.GetM416PooledBullet(muzzleLocation, _wpManager._currentWeapon.FireRate);
            }

            Debug.Log("Current DMG : " + currDMG +
                    " , Current Mag Ammo : " + currMagAmmo +
                    " , Current Ammo : " + currAvailableAmmo);
        }
        else if (currAvailableAmmo <= 0 || currMagAmmo <= 0 && !IsReloading)
        {
            if (_wpManager._currentWeapon.weaponDryFire != null)
            {
                dryFireAudioSource.PlayOneShot(_wpManager._currentWeapon.weaponDryFire);
            }
            CanFire = false;
            print("!!! -- Out Of Ammo -- !!!");
        }
        //else
        //{
        //    _isFiring = false;
        //}

        PlayerUIManager.munitionUpdate?.Invoke();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (!this.isActiveAndEnabled)
        {
            CanShoot();
        }

        if (currAvailableAmmo <= 0)
        {
            this._canReload = false;
            return;
        }

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

        switch (FPSController.IsSprinting)
        {
            case true:
                CanFire = false;
                break;
            case false:
                CanFire = true;
                break;            
        }        
    }
}
