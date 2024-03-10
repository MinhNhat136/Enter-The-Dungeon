using Atomic.Character.Model;
using Atomic.Character.Module;
using Atomic.Core.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Atomic.Character.Player
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class PlayerAgent : BaseAgent
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------
        public PlayerControls Controls { get; private set; }
        public AiVisionSensorSystem VisionSensor { get; private set; }
        public BaseAgent TargetAgent { get; set; }

        //  Fields ----------------------------------------


        //  Initialization  -------------------------------


        //  Unity Methods   -------------------------------
        private new void Awake()
        {
            base.Awake();
            Controls = new PlayerControls();
            VisionSensor = GetComponent<AiVisionSensorSystem>();
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
        

        //  Event Handlers --------------------------------

    }

}
