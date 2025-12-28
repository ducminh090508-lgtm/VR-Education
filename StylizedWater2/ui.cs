using UnityEngine;

[RequireComponent(typeof(Canvas))]  // or any UI root
public class UIController : MonoBehaviour
{
    [Tooltip("Assign the root GameObject (Panel, Canvas, etc.) you want to show/hide")]
    public GameObject uiPanel;

    private void OnEnable()
    {
        UIEventManager.OnShowUI   += Show;
        UIEventManager.OnHideUI   += Hide;
        UIEventManager.OnToggleUI += Toggle;
    }

    private void OnDisable()
    {
        UIEventManager.OnShowUI   -= Show;
        UIEventManager.OnHideUI   -= Hide;
        UIEventManager.OnToggleUI -= Toggle;
    }

    private void Show()
    {
        if (uiPanel != null)
            uiPanel.SetActive(true);
    }

    private void Hide()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    private void Toggle()
    {
        if (uiPanel != null)
            uiPanel.SetActive(!uiPanel.activeSelf);
    }
}