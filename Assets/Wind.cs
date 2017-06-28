using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {
    public float timeInterval = 60.0f;
    private float time = 0.0f;
    private WindZone wind;

	// Use this for initialization
	void Start () {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var fo = ps.forceOverLifetime;
        fo.enabled = true;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        curve.AddKey(0.75f, 1.0f);
        fo.x = new ParticleSystem.MinMaxCurve(1.5f, curve);
}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= timeInterval)
        {
            time -= timeInterval;
            wind.windMain = Random.Range(-30.0f, 30.0f);
        }
	}
}
