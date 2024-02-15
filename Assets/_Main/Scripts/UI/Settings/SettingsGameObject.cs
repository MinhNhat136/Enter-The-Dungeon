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

    private void Start()
    {
        SettingsMini mini = new SettingsMini(rightGroupSettingsView, leftGroupSettingsView, bottomGroupSettingsView);
        mini.Initialize();
    }
}
