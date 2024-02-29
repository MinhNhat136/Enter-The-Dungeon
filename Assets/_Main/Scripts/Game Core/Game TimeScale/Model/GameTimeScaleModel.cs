using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;
using UnityEngine;

namespace Atomic.Models
{
    public class GameTimeScaleModel : BaseModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<float> GameTimeScale
        {
            get
            {
                return _gameTimeScale;
            }
        }

        //  Fields ----------------------------------------
        private readonly Observable<float> _gameTimeScale = new();


        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            base.Initialize(context);
            _gameTimeScale.Value = Time.timeScale;

        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------


    }
}



