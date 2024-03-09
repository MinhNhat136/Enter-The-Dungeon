using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Character.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent<bool> OnStateRunning = new();

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
    }

}
