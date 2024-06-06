﻿#if ENABLE_PLAYFABADMIN_API
using CBS.Models;
using CBS.Scriptable;
using CBS.SharedData;
using UnityEditor;
using UnityEngine;

namespace CBS.Editor
{
    public class AuthConfigurator : BaseConfigurator
    {
        protected override string Title => "Auth Configurator";

        protected override bool DrawScrollView => true;

        private AuthData AuthData { get; set; }

        public override void Init(MenuTitles title)
        {
            base.Init(title);
            AuthData = CBSScriptable.Get<AuthData>();
        }

        protected override void OnDrawInside()
        {
            var titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.fontSize = 16;

            GUILayout.Space(10);
            EditorGUILayout.LabelField("General options", titleStyle);
            GUILayout.Space(10);
            // autogenerate nick name
            bool autoGen = EditorGUILayout.Toggle("AutoCreate Display Name", AuthData.AutoGenerateRandomNickname);
            string nickNamePrefix = AuthData.RandomNamePrefix;
            if (autoGen)
            {
                GUILayout.Space(10);
                nickNamePrefix = EditorGUILayout.TextField("Random Name Prefix", AuthData.RandomNamePrefix, new GUILayoutOption[] { GUILayout.Width(400) });
            }
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to automatically generate player nicknames after registration. Does not apply to registration via email", MessageType.Info);
            
            GUILayout.Space(10);
            bool autoLogin = EditorGUILayout.Toggle("Auto Login", AuthData.AutoLogin);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option for the system to save the last successful authorization on the device. Allows you to use the CBSAuthModule.AutoLogin method to log into the game.", MessageType.Info);

            GUILayout.Space(10);
            bool profanityCheck = EditorGUILayout.Toggle("Nickname profanity check?", AuthData.DisplayNameProfanityCheck);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option for the system to enable profanity check for profile display name.", MessageType.Info);

            GUILayout.Space(10);
            var deviceIdProvider = (DeviceIdDataProvider)EditorGUILayout.EnumPopup("Device ID Data Provider", AuthData.DeviceIdProvider);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("The source that provides the deviceID for the login.", MessageType.Info);
            
            GUILayout.Space(10);
            var newPlayerChecker = (NewlyCreatedCheck)EditorGUILayout.EnumPopup("New player check solution", AuthData.NewPlayerCheckSolution);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("PLAYFAB_PROPERTY - Uses PlayFab LoginResult.NewlyCreated property. PROFILE_DATA_PROPERTY - CBS solution that identifies a new player in different titles under the same studio", MessageType.Info);

            GUILayout.Space(10);
            EditorUtils.DrawUILine(Color.grey, 1, 20);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Preload Data options", titleStyle);
            GUILayout.Space(10);
            
            GUILayout.Space(10);
            var loadCatalogType = (LoadCatalogItems)EditorGUILayout.EnumPopup("Preload catalog items", AuthData.LoadItemsType);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Single Call - allows you to load catalog items in one request during login. Separate call - Allows you to load catalog items with a separate request. Best used when itmes size is large and exceeds the allowed limit for a PlayFab request", MessageType.Info);

            bool preloadAccount = EditorGUILayout.Toggle("Preload Account Data", AuthData.PreloadAccountInfo);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to get your account details right after login. For example after login you will have access to the property CBSProfileModule.DisplayName, CBSProfileModule.Avatar.", MessageType.Info);
            GUILayout.Space(10);

            bool preloadLevelData = EditorGUILayout.Toggle("Preload Level Data", AuthData.PreloadLevelData);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to get data about the current level and experience of the player immediately after login. For example, after login, the CBSProfileБщвгду.CachedLevelInfo property will be available to you.", MessageType.Info);
            GUILayout.Space(10);

            bool preloadCurrencies = EditorGUILayout.Toggle("Preload Currencies", AuthData.PreloadCurrency);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to get all player Currencies immediately after login. For example, after login, the CBSCurrencyModule.CacheCurrencies property will be available to you.", MessageType.Info);
            GUILayout.Space(10);

            bool preloadInventory = EditorGUILayout.Toggle("Preload Inventory", AuthData.PreloadInventory);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to get player Inventory immediately after login. For example, after login, the CBSInventoryModule.InventoryCache property will be available to you.", MessageType.Info);
            GUILayout.Space(10);

            bool preloadUserData = EditorGUILayout.Toggle("Preload Profile Data", AuthData.PreloadUserData);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to get player Data immediately after login. For example, after login, the CBSProfileModule.CachedProfileData property will be available to you.", MessageType.Info);
            GUILayout.Space(10);

            bool preloadTitleData = EditorGUILayout.Toggle("Preload Title Data", AuthData.PreloadTitleData);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to get Title Data immediately after login. For example, after login, the CBSTitleDataModule.GetFromCache method will be available to you.", MessageType.Info);
            GUILayout.Space(10);

            bool preloadClan = EditorGUILayout.Toggle("Preload Clan Data", AuthData.PreloadClan);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Enable this option to get Clan Entity immediately after login if profile exist. For example, after login, the CBSProfile.ClanEntity method will be available to you.", MessageType.Info);
            GUILayout.Space(10);

            AuthData.PreloadLevelData = preloadLevelData;
            AuthData.PreloadAccountInfo = preloadAccount;
            AuthData.AutoGenerateRandomNickname = autoGen;
            AuthData.RandomNamePrefix = nickNamePrefix;
            AuthData.PreloadCurrency = preloadCurrencies;
            AuthData.AutoLogin = autoLogin;
            AuthData.DeviceIdProvider = deviceIdProvider;
            AuthData.PreloadInventory = preloadInventory;
            AuthData.PreloadUserData = preloadUserData;
            AuthData.PreloadTitleData = preloadTitleData;
            AuthData.PreloadClan = preloadClan;
            AuthData.DisplayNameProfanityCheck = profanityCheck;
            AuthData.NewPlayerCheckSolution = newPlayerChecker;
            AuthData.LoadItemsType = loadCatalogType;

            AuthData.Save();
        }
    }
}
#endif
