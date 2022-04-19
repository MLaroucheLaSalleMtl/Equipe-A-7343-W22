using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/KeyPickableItems")]
public class KeyItemsSO : ScriptableObject
{
    public static KeyItemsSO instance;   

    public void OnCollect()
    {
        
    }
}
