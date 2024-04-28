using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingBoard : MonoBehaviour
{

    public LayerMask mask;
    
    private void OnTriggerEnter(Collider other)
    {
        if ((mask |= other.gameObject.layer) == mask)
        {
            Debug.Log(Time.time);
        }
    }
}
