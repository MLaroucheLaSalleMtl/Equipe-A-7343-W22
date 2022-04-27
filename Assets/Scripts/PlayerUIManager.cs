using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public delegate void PlayerStaminaUpdate();
    public static PlayerStaminaUpdate playerStaminaUpdate;

    public delegate void PlayerHealthUpdate();
    public static PlayerHealthUpdate playerHealthUpdate;

    public delegate void PlayerArmorUpdate();
    public static PlayerArmorUpdate playerArmorUpdate;

    public delegate void WeaponMunitionUpdate();
    public static WeaponMunitionUpdate munitionUpdate;

    RigidBodyFPSController FPSController;

    public Slider playerStaminaSlider;
    public Slider playerArmorSlider;
    public Slider playerHealthSlider;

    [SerializeField] private TMP_Text weaponAmmo;

    private void Awake()
    {
        FPSController = FindObjectOfType<RigidBodyFPSController>();

        playerStaminaUpdate += UpdateStamina;
        playerHealthUpdate += UpdateHealth;
        playerArmorUpdate += UpdateArmor;

        munitionUpdate += UpdateMunition;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Move the player stats to a scriptable OBJ 
        //UpdateArmor();
        //UpdateHealth();
        //UpdateStamina();
               
        playerStaminaSlider.value = FPSController.playerStatsSO.PlayerStamina;
        playerArmorSlider.value = FPSController.playerStatsSO.PlayerArmor;
        playerHealthSlider.value = FPSController.playerStatsSO.PlayerHealth;
    }

    public void UpdateArmor()
    {
        playerArmorSlider.value = FPSController.playerArmor;
    }

    public void UpdateHealth()
    {
        playerHealthSlider.value = FPSController.playerHealth;
    }

    public void UpdateStamina()
    {
        playerStaminaSlider.value = FPSController.playerStamina;
    }

    public void UpdateMunition()
    {
        if (FPSController._weaponManager.CurrentWeaponType == WeaponType.Unarmed)
            weaponAmmo.text = "Unarmed";
        else
            weaponAmmo.text = FPSController._weaponManager.weaponShoot.currMagAmmo.ToString() + " / " + FPSController._weaponManager.weaponShoot.currAvailableAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}