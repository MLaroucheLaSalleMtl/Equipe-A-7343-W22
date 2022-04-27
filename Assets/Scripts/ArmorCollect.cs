using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCollect : MonoBehaviour
{
    RigidBodyFPSController FPSController;
    float _armorCapacity = 50;

    private void Awake()
    {
        FPSController = FindObjectOfType<RigidBodyFPSController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RigidBodyFPSController>())
        {
            Collect();
        }
    }    

    public void Collect()
    {
        if (FPSController.playerArmor == FPSController.playerStatsSO.m_MaxPlayerArmor)
        {
            print("Max Armor");
            return;
        }
        else
        {
            FPSController.playerArmor += _armorCapacity;
            PlayerUIManager.playerArmorUpdate?.Invoke();
            Destroy(this.gameObject);
        }        
    }
}
