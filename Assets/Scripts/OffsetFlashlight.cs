using UnityEngine;
using UnityEngine.InputSystem;

public class OffsetFlashlight : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject directionalLight;
    [SerializeField] private InputActionProperty triggerAction;

    private void OnEnable()
    {
        if (triggerAction.action != null)
            triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        if (triggerAction.action != null)
            triggerAction.action.Disable();
    }

    private void Start()
    {
        SetFlashlightState(false);
    }

    private void Update()
    {
        if (triggerAction.action != null && triggerAction.action.WasPressedThisFrame())
        {
            Debug.Log("XR trigger pressed");
            if (flashlight != null)
            {
                SetFlashlightState(!flashlight.activeSelf);
            }
        }
    }

    private void SetFlashlightState(bool isOn)
    {
        if (flashlight != null)
            flashlight.SetActive(isOn);

        if (directionalLight != null)
            directionalLight.SetActive(isOn);
    }
}