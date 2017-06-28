using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour {
    public int TimeScale = 45;
    // Use this for initialization
    private Text clockText;
    private Text dayText;
    private Text seasonText;
    private Text yearText;
    private double minute, hour, day, second, month, year;
    private Material skybox;
	void Start () {
        month = 1;
        day = 1;
        year = 2077;
        skybox = Camera.main.gameObject.GetComponent<Skybox>().material;
        clockText = GameObject.Find("Hour").GetComponent<Text>();
        dayText = GameObject.Find("Day").GetComponent<Text>();
        seasonText = GameObject.Find("Season").GetComponent<Text>();
        yearText = GameObject.Find("Year").GetComponent<Text>();
        CalculateSeason();

    }

    // Update is called once per frame
    void Update () {
        CalculateTime();
	}

    void updateLight()
    {
        Debug.Log(skybox);
        if (hour>=5 && hour <= 11)
        {
            float atmosphereThickness = 0.2f+(float)((hour-5)*60+minute)/6/60*0.64f;
            float exposure = (float)((hour - 5) * 60 + minute) / 60/6 * 2.5f;
            if (skybox)
            {
                skybox.SetFloat("_Exposure", exposure);
                skybox.SetFloat("_AtmosphereThickness", atmosphereThickness);
                //Debug.Log("exposure:" + skybox.GetFloat("_Exposure"));
                //Debug.Log("atmosphere thickness:" + skybox.GetFloat("_AtmosphereThickness"));

            }
        }
        //if (hour >= 12 && hour <= 15)
        //{
        //    float atmosphereThickness = 0.84f + (float)((hour-12)*60+minute) / 4/60;
        //    float exposure = 2.5f - (float)((hour-12)*60+minute) / 4/60 * 2f;
        //    if (skybox)
        //    {
        //        skybox.SetFloat("_Exposure", exposure);
        //        //skybox.SetFloat("_AtmosphereThickness", atmosphereThickness);
        //        //Debug.Log("exposure:" + skybox.GetFloat("_Exposure"));
        //        //Debug.Log("atmosphere thickness:" + skybox.GetFloat("_AtmosphereThickness"));

        //    }
        //}
        if (hour >= 16 && hour <= 19)
        {
            float atmosphereThickness = 0.84f + (float)((hour - 16) * 60 + minute) / 4 / 60*0.5f;
            float exposure = 2.5f - (float)((hour-16)*60+minute) / 4 /60* 2.5f;
            if (skybox)
            {
                skybox.SetFloat("_Exposure", exposure);
                skybox.SetFloat("_AtmosphereThickness", atmosphereThickness);
                //Debug.Log("exposure:" + skybox.GetFloat("_Exposure"));
                //Debug.Log("atmosphere thickness:" + skybox.GetFloat("_AtmosphereThickness"));

            }
        }
    }
    void TextCallFunction()
    {
        dayText.text = "Day: " + day;
        clockText.text = "Time: " + hour + ":" + minute;
        yearText.text = "Year: " + year;
    }
    private void CalculateSeason()
    {
        if (month >= 6 && month <= 8)
        {
            seasonText.text = "Summer";
        }
        if (month>=9 && month <= 11)
        {
            seasonText.text = "Fall";
        }
        if (month == 12 || month <= 2)
        {
            seasonText.text = "Winter";
        }
        if (month >= 3 && month <= 5)
        {
            seasonText.text = "Spring";
        }

    }

    void CalculateMonth()
    {

    }

    void CalculateTime()
    {
        second += Time.deltaTime * TimeScale;
        if (second >= 60)
        {
            updateLight();
            minute++;
            second = 0;
            TextCallFunction();
        }
        else if (minute >= 60)
        {
            hour++;
            minute = 0;
            TextCallFunction();
        }
        else if (hour >= 24)
        {
            day++;
            hour = 0;
            TextCallFunction();
        }
        else if (day >= 30)
        {
            month++;
            day = 1;
            TextCallFunction();
            CalculateSeason();

        }
        else if (month >= 12)
        {
            month = 1;
            year++;
            TextCallFunction();
            CalculateSeason();
        }
    }
}
