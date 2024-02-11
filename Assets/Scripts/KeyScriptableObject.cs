using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Inventory/Key")]
public class KeyScriptableObject : Item
{
    [SerializeField] private long keyCode;

    // Update is called once per frame
    void Update()
    {
        
    }
}
