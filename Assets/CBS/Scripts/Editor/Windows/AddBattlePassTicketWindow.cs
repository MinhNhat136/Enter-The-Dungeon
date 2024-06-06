﻿using CBS.Models;
using CBS.Utils;
using System;
using UnityEditor;
using UnityEngine;

namespace CBS.Editor.Window
{
    public class AddBattlePassTicketWindow : EditorWindow
    {
        private static Action<BattlePassTicket> AddCallback { get; set; }
        private static string BattlePassID { get; set; }

        private string EntereID { get; set; }
        private string EnteredName { get; set; }

        public static void Show(string battlePassID, Action<BattlePassTicket> modifyCallback)
        {
            AddCallback = modifyCallback;
            BattlePassID = battlePassID;

            AddBattlePassTicketWindow window = ScriptableObject.CreateInstance<AddBattlePassTicketWindow>();
            window.maxSize = new Vector2(400, 700);
            window.minSize = window.maxSize;
            window.ShowUtility();
        }

        private void Hide()
        {
            this.Close();
        }

        void OnGUI()
        {
            using (var areaScope = new GUILayout.AreaScope(new Rect(0, 0, 400, 700)))
            {
                GUILayout.Space(10);

                GUILayout.BeginVertical();

                var titleStyle = new GUIStyle(GUI.skin.label);
                titleStyle.alignment = TextAnchor.MiddleCenter;
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.fontSize = 18;
                EditorGUILayout.LabelField("Add new BattlePass Ticket", titleStyle);

                GUILayout.Space(30);

                GUILayout.Label("Ticket ID", GUILayout.Width(120));
                EntereID = GUILayout.TextField(EntereID);

                GUILayout.Space(5);

                EditorGUILayout.HelpBox("id cannot contain special characters (/*-+_@&$#%)", MessageType.Info);

                GUILayout.Space(5);

                GUILayout.Label("Display Name", GUILayout.Width(120));
                EnteredName = GUILayout.TextField(EnteredName);

                GUILayout.EndVertical();

                GUILayout.Space(30);

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Save"))
                {
                    if (string.IsNullOrEmpty(EntereID) || string.IsNullOrEmpty(EnteredName))
                        return;
                    if (TextUtils.ContainSpecialSymbols(EntereID))
                        return;
                    var newInstance = new BattlePassTicket
                    {
                        ID = EntereID,
                        BattlePassID = BattlePassID,
                        ExpMultiply = 1f
                    };
                    newInstance.DisplayName = EnteredName;
                    AddCallback?.Invoke(newInstance);
                    Hide();
                }
                if (GUILayout.Button("Close"))
                {
                    Hide();
                }

                GUILayout.EndVertical();
            }
        }
    }
}
