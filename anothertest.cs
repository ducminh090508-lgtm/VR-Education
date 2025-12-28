using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRUIButtonToggle : MonoBehaviour
{
    [Tooltip("The XR Interactable component on your 3D button.")]
    public XRBaseInteractable interactable;

    [Tooltip("The world-space UI panel you want to toggle.")]
    public GameObject uiPanel;

    private void OnEnable()
    {
        // Subscribe to the Activate event
        interactable.activated.AddListener(OnButtonActivated);
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        interactable.activated.RemoveListener(OnButtonActivated);
    }

    private void OnButtonActivated(ActivateEventArgs args)
    {
        // Toggle the panel’s active state
        uiPanel.SetActive(!uiPanel.activeSelf);
    }
}