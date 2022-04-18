using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public delegate void PlayerStaminaUpdate();
    public static PlayerStaminaUpdate playerStaminaUpdate;

    RigidBodyFPSController FPSController;


    public Slider playerHealthSlider;
    public Slider playerArmorSlider;
    public Slider playerStaminaSlider;

    private void Awake()
    {
        FPSController = FindObjectOfType<RigidBodyFPSController>();
        playerStaminaUpdate += OnPlayerSprint;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Move the player stats to a scriptable OBJ 
        playerArmorSlider.value = FPSController.playerStatsSO.PlayerArmor;
        playerHealthSlider.value = FPSController.playerStatsSO.PlayerHealth;
        playerStaminaSlider.value = FPSController.playerStatsSO.PlayerStamina;
    }

    public void OnPlayerSprint()
    {
        playerStaminaSlider.value -= 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}