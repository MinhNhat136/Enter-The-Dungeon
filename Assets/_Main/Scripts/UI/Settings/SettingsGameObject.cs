using Atomic.UI;
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
        SettingsMini mini = new(rightGroupSettingsView, leftGroupSettingsView, bottomGroupSettingsView);
        mini.Initialize();
    }
}
