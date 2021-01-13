using UnityEngine;

public class SkyBoxControler : MonoBehaviour {

	public  float CloudsSpeed;
    public Texture Texture;

    void Start()
    {
        CrossFadeSky bt = this.GetComponent<CrossFadeSky>();
        bt.CrossFadeTo(Texture);
    }

    void Update()
	{
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * CloudsSpeed);
	}
}
