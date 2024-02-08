using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Atomic.UI
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Data transfer object (DTO) containing all
    /// info needed to request a login.
    /// </summary>
    public class UserData
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        public string Name { get; set; }
        public int Age { get; set; }

        //  Initialization  -------------------------------

        public UserData(string name, int age)
        {
            Name = name;
            Age = age;
        }

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------

    }
}

