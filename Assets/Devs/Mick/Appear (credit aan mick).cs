using System.Collections;
using UnityEngine;

public class Appear : MonoBehaviour
{
    //---DissolveVars---//
    [SerializeField] private float dissolveDuration = 2f;

    private Material dissolveMaterial;
    private Coroutine currentRoutine;
    private bool lastUVLightState;

    //---Unity Methods---//
    void Awake()
    {
        dissolveMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (UVLight.Instance == null) return;

        bool isUVActive = UVLight.Instance.isUVLightOn;

        if (isUVActive != lastUVLightState)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }

            currentRoutine = StartCoroutine(
                isUVActive ? ReAppear() : Disappear()
            );

            lastUVLightState = isUVActive;
        }
    }

    //---Appear / Disappear---//
    private IEnumerator Disappear()
    {
        float elapsed = 0f;

        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            dissolveMaterial.SetFloat("_DissolveStrength", Mathf.Clamp01(elapsed / dissolveDuration));
            yield return null;
        }

        dissolveMaterial.SetFloat("_DissolveStrength", 1f);
    }

    private IEnumerator ReAppear()
    {
        float elapsed = dissolveDuration;

        while (elapsed > 0f)
        {
            elapsed -= Time.deltaTime;
            dissolveMaterial.SetFloat("_DissolveStrength", Mathf.Clamp01(elapsed / dissolveDuration));
            yield return null;
        }

        dissolveMaterial.SetFloat("_DissolveStrength", 0f);
    }
}