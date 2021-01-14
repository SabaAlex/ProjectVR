using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeSky : MonoBehaviour
{
    public Material newSkyboxMaterial;

    // Elapsed time
    private float time;

    // Skybox changed trigger point
    private bool skyboxChanged;

    void Start()
    {
        time = 0.0f;

        skyboxChanged = false;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 8.0f && !skyboxChanged)
        {
            RenderSettings.skybox = newSkyboxMaterial;
            skyboxChanged = true;
        }
    }
}
