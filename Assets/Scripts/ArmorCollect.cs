using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorCollect : MonoBehaviour
{
    //public delegate void OnArmorCollect();
    //public static OnArmorCollect onArmorCollect;

    //[SerializeField] private ArmorSO armorSO;

    RigidBodyFPSController FPSController;
    float _armorCapacity = 50;

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
        FPSController.playerArmor += _armorCapacity;
        Destroy(this.gameObject);
    }

    //private void Awake()
    //{
    //    onArmorCollect = CollectItem;
    //}
}
