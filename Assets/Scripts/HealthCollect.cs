using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollect : MonoBehaviour
{
    //public delegate void OnHealthCollect();
    //public static OnHealthCollect onHealthCollect;

    //[SerializeField] private HealthPackSO healthPackSO;

    RigidBodyFPSController FPSController;
    float _healthCapacity = 50;

    private void Awake()
    {
        FPSController = FindObjectOfType<RigidBodyFPSController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        FPSController.playerHealth += _healthCapacity;
        Destroy(this.gameObject);
    }

    //public void CollectItem()
    //{
    //    healthPackSO.OnCollect();
    //}

    //private void Awake()
    //{
    //    onHealthCollect = CollectItem;
    //}
}
