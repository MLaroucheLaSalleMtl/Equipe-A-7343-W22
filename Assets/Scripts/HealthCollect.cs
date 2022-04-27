using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollect : MonoBehaviour
{
    RigidBodyFPSController FPSController;
    float _healthCapacity = 50;

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
        if (FPSController.playerHealth == FPSController.playerStatsSO.m_MaxPlayerHealth)
        {
            print("Max Health");
            return;
        }
        else
        {
            FPSController.playerHealth += _healthCapacity;
            PlayerUIManager.playerHealthUpdate?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
