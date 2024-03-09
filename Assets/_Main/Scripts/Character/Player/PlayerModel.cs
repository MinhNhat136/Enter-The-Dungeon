using RMC.Core.Architectures.Mini.Context;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Character.Player
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerModel : MonoBehaviour, IInitializable
    {
        //  Events ----------------------------------------
        [HideInInspector]
        public UnityEvent<bool> OnStateRunning = new();

        //  Properties ------------------------------------
        public PlayerControls Controls { get; private set; }

        public Vector2 MoveInput;
        public float InputHorizontal
        {
            get
            {
                return MoveInput.x;
            }
        }

        public float InputVertical
        {
            get
            {
                return MoveInput.y;
            }
        }

        public bool IsInitialized => throw new System.NotImplementedException();

        //  Fields ----------------------------------------
        private bool _isInitialized;

        //  Initialization  -------------------------------
        public void Initialize()
        {
            if (!_isInitialized)
            {
                AssignInputEvents();
            }
        }

        public void RequireIsInitialized()
        {
            if (!_isInitialized)
            {
                throw new System.Exception("PlayerModel not initialized");
            }
        }

        //  Unity Methods   -------------------------------
        private void Awake()
        {
            Controls = new PlayerControls();
        }

        private void OnEnable()
        {
            Controls.Enable();
        }

        private void OnDisable()
        {
            Controls.Disable();
        }

        //  Other Methods ---------------------------------
        public void AssignInputEvents()
        {
            Controls.Character.Movement.performed += context =>
            {
                MoveInput = context.ReadValue<Vector2>();
            };
            Controls.Character.Movement.canceled += context =>
            {
                MoveInput = Vector2.zero;
            };

            Controls.Character.Run.started += context =>
            {
                OnStateRunning?.Invoke(true);
            };

            Controls.Character.Run.canceled += context =>
            {
                OnStateRunning?.Invoke(false);
            };
        }
        //  Event Handlers --------------------------------

    }

}
