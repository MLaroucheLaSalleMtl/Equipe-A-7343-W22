using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public WeaponScriptableObject()
    {
        //this.WeaponPrefab = weaponPrefab;
        this.MaxFireRange = maxFireRange;
        this.WeaponMagazineAmmo = weaponMagazineAmmo;
        this.WeaponMaxAmmo = weaponMaxAmmo;
        this.WeaponMinDMG = weaponMinDMG;
        this.WeaponMaxDMG = weaponMaxDMG;
        this.RecoilX = recoilX;
        this.RecoilY = recoilY;        
    }

    //Encapsuated Fields
    //public GameObject WeaponPrefab { get => weaponPrefab; set => weaponPrefab = value; }
    public int WeaponMagazineAmmo { get => weaponMagazineAmmo; set => weaponMagazineAmmo = value; }
    public int WeaponMaxAmmo { get => weaponMaxAmmo; set => weaponMaxAmmo = value; }
    public int WeaponMinDMG { get => weaponMinDMG; set => weaponMinDMG = value; }
    public int WeaponMaxDMG { get => weaponMaxDMG; set => weaponMaxDMG = value; }
    public int RecoilX { get => recoilX; set => recoilX = value; }
    public int RecoilY { get => recoilY; set => recoilY = value; }
    public float MaxFireRange { get => maxFireRange; set => maxFireRange = value; }
    public WeaponType WeaponType { get => weaponClass.WeaponType; set => weaponClass.WeaponType /*weaponType*/ = value; }

    [Header("--- Weapon UI Image ---")]
    [SerializeField] private Sprite weaponUISprite;
    [Header("--- Weapon Animator Options ---")]
    public Animator weaponAnimator;
    public RuntimeAnimatorController weaponAnimatorController;
    public string Weapon_Fire_Anim;
    public string Weapon_ADSFire_Anim;
    public string Weapon_Reload_Anim;
    public string Weapon_Idle_Anim;
    [Header("--- Weapon VFX/SFX Settings ---")]
    public AudioSource[] weaponSoundFX;
    //public Transform muzzleLocation;
    //public ParticleSystem particles;
    [Header("--- Weapon Stats ---")]
    [SerializeField] private float maxFireRange = 25f;
    [SerializeField] private int weaponMaxAmmo = 252;
    [SerializeField] private int weaponMagazineAmmo = 30;
    [SerializeField] private int weaponMinDMG = 20;
    [SerializeField] private int weaponMaxDMG = 25;
    [SerializeField] private int recoilX;
    [SerializeField] private int recoilY;
    [Header("--- Weapon Type ---")]
    [SerializeField] private WeaponClassScriptableObject weaponClass;
    //private WeaponType weaponType;
    //

    private void OnEnable()
    {
        this.WeaponType = weaponClass.WeaponType;
    }
}