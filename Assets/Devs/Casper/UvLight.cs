using UnityEngine;

public class UVLight : MonoBehaviour
{
    public static UVLight Instance { get; private set; }

    public bool isUVLightOn = false;
    [SerializeField] private Light spotLight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (spotLight == null)
        {
            spotLight = GetComponent<Light>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isUVLightOn = !isUVLightOn;
            if (spotLight != null)
            {
                spotLight.enabled = isUVLightOn;
            }
        }
    }
}