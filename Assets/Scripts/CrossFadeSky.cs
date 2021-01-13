using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeSky : MonoBehaviour
{
    private Texture newTexture;

    public float BlendSpeed = 3.0f;

    private bool trigger = false;
    private float fader = 1f;

    void Start()
    {

    }

    void Update()
    {
        if (true == trigger)
        {
            fader += Time.deltaTime * BlendSpeed;

            RenderSettings.skybox.SetFloat("_Blend", fader);

            if (fader >= 1.0f)
            {
                trigger = false;
                fader = 0f;

                RenderSettings.skybox.SetTexture("_MainTex", newTexture);
                RenderSettings.skybox.SetFloat("_Blend", 0f);
            }
        }
    }

    public void CrossFadeTo(Texture curTexture)
    {
        newTexture = curTexture;
        RenderSettings.skybox.SetTexture("_Texture2", curTexture);
        trigger = true;
    }
}
