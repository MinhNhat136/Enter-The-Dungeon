namespace Atomic.Models
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Data transfer object (DTO) containing all
    /// info needed to request a login.
    /// </summary>
    public class UserProfileData
    {
        //  Events ----------------------------------------

        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        public string Name { get; set; }

        //  Initialization  -------------------------------

        public UserProfileData(string name)
        {
            Name = name;
        }

        //  Methods ---------------------------------------

        //  Event Handlers --------------------------------

    }
}

