using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Attach this script to an empty GameObject in your scene (e.g., "UIManager").
/// Assign the UI panel you want to open/close, the world-space anchor, and the input action that triggers toggling.
/// </summary>
public class VRUIPanelController : MonoBehaviour
{
    [Header("UI Panel")]
    [Tooltip("Drag the UI Panel GameObject here (initially inactive if you want it hidden by default)")]
    public GameObject uiPanel;

    [Header("Anchor Transform")]
    [Tooltip("Drag the Transform (e.g., controller or empty GameObject) where the panel should appear")]
    public Transform panelAnchor;

    [Header("Input Action")]
    [Tooltip("Assign an Input Action (e.g., XR controller button) for toggling the panel")]
    public InputActionReference togglePanelAction;

    [Header("Optional Offset")]
    [Tooltip("Local position offset from the anchor")]
    public Vector3 positionOffset = Vector3.zero;
    [Tooltip("Local rotation offset from the anchor (Euler angles)")]
    public Vector3 rotationOffset = Vector3.zero;

    private void OnEnable()
    {
        if (togglePanelAction?.action != null)
        {
            togglePanelAction.action.performed += OnTogglePanel;
            togglePanelAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (togglePanelAction?.action != null)
        {
            togglePanelAction.action.performed -= OnTogglePanel;
            togglePanelAction.action.Disable();
        }
    }

    private void OnTogglePanel(InputAction.CallbackContext context)
    {
        if (uiPanel == null || panelAnchor == null)
            return;

        // Toggle visibility
        bool isActive = !uiPanel.activeSelf;
        uiPanel.SetActive(isActive);

        if (isActive)
        {
            // Position & rotate the panel at the anchor
            uiPanel.transform.SetPositionAndRotation(
                panelAnchor.position + panelAnchor.TransformVector(positionOffset),
                panelAnchor.rotation * Quaternion.Euler(rotationOffset)
            );
        }
    }
}
