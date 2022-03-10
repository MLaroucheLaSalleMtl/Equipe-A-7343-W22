using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
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

    //private void Awake()
    //{
    //    stcWeaponMinDMG = weaponMinDMG;
    //    stcWeaponMaxDMG = weaponMaxDMG;
    //}

    //[SerializeField] public int weaponDMG { get; private set; } = RNG.GetInstance().Next(stcWeaponMinDMG, stcWeaponMaxDMG);
}
