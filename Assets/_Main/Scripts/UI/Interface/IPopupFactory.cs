using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using VContainer;

public interface IPopupFactory
{
    IPopupView CreatePopup(GameObject popupPrefab);
}
