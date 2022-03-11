using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    private WeaponManager _weaponManager;
    private RigidBodyFPSController FPSController;
    private MuzzleScript muzzleFX;

    private int currDMG, currMagAmmo, currAvailableAmmo;
    private AudioSource audioSource;

    //[SerializeField] private GameObject bulletHole;
    //[SerializeField] private GameObject bulletObject;
    //[SerializeField] private Transform WeaponMuzzleLocation;

    private Animator weaponAnim;
    private RuntimeAnimatorController WP_runtimeAnimatorController;
    //private WeaponScriptableObject _currentWP = null;

    //private int wpFire, wpReload, wpIdle, wpADSFire/* wpTakeOut, wpStore*/;    

    //Start is called before the first frame update
    void Start()
    {
        _weaponManager = WeaponManager.instance;
        FPSController = GetComponentInParent<RigidBodyFPSController>();

        //Muzzle VFX:
        //if (_weaponManager._currentWeapon != null)
        //{
        //}

        //_currentWP.WeaponType = _weaponManager.CurrentWeaponType;

        //_weaponScript = Weapon.instance;
        //_currentWP = _weaponManager._currentWeapon;
        //WeaponMuzzleLocation = _weaponScript.wpMuzzle;

        //Weapon Animator:
        //weaponAnim = GetComponent<Animator>();
        //WP_runtimeAnimatorController = GetComponentInChildren<RuntimeAnimatorController>();
        //WP_runtimeAnimatorController = _weaponScript._weapon.weaponAnimatorController;        

        //Damage settings:
             
    }

    // Update is called once per frame
    void Update()
    {
        //if (_weaponManager.CurrentWeaponType != WeaponType.Unarmed)
        //{
            
        //}

        if (_weaponManager._currentWeapon != null || _weaponManager.CurrentWeaponType == WeaponType.AssaultRifle || _weaponManager.CurrentWeaponType == WeaponType.Pistol) 
        {
            muzzleFX = GetComponentInChildren<MuzzleScript>();

            currDMG = Mathf.Clamp(currDMG, _weaponManager._currentWeapon.WeaponMinDMG, _weaponManager._currentWeapon.WeaponMaxDMG);
            currMagAmmo = _weaponManager._currentWeapon.WeaponMagazineAmmo;
            currAvailableAmmo = _weaponManager._currentWeapon.WeaponMaxAmmo;

            if (FPSController.fireBool || FPSController.isFiring || FPSController.fireTrigger && currMagAmmo >= 0)
            {
                //GameObject bulletOBJCopy = Instantiate(bulletObject, transform.position, transform.rotation);
                //bulletOBJCopy.GetComponent<Rigidbody>().AddForce(transform.forward * 150f, ForceMode.Impulse);

                currMagAmmo--;
                if (weaponAnim)
                {
                    //weaponAnim.Play(wpFire);
                    weaponAnim.SetBool("IsFiring", FPSController.isFiring);
                }
                //if (_weaponScript._weapon.weaponSoundFX[0] /*&& !manager.isPaused */ /* && currMagAmmo >= 0*/)
                //    _weaponScript._weapon.weaponSoundFX[0].PlayOneShot(_weaponScript._weapon.weaponSoundFX[0].clip);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, _weaponManager._currentWeapon.MaxFireRange))
                {
                    Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                    if (hit.collider.CompareTag("Zombies"))
                    {
                        hit.collider.gameObject.GetComponent<EnemieController>().Hit();
                    }
                    muzzleFX.StartEmit(hit.point);

                    //Quaternion rotFX = Quaternion.LookRotation(hit.normal);
                    //GameObject bullet = Instantiate(bulletHole, hit.point, rotFX);
                    //bullet.transform.parent = hit.transform;
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.forward * _weaponManager._currentWeapon.MaxFireRange, Color.red);
                    muzzleFX.StartEmit(_weaponManager._currentWeapon.MaxFireRange);
                }
            }
            else if (FPSController.fireBool || FPSController.isFiring || FPSController.fireTrigger && currMagAmmo <= 0)
            {
                //if (_weaponScript._weapon.weaponSoundFX[1] /*&& !manager.isPaused */ /* && currMagAmmo >= 0*/)
                //    _weaponScript._weapon.weaponSoundFX[1].PlayOneShot(_weaponScript._weapon.weaponSoundFX[0].clip);
                FPSController.fireTrigger = false;
                FPSController.fireBool = false;
                FPSController.isFiring = false;
            }
        }        

        //currDMG = new RNG().GetInstance().Next();


        Debug.Log("Current DMG : " + currDMG +
                  " , Current Mag Ammo : " + currMagAmmo +
                  " , Current Ammo : " + currAvailableAmmo);
        //if (true)
        //{

        //}
    }
}
