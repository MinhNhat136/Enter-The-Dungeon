using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.UI
{
    public class SignUpWithGuessModel : BaseModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<UserData> SignedUpUserData { get { return _signedUpUserData; } }
        public bool IsSignedUp { get { return SignedUpUserData.Value != null; } }

        //  Fields ----------------------------------------
        private readonly Observable<UserData> _signedUpUserData = new();


        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);

                // Set Defaults
                _signedUpUserData.Value = null;
            }
        }

        //  Methods ---------------------------------------


        //  Event Handlers --------------------------------

    }
}

