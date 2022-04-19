using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public delegate void PlayerStaminaUpdate();
    public static PlayerStaminaUpdate playerStaminaUpdate;

    RigidBodyFPSController FPSController;

    public Slider playerStaminaSlider;

    private void Awake()
    {
        FPSController = FindObjectOfType<RigidBodyFPSController>();
        playerStaminaUpdate += PlayerStamina;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Move the player stats to a scriptable OBJ 
       
        playerStaminaSlider.value = FPSController.playerStatsSO.PlayerStamina;
    }

    public void PlayerStamina()
    {
        playerStaminaSlider.value = FPSController.playerStamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}