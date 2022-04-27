using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerDamage : MonoBehaviour
{
    public static int playerHealth = 100;
    public static bool isGameover;
    public TextMeshProUGUI playertext;
   
    // Start is called before the first frame update
    void Start()
    {
        isGameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        playertext.text = " " + playerHealth;
        if(isGameover)
        {
            //display game over screen
        }
    }
   

    public static void TakeDamage(int damageAmount)
    {        
        playerHealth -= damageAmount;
        if (playerHealth <= 0)
            isGameover = true;
    }
}
