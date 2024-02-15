using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace Atomic.Models
{
    public class GuestSignUpModel : BaseModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<UserProfileData> SignedUpUserData { get { return _signedUpUserData; } }
        public bool IsSignedUp { get { return SignedUpUserData.Value != null; } }

        //  Fields ----------------------------------------
        private readonly Observable<UserProfileData> _signedUpUserData = new();


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

