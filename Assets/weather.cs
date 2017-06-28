using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weather : MonoBehaviour {
    public Material Rain;
    public Material Snow;
    public Material Hail;
    public float timeInterval = 60;
    private float time = 0.0f;
    private ParticleSystemRenderer render;
    private ParticleSystem ps;
    private string currentWeather = "rain";
	// Use this for initialization
	void Start () {
        ps  = this.GetComponent<ParticleSystem>();
        render = this.GetComponent<ParticleSystemRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= timeInterval)
        {
            Debug.Log("Weather changing");
            var ma = ps.main;
            time = time - timeInterval;
            if (currentWeather == "rain")
            {
                ma.gravityModifier = 0.25f;
                render.material = Snow;
                ma.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.25f);
                currentWeather = "snow";
                Debug.Log("changed to Snow");
            }
            else if (currentWeather == "snow")
            {
                ma.gravityModifier = 1f;
                render.material = Rain;
                ma.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.5f);
                currentWeather = "rain";
                Debug.Log("changed to Rain");
            }
        }
    }
}
