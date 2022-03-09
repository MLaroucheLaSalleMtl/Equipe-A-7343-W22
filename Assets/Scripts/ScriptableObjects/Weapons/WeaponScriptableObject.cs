using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    private Animator weaponAnimator;
    public RuntimeAnimatorController weaponAnimatorController;
    public AudioSource[] weaponSoundFX;
    public float fireRange = 25f;
    public int weaponMaxAmmo = 252;
    public int weaponMagazineAmmo = 30;
    public int weaponMinDMG = 20;
    public int weaponMaxDMG = 25;
    public int recoilX;
    public int recoilY;    

    ////Static variables
    //public static int stcWeaponMinDMG;
    //public static int stcWeaponMaxDMG;
        
    //[SerializeField] public int weaponDMG { get; private set; } = RNG.GetInstance().Next(stcWeaponMinDMG, stcWeaponMaxDMG);
}
