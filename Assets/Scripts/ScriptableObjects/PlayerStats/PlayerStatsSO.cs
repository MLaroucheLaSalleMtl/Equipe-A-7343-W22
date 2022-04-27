using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [SerializeField] float playerArmor;
    [SerializeField] float playerHealth;
    [SerializeField] float playerStamina;

    [SerializeField] public float m_MaxPlayerHealth { get; private set; } = 100;
    [SerializeField] public float m_MaxPlayerArmor { get; private set; } = 100;

    public float PlayerArmor { get => playerArmor; set => playerArmor = value; }
    public float PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public float PlayerStamina { get => playerStamina; set => playerStamina = value; }    
}
