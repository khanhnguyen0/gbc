using UnityEngine;
using System.Collections;
using System;

public class LightCycle : MonoBehaviour
{
    private float _lastTime;

    [Header("Time")]
    [SerializeField]
    private float _time;
    [SerializeField]
    private float _duration;

    [Header("Sun, Moon & Stars")]
    [SerializeField]
    private Vector3 _sunAxis;
    [SerializeField]
    private float _orbitDistance;
    [SerializeField]
    private AnimationCurve _ambientSunIntensity;
    [SerializeField]
    private AnimationCurve _starVisibility;
    [SerializeField]
    private Gradient _ambientLightGradient;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _sun;
    [SerializeField]
    private GameObject _moon;
    [SerializeField]
    private SpriteRenderer _stars;
    [SerializeField]
    private Light _sunLght;
    [SerializeField]
    private Light _ambientLight;

	// Use this for initialization
	void Start ()
    {
        _time = -0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        _lastTime = _time;
        _time += Time.deltaTime;

        // Reset the time for the day cycle
        if (_time >= _duration)
        {
            _time = 0.0f;
        }

        // TODO: Time isnt properly set, time starts at dawn rather than midnight
        float percentage = _time / _duration;
        float timeModifier = ((percentage * 360.0f) + 90.0f) * Mathf.Deg2Rad;

        // Calculate the positions in the sky for the sun & moon
        Vector3 sunPos = pointOnOrbit(percentage * Mathf.Deg2Rad * 360.0f);
        Vector3 moonPos = pointOnOrbit((percentage + 0.5f) * Mathf.Deg2Rad * 360.0f);

        _sun.transform.position = sunPos;
        _moon.transform.position = moonPos;

        // Calculate the skybox light angle
        Vector3 rot = _sunLght.transform.eulerAngles;
        _sunLght.transform.rotation = Quaternion.AngleAxis(percentage * 360.0f, _sunAxis);
        _sunLght.transform.Rotate(new Vector3(0, -90, 0), Space.World);

        // Set the world light level
        _ambientLight.intensity = _ambientSunIntensity.Evaluate(percentage);

        // Determine how visible the stars are
        _stars.color = new Color(1, 1, 1, _starVisibility.Evaluate(percentage));
        _stars.transform.rotation = Quaternion.Euler(0, 0, percentage * 360.0f);

        if (percentage < 0.55f) // Day time
        {
            if (!_sun.activeSelf)
            {
                _sun.SetActive(true);
                _moon.SetActive(false);
            }
        }
        else // Night time
        {
            if (!_moon.activeSelf)
            {
                _moon.SetActive(true);
                _sun.SetActive(false);
            }
        }

        // Adjust the light colour depending on time
        _ambientLight.color = _ambientLightGradient.Evaluate(percentage);
        _sunLght.color = _ambientLightGradient.Evaluate(percentage);
	}

    private DateTime getTime()
    {
        // TODO: Adjust time to start at midnight rather than sunrise
        //.
        float percentage = _time / _duration;

        float modHours = 1.0f / 24.0f;
        float modMinutes = modHours / 60.0f;

        float hours = percentage / modHours;
        float minutes = (hours - (Mathf.Floor(hours))) / modMinutes;

        float mod = 1.0f / 1440.0f;

        TimeSpan span = new TimeSpan(0, 0, (int)(percentage / mod));

        GUILayout.Label("Time: " + span);

        return DateTime.Now;//new DateTime(1, 1, 1, Mathf.FloorToInt(hours), Mathf.FloorToInt(minutes),1);
    }
    
    void OnGUI()
    {
        //getTime();
       // GUILayout.Label("Time: " + getTime());
    }

    /// <summary>
    /// Get a point along the orbit of the parent body, orbits are always circular
    /// </summary>
    /// <returns></returns>
    private Vector3 pointOnOrbit(float angleAround)
    {
        Vector3 origin = Camera.main.transform.position;

        float x = origin.x + _orbitDistance * Mathf.Cos(angleAround);
        float y = origin.y + _orbitDistance * Mathf.Sin(angleAround);

        return new Vector3(x, y, 15);
    }

}
