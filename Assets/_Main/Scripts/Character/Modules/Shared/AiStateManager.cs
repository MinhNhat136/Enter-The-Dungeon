using System;
using System.Collections.Generic;
using Atomic.Character.Model;
using Atomic.Core.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Atomic.Character.Module
{
    //  Namespace Properties ------------------------------
    public enum EnvironmentEnum
    {
        OnGrounded, 
        Airborne,
    }
    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class AiStateManager : SerializedMonoBehaviour, IInitializableWithBaseModel<BaseAgent>
    {
        //  Events ----------------------------------------
        public event Action OnDeath;

        //  Properties ------------------------------------
        public EnvironmentEnum CurrentEnvironmentState { get; set; }
        public CombatMode CurrentCombatMode { get; set; }
        
        public bool IsInitialized { get; private set; } = false;
        public BaseAgent Model { get; set; }

        public bool IsMovement=>  !Mathf.Approximately(Model.MotorController.MoveInput.sqrMagnitude, 0f);
        public bool IsRolling { get; set; } = false;
        public bool IsPreparingAttack { get; set; } = false;
        public bool IsAttacking { get; set; } = false;
        public bool IsShot { get; set; } = false;
        public bool IsSwapWeapon { get; set; } = false;

        public bool CanAction { get; set; } = true;
        public bool CanRollAgain { get; set; } = true;
        public bool CanPrepareAttackAgain { get; set; } = true;
        public bool CanAttack { get; set; }
        
        public bool CanMoveNextSkill { get; set; } = true;
        
        //  Collections  ----------------------------------
        [SerializeField]
        private Dictionary<EnvironmentEnum, List<CharacterActionType>> lookUpEnvironment;
        
        
        //  Fields ----------------------------------------

        
        //  Initialization  -------------------------------
        public void Initialize(BaseAgent model)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Model = model;
            }
        }

        public void RequireIsInitialized()
        {
            throw new System.NotImplementedException();
        }

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
   

        //  Event Handlers --------------------------------
        


    }
    
}
