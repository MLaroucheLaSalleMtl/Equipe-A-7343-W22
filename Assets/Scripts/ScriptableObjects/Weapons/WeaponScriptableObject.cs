using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public WeaponScriptableObject()
    {
        //this.WeaponPrefab = weaponPrefab;
        this.FireRate = fireRate;
        this.MaxFireRange = maxFireRange;
        this.WeaponMagazineAmmo = weaponMagazineAmmo;
        this.WeaponMaxAmmo = weaponMaxAmmo;
        this.WeaponMinDMG = weaponMinDMG;
        this.WeaponMaxDMG = weaponMaxDMG;
        this.RecoilX = recoilX;
        this.RecoilY = recoilY;        
    }

    //Encapsulated Fields
    //public GameObject WeaponPrefab { get => weaponPrefab; set => weaponPrefab = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float MaxFireRange { get => maxFireRange; set => maxFireRange = value; }
    public int WeaponMagazineAmmo { get => weaponMagazineAmmo; set => weaponMagazineAmmo = value; }
    public int WeaponMaxAmmo { get => weaponMaxAmmo; set => weaponMaxAmmo = value; }
    public int WeaponMinDMG { get => weaponMinDMG; set => weaponMinDMG = value; }
    public int WeaponMaxDMG { get => weaponMaxDMG; set => weaponMaxDMG = value; }
    public int RecoilX { get => recoilX; set => recoilX = value; }
    public int RecoilY { get => recoilY; set => recoilY = value; }
    public WeaponType WeaponType { get => weaponClass.WeaponType; set => weaponClass.WeaponType /*weaponType*/ = value; }  
    public WeaponFireMode WeaponFireMode { get => weaponClass.WeaponFireMode; set => weaponClass.WeaponFireMode = value; }

    [Header("--- Weapon UI Image ---")]
    [SerializeField] private Sprite weaponUISprite;
    [Header("--- Weapon Animator Options ---")]
    public Animator ArmsAnimator;
    public Animator weaponAnimator;
    public RuntimeAnimatorController weaponAnimatorController;
    public AnimationClip ArmsRaiseAnim;
    public AnimationClip ArmsLowerAnim;
    //public AnimationClip ArmsIdleAnim;
    public AnimationClip ArmsFireAnim;
    public AnimationClip ArmsADSFireAnim;
    public AnimationClip ArmsReloadAnim;
    public AnimationClip WeaponFireAnim;
    public AnimationClip WeaponADSFireAnim;
    public AnimationClip WeaponReloadAnim;
    [Header("--- Weapon VFX/SFX Settings ---")]
    public AudioClip weaponDryFire;
    //public AudioClip weaponFire;
    //public AudioClip weaponImpact;

    [Space(20)]
    [SerializeField] private AudioClip[] weaponFireStartSoundFX;
    [SerializeField] private AudioClip[] weaponFireEndSoundFX;
    [SerializeField] private AudioClip[] weaponReloadSoundFX;
    [SerializeField] private AudioClip[] weaponImpactSoundFX;
    [SerializeField] private AudioClip[] weaponImpactBodySoundFX;
    [SerializeField] private AudioClip[] weaponWhipSoundFX;
    //public Transform muzzleLocation;
    //public ParticleSystem particles;
    [Header("--- Weapon Stats ---")]
    [SerializeField] private float fireRate = 0f;
    [SerializeField] private float maxFireRange = 25f;
    [SerializeField] private int weaponMaxAmmo = 252;
    [SerializeField] private int weaponMagazineAmmo = 30;
    [SerializeField] private int weaponMinDMG = 20;
    [SerializeField] private int weaponMaxDMG = 25;
    [SerializeField] private int recoilX;
    [SerializeField] private int recoilY;
    [Header("--- Weapon Type ---")]
    [SerializeField] private WeaponClassScriptableObject weaponClass;

    //Test
    public IState weaponState;

    //private WeaponType weaponType;
    //
    
    public void PlayFireSFX(AudioSource fireStartAudioSource, AudioSource fireEndAudioSource)
    {
        int randomFireStartIndex = RNG.GetInstance().Next(0, 5);
        int randomFireEndIndex = RNG.GetInstance().Next(0, 3);

        fireStartAudioSource.PlayOneShot(weaponFireStartSoundFX[randomFireStartIndex]);
        fireEndAudioSource.PlayOneShot(weaponFireStartSoundFX[randomFireEndIndex]);
        //fireEndAudioSource.clip = weaponFireEndSoundFX[randomFireEndIndex];
    }

    //public void PlayReloadSFX()
    //{        
        
    //}

    public void PlayBulletImpactSFX(string tag, AudioSource bulletImpactAudioSource)
    {
        int rngIndex = RNG.GetInstance().Next(0, 5);
        if (tag == "Zombies")
        {
            bulletImpactAudioSource.PlayOneShot(weaponImpactBodySoundFX[rngIndex]);

            //switch (rngIndex)
            //{
            //    case 0:
            //        bulletImpactAudioSource.PlayOneShot(weaponImpactBodySoundFX[0]);
            //        break;
            //    case 1:
            //        bulletImpactAudioSource.PlayOneShot(weaponImpactBodySoundFX[1]);
            //        break;
            //    case 2:
            //        bulletImpactAudioSource.PlayOneShot(weaponImpactBodySoundFX[2]);
            //        break;
            //    case 3:
            //        bulletImpactAudioSource.PlayOneShot(weaponImpactBodySoundFX[3]);
            //        break;
            //    case 4:
            //        bulletImpactAudioSource.PlayOneShot(weaponImpactBodySoundFX[4]);
            //        break;
            //    case 5:
            //        bulletImpactAudioSource.PlayOneShot(weaponImpactBodySoundFX[5]);
            //        break;
            //}
        }
        else
        {
            bulletImpactAudioSource.PlayOneShot(weaponImpactSoundFX[rngIndex]);

            //switch (rngIndex)
            //{
            //    case 0:
            //        weaponImpact = weaponImpactSoundFX[0];
            //        break;
            //    case 1:
            //        weaponImpact = weaponImpactSoundFX[1];
            //        break;
            //    case 2:
            //        weaponImpact = weaponImpactSoundFX[2];
            //        break;
            //    case 3:
            //        weaponImpact = weaponImpactSoundFX[3];
            //        break;
            //    case 4:
            //        weaponImpact = weaponImpactSoundFX[4];
            //        break;
            //    case 5:
            //        weaponImpact = weaponImpactSoundFX[5];
            //        break;
            //}
        }        
    }

    public void OnEnable()
    {
        this.WeaponType = weaponClass.WeaponType;
        this.WeaponFireMode = weaponClass.WeaponFireMode;        

        //ArmsAnimator.gameObject.SetActive(true);
        //if (this.WeaponType != WeaponType.Unarmed)
        //{
        //    ArmsAnimator.Play(ArmsRaiseAnim.name); 
        //}
    }    

    //private void OnDisable()
    //{
    //    if (this.WeaponType != WeaponType.Unarmed)
    //    {
    //        ArmsAnimator.Play(ArmsLowerAnim.name);
    //    }
    //}
}