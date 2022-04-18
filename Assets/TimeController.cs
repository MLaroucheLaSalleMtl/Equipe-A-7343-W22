using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeController : MonoBehaviour
{
    [SerializeField] private float timeMultiplier;
    [SerializeField] private float starHour;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Light sunLight;
    [SerializeField] private float sunrisehour;
    [SerializeField] private float sunsetHour;

    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private AnimationCurve lightChangeCurve;
    [SerializeField] private float maxSunLightIntesity;
    [SerializeField] private Light moonLight;
    [SerializeField] private float maxMoonLightIntesity;

    private DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTIme;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(starHour);
        sunriseTime = TimeSpan.FromHours(sunrisehour);
        sunsetTIme = TimeSpan.FromHours(sunsetHour);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSuns();
        UpdateLightSetting();

    }

     void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if(timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    void RotateSuns()
    {
        float sunsLightRotation;
        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunriseTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDiffrence(sunriseTime, sunsetTIme);
            TimeSpan timeSinceSunrise = CalculateTimeDiffrence(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunsLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }

        else
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDiffrence(sunsetTIme, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDiffrence(sunsetTIme, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunsLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunsLightRotation, Vector3.right);
    }
    void UpdateLightSetting()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntesity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntesity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    TimeSpan CalculateTimeDiffrence(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan diffrence = toTime - fromTime;
        if(diffrence.TotalSeconds < 0)
        {
            diffrence += TimeSpan.FromHours(24);
        }
        return diffrence;
    }

}
