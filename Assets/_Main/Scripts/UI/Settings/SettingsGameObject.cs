using Atomic.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsGameObject : MonoBehaviour
{
    [SerializeField]
    private RightGroupSettingsView rightGroupSettingsView;

    [SerializeField]
    private LeftGroupSettingsView leftGroupSettingsView;

    [SerializeField]
    private BottomGroupSettingsView bottomGroupSettingsView;

    public void OnStart()
    {
        SettingsMini mini = new SettingsMini(rightGroupSettingsView, leftGroupSettingsView, bottomGroupSettingsView);
        mini.Initialize();
    }
}
