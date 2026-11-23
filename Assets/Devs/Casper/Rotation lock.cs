using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRPhysicalKeyLock : MonoBehaviour
{
    [Header("Key & Lock References")]
    public XRGrabInteractable keyInteractable;
    public Transform keyPivot;

    [Header("Turn Settings")]
    [Tooltip("Degrees needed to unlock (e.g., 90 or 180. Use negative for counter-clockwise).")]
    public float targetAngle = 90f;
    public float unlockTolerance = 5f;
    public bool springBackOnRelease = true;
    public float springReturnSpeed = 180f;

    [Header("Events")]
    public UnityEvent onUnlocked;

    private Transform _grabbingHand;
    private Vector3 _lastProjectedHandUp;
    private float _currentAngle;
    private bool _isUnlocked;

    private void OnEnable()
    {
        if (keyInteractable != null)
        {
            keyInteractable.selectEntered.AddListener(OnKeyGrabbed);
            keyInteractable.selectExited.AddListener(OnKeyReleased);
        }
    }

    private void OnDisable()
    {
        if (keyInteractable != null)
        {
            keyInteractable.selectEntered.RemoveListener(OnKeyGrabbed);
            keyInteractable.selectExited.RemoveListener(OnKeyReleased);
        }
    }

    private void OnKeyGrabbed(SelectEnterEventArgs args)
    {
        if (_isUnlocked) return;

        // Lock position/rotation tracking so the player twists the key in-place
        keyInteractable.trackPosition = false;
        keyInteractable.trackRotation = false;

        _grabbingHand = args.interactorObject.transform;
        _lastProjectedHandUp = Vector3.ProjectOnPlane(_grabbingHand.up, transform.forward).normalized;
    }

    private void OnKeyReleased(SelectExitEventArgs args)
    {
        _grabbingHand = null;
    }

    private void Update()
    {
        if (keyPivot == null) return;

        if (_grabbingHand != null && !_isUnlocked)
        {
            Vector3 currentProjectedUp = Vector3.ProjectOnPlane(_grabbingHand.up, transform.forward).normalized;

            if (currentProjectedUp.sqrMagnitude > 0.001f && _lastProjectedHandUp.sqrMagnitude > 0.001f)
            {
                float deltaAngle = Vector3.SignedAngle(_lastProjectedHandUp, currentProjectedUp, transform.forward);
                _currentAngle += deltaAngle;

                // Clamp between 0 and targetAngle (supports both positive and negative target angles)
                float minAngle = Mathf.Min(0f, targetAngle);
                float maxAngle = Mathf.Max(0f, targetAngle);
                _currentAngle = Mathf.Clamp(_currentAngle, minAngle, maxAngle);

                _lastProjectedHandUp = currentProjectedUp;
            }

            if (Mathf.Abs(_currentAngle - targetAngle) <= unlockTolerance)
            {
                _currentAngle = targetAngle;
                _isUnlocked = true;
                onUnlocked?.Invoke();
            }
        }
        else if (!_isUnlocked && springBackOnRelease && Mathf.Abs(_currentAngle) > 0.01f)
        {
            // Gently return key to 0 degrees if released before fully turned
            _currentAngle = Mathf.MoveTowards(_currentAngle, 0f, springReturnSpeed * Time.deltaTime);
        }

        // Apply rotation strictly around the keyhole's local Z-axis
        keyPivot.localRotation = Quaternion.Euler(0f, 0f, _currentAngle);
    }
}