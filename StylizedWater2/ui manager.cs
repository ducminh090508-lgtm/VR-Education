// UIEventManager.cs
using System;

public static class UIEventManager
{
    public static event Action OnShowUI, OnHideUI, OnToggleUI;
    public static void ShowUI()   => OnShowUI?.Invoke();
    public static void HideUI()   => OnHideUI?.Invoke();
    public static void ToggleUI() => OnToggleUI?.Invoke();
}