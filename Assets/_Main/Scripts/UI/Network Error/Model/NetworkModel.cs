using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;



namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// </summary>
    public class NetworkModel : BaseModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<bool> NetworkConnetion
        {
            get { return _networkConnetion; }
        }

        public bool IsNetworkConntected
        {
            set
            {
                _networkConnetion.Value = value;
            }
        }

        //  Fields ----------------------------------------
        private Observable<bool> _networkConnetion = new();

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            IsNetworkConntected = true;

        }

        
        //  Other Methods ---------------------------------


    }
}


