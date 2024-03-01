using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Model;

namespace Atomic.Models
{
    public enum GameState
    {
        Pause,
        Normal,
    }

    public class GameStateModel : BaseModel
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Observable<GameState> GameState
        {
            get
            {
                return _gameState;
            }
        }

        public void SetGameState(GameState gameState)
        {
            _gameState.Value = gameState;
        }

        //  Fields ----------------------------------------
        private readonly Observable<GameState> _gameState = new();


        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            base.Initialize(context);


        }

        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------


    }
}



