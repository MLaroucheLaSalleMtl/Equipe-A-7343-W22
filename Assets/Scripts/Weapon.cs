using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(WeaponShoot))]

public class Weapon : MonoBehaviour
{
    //public WeaponManager _wpManager = null;

    #region Singleton
    //public static Weapon instance = null;

    //private void Awake()
    //{
    //    //_wpManager = WeaponManager.instance;

    //    //if (instance == null)
    //    //    instance = this;
    //    //else if (instance != this)
    //    //    gameObject.SetActive(false);

    //    //weaponAnim = GetComponent<Animator>();
    //    //wpIdle = Animator.StringToHash(_weapon.Weapon_Idle_Anim);
    //    //wpFire = Animator.StringToHash(_weapon.Weapon_Fire_Anim);
    //    //wpReload = Animator.StringToHash(_weapon.Weapon_Reload_Anim);
    //    //wpADSFire = Animator.StringToHash(_weapon.Weapon_ADSFire_Anim);
    //    //wpStore = Animator.StringToHash("");
    //}
    #endregion

    public WeaponScriptableObject WeaponSO;
    //public Transform wpMuzzle;
    
    //private RigidBodyFPSController FPSController;
    //private MuzzleScript muzzleFX;

    //private int currDMG, currMagAmmo, currAvailableAmmo;
    //private AudioSource audioSource;

    ////[SerializeField] private GameObject bulletHole;
    ////[SerializeField] private GameObject bulletObject;
    //[SerializeField] private Transform WeaponMuzzleLocation;

    //private Animator weaponAnim;
    //private RuntimeAnimatorController WP_runtimeAnimatorController;

    //private int wpFire, wpReload, wpIdle, wpADSFire/* wpTakeOut, wpStore*/;    

    //Start is called before the first frame update
    //void Start()
    //{
    //    FPSController = GetComponentInParent<RigidBodyFPSController>();

    //    //Muzzle VFX:
    //    muzzleFX = FindObjectOfType<MuzzleScript>();

    //    //Weapon Animator:
    //    //weaponAnim = GetComponent<Animator>();
    //    //WP_runtimeAnimatorController = GetComponentInChildren<RuntimeAnimatorController>();
    //    //WP_runtimeAnimatorController = _weapon.weaponAnimatorController;        

    //    //Damage settings:
    //    currDMG = Mathf.Clamp(currDMG, _weapon.WeaponMinDMG, _weapon.WeaponMaxDMG);
    //    currMagAmmo = _weapon.WeaponMagazineAmmo;
    //    currAvailableAmmo = _weapon.WeaponMaxAmmo;     
    //}

    //public void WeaponFire()
    //{
        
    //}

    //public void WeaponReload()
    //{
    //    currAvailableAmmo -= currMagAmmo;
    //    currMagAmmo = _weapon.WeaponMagazineAmmo;
    //}

    ////IEnumerator WaitForAnimationEnd() 
    ////{
    ////    yield return ;
    ////}

    ////// Update is called once per frame
    //void Update()
    //{
    //    if (FPSController.fireBool || FPSController.isFiring || FPSController.fireTrigger && currMagAmmo >= 0)
    //    {
    //        //GameObject bulletOBJCopy = Instantiate(bulletObject, transform.position, transform.rotation);
    //        //bulletOBJCopy.GetComponent<Rigidbody>().AddForce(transform.forward * 150f, ForceMode.Impulse);

    //        currMagAmmo--;
    //        if (weaponAnim)
    //        {
    //            //weaponAnim.Play(wpFire);
    //            weaponAnim.SetBool("IsFiring", FPSController.isFiring);
    //        }
    //        if (_weapon.weaponSoundFX[0] /*&& !manager.isPaused */ /* && currMagAmmo >= 0*/)
    //            _weapon.weaponSoundFX[0].PlayOneShot(_weapon.weaponSoundFX[0].clip);

    //        RaycastHit hit;
    //        if (Physics.Raycast(WeaponMuzzleLocation.position, WeaponMuzzleLocation.forward, out hit, _weapon.MaxFireRange))
    //        {
    //            Debug.DrawRay(WeaponMuzzleLocation.position, WeaponMuzzleLocation.forward * _weapon.MaxFireRange, Color.yellow);
    //            if (hit.collider.CompareTag("Zombies"))
    //            {
    //                hit.collider.gameObject.GetComponent<EnemieController>().Hit();
    //            }

    //            muzzleFX.StartEmit(hit.point);
    //            //Quaternion rotFX = Quaternion.LookRotation(hit.normal);
    //            //GameObject bullet = Instantiate(bulletHole, hit.point, rotFX);
    //            //bullet.transform.parent = hit.transform;
    //        }
    //        else
    //        {
    //            Debug.DrawRay(WeaponMuzzleLocation.position, WeaponMuzzleLocation.forward * _weapon.MaxFireRange, Color.red);
    //            muzzleFX.StartEmit(_weapon.MaxFireRange);
    //        }
    //    }
    //    else if (FPSController.fireBool || FPSController.isFiring || FPSController.fireTrigger && currMagAmmo <= 0)
    //    {
    //        if (_weapon.weaponSoundFX[1] /*&& !manager.isPaused */ /* && currMagAmmo >= 0*/)
    //            _weapon.weaponSoundFX[1].PlayOneShot(_weapon.weaponSoundFX[0].clip);
    //        FPSController.fireTrigger = false;
    //        FPSController.fireBool = false;
    //        FPSController.isFiring = false;
    //    }

    //    //currDMG = new RNG().GetInstance().Next();


    //    Debug.Log("Current DMG : " + currDMG +
    //              " , Current Mag Ammo : " + currMagAmmo +
    //              " , Current Ammo : " + currAvailableAmmo);
    //    //if (true)
    //    //{

    //    //}
    //}
}
