using UnityEngine;

public class SkyBoxController : MonoBehaviour {

	public float cloudsSpeed;

    void Update()
	{
		RenderSettings.skybox.SetFloat("_Rotation", Time.deltaTime * cloudsSpeed);
	}
}
