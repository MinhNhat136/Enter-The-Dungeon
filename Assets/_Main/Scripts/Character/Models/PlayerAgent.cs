using Atomic.Character.Module;
using UnityEngine;

namespace Atomic.Character.Model
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerAgent : BaseAgent
    {
        //  Events ----------------------------------------
        [HideInInspector] public bool isAttack;
        [HideInInspector] public bool isRolling;
        [HideInInspector] public bool canAttack;

        protected bool isInAttack;                     
        protected bool isInCombo;                      

        //  Properties ------------------------------------
        public PlayerControls InputControls 
        { 
            get; private set; 
        }

        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(MotorController.LocomotionController.MoveInput.sqrMagnitude, 0f); }
        }


        //  Fields ----------------------------------------

        private PlayerAnimatorController _animatorController;

        //  Initialization  -------------------------------
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                base.Initialize();

                InputControls = new PlayerControls();
                Assign();
            }
        }

        //  Unity Methods   -------------------------------
        public override void DoEnable()
        {
            base.DoEnable();
            InputControls.Enable();
        }

        public override void DoDisable()
        {
            base.DoDisable();
            InputControls.Disable();

        }

        //  Other Methods ---------------------------------
        public override void Assign()
        {
            _animatorController = GetComponent<PlayerAnimatorController>();

            AssignInputEvents();

        }


        public void AssignInputEvents()
        {
            InputControls.Character.Movement.performed += context =>
            {
                MotorController.LocomotionController.MoveInput = context.ReadValue<Vector2>();
            };
            InputControls.Character.Movement.canceled += context =>
            {
                MotorController.LocomotionController.MoveInput = Vector2.zero;
            };
            InputControls.Character.Roll.performed += context =>
            {
                isRolling = true;
            };
            InputControls.Character.Roll.canceled += context => isRolling = false; 

            InputControls.Character.Attack.performed += context =>
            {
                isAttack = true;
            };

            InputControls.Character.Attack.canceled += context =>
            {
                isAttack = false;
            };
        }

        public void BindingAction()
        {
            IsInVulnerable = _animatorController.IsRolling;
        }

        //  Event Handlers --------------------------------

    }

}
