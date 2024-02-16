using Doozy.Runtime.UIManager.Components;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.View;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Atomic.Template
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class GuestSignUpView : MonoBehaviour, IView
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public IContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        //  Fields ----------------------------------------
        [SerializeField]
        private UIButton _buttonOK;

        [SerializeField]
        private TMP_InputField _inputField;
        
        private bool _isInitialized;
        private IContext _context;


        //  Dependencies ----------------------------------


        //  Initialization  -------------------------------
        public void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                _isInitialized = true;
                _context = context;


            }
        }

        public void RequireIsInitialized()
        {
            if(!IsInitialized)
            {
                throw new System.Exception("GuestSignUpView not yet initialized");
            }
        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


    }
}