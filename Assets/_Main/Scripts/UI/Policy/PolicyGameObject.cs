using Doozy.Runtime.UIManager.Containers;
using RMC.Core.Architectures.Mini.Context;
using System;
using UnityEngine;

namespace Atomic.UI
{
    public class PolicyGameObject : MonoBehaviour
    {
        [SerializeField] 
        private UIPopup _policyPopup;

        public void OnEnable()
        {
            Context context = new();

            PolicyValidationMini validationMini = new(context);
            PolicyDisplayMini displayMini = new(_policyPopup, context);
            
            validationMini.Initialize();
            displayMini.Initialize();
        }

    }

}
