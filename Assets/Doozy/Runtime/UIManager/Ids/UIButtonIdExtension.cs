// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Components
{
    public partial class UIButton
    {
        public static IEnumerable<UIButton> GetButtons(UIButtonId.Login id) => GetButtons(nameof(UIButtonId.Login), id.ToString());
        public static bool SelectButton(UIButtonId.Login id) => SelectButton(nameof(UIButtonId.Login), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Settings id) => GetButtons(nameof(UIButtonId.Settings), id.ToString());
        public static bool SelectButton(UIButtonId.Settings id) => SelectButton(nameof(UIButtonId.Settings), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Share id) => GetButtons(nameof(UIButtonId.Share), id.ToString());
        public static bool SelectButton(UIButtonId.Share id) => SelectButton(nameof(UIButtonId.Share), id.ToString());
        public static IEnumerable<UIButton> GetButtons(UIButtonId.Splash id) => GetButtons(nameof(UIButtonId.Splash), id.ToString());
        public static bool SelectButton(UIButtonId.Splash id) => SelectButton(nameof(UIButtonId.Splash), id.ToString());
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIButtonId
    {
        public enum Login
        {
            Facebook,
            GameCenter,
            Google,
            Guest
        }

        public enum Settings
        {
            About,
            Connect,
            Language,
            Like,
            Logout,
            OtherGames,
            Rate,
            UserID
        }

        public enum Share
        {
            Back,
            OK,
            TermsOfService
        }
        public enum Splash
        {
            TapToStart
        }    
    }
}
