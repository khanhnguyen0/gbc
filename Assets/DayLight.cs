using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLight : MonoBehaviour {
    public float dayTime = 30.0f;
    private Material skybox;
    private float time = 0;
    private GameObject mainCamera;
    // Use this for initialization
    void Start () {
        mainCamera = Camera.main.gameObject;
        skybox = mainCamera.GetComponent<Skybox>().material;

	}
	
	// Update is called once per frame
	void Update () {
        float exposure = Mathf.Sin(Time.time / dayTime * Mathf.Deg2Rad*100) + 1;
        float atmosphereThickness =0.5f+ Mathf.Abs(Mathf.Sin(2*Time.time / dayTime * Mathf.Deg2Rad*100));
        if (skybox)
        {
            skybox.SetFloat("_Exposure", exposure);
            skybox.SetFloat("_AtmosphereThickness", atmosphereThickness);
            //Debug.Log("exposure:" + skybox.GetFloat("_Exposure"));
            //Debug.Log("atmosphere thickness:" + skybox.GetFloat("_AtmosphereThickness"));

        }
    }
}
