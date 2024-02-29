using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;



namespace Atomic.Models
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// </summary>
    public class NetworkStateModel : BaseModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<bool> GetConnectionStatus
        {
            get { return _networkConnetion; }
        }

        public bool SetConnectionStatus
        {
            set
            {
                _networkConnetion.Value = value;
            }
        }

        //  Fields ----------------------------------------
        private readonly Observable<bool> _networkConnetion = new();

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            SetConnectionStatus = true;
        }

        
        //  Other Methods ---------------------------------


    }
}


