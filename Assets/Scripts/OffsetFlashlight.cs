using Unity.Tutorials.Editor;
using UnityEngine;

public class OffsetFlashlight : MonoBehaviour
{
    [SerializeField] GameObject flashlight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeSelf);
        }
        if (flashlight.activeSelf == true)
        {
              GameObject.Find("directional light").SetActive(true);
        }
        else if (flashlight.activeSelf == false)
        {
            GameObject.Find("directional light").SetActive(false);
        }
    }
}
