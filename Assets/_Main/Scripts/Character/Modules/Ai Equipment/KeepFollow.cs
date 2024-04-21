using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Atomic.Equipment
{
    public class KeepFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        public void Update()
        {
            this.transform.position = target.transform.position;
        }
    }    
}

