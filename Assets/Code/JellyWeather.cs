using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JellyWeather : MonoBehaviour
{
    public SpriteRenderer BackgroundObject, NightTimeObject, CloudObject, ForegroundObject;

    public Sprite[] BackgroundsSprite;
    public Sprite[] CloudSprite;
    public Sprite[] ForegroundSprite;

    private string Uri = "api.openweathermap.org/data/2.5/weather?q=Halifax&appid=d3a37f607e2208f6bedd2050f20b7075";

    void Start()
    {
        StartCoroutine(UpdateWeather());
    }

    IEnumerator UpdateWeather()
    {
        UnityWebRequest request = UnityWebRequest.Get(Uri);
        yield return request.SendWebRequest();
        string json = request.downloadHandler.text;
        RootObject currWeatherObj = JsonUtility.FromJson<RootObject>(json);
        SetCurrentWeather(currWeatherObj.weather[0].main);
    }

    //https://stackoverflow.com/questions/1504494/find-if-current-time-falls-in-a-time-range
    void SetCurrentWeather(string weather)
    {
        TimeSpan start = TimeSpan.Parse("20:00"); // 8 PM
        TimeSpan end = TimeSpan.Parse("07:00");   // 7 AM
        TimeSpan now = DateTime.Now.TimeOfDay;
        
        if (start <= end)
        {
            // start and stop times are in the same day
            if (now >= start && now <= end)
            {
                // current time is between start and stop
                NightTimeObject.enabled = true;
            }
        }
        else
        {
            // start and stop times are in different days
            if (now >= start || now <= end)
            {
                // current time is between start and stop
                NightTimeObject.enabled = true;
            }
        }

        weather = weather.ToLower();
        switch (weather)
        {
            case "thunderstorm":
                BackgroundObject.sprite = BackgroundsSprite[1];
                CloudObject.sprite = CloudSprite[1];
                ForegroundObject.sprite = ForegroundSprite[0];
                NightTimeObject.enabled = true;
                break;
            case "drizzle":
                BackgroundObject.sprite = BackgroundsSprite[1];
                CloudObject.sprite = CloudSprite[0];
                ForegroundObject.sprite = ForegroundSprite[0];
                break;
            case "rain":
                BackgroundObject.sprite = BackgroundsSprite[1];
                CloudObject.sprite = CloudSprite[1];
                ForegroundObject.sprite = ForegroundSprite[0];
                NightTimeObject.enabled = true;
                break;
            case "snow":
                BackgroundObject.sprite = BackgroundsSprite[1];
                CloudObject.sprite = CloudSprite[0];
                ForegroundObject.sprite = ForegroundSprite[1];
                break;
            case "clouds":
                BackgroundObject.sprite = BackgroundsSprite[0];
                CloudObject.sprite = CloudSprite[0];
                ForegroundObject.sprite = ForegroundSprite[0];
                break;
            default:
                BackgroundObject.sprite = BackgroundsSprite[0];
                CloudObject.sprite = null;
                ForegroundObject.sprite = ForegroundSprite[0];
                break;
        }
    }

    #region Json weather object

    [System.Serializable]
    private class Coord
    {
        public double lon;
        public double lat;
    }

    [System.Serializable]
    private class Weather
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    [System.Serializable]
    private class Main
    {
        public double temp;
        public int pressure;
        public int humidity;
        public double temp_min;
        public double temp_max;
    }

    [System.Serializable]
    private class Wind
    {
        public double speed;
        public int deg;
    }

    [System.Serializable]
    private class Clouds
    {
        public int all;
    }

    [System.Serializable]
    private class Sys
    {
        public int type;
        public int id;
        public double message;
        public string country;
        public int sunrise;
        public int sunset;
    }

    [System.Serializable]
    private class RootObject
    {
        public Coord coord;
        public List<Weather> weather;
        public string @base;
        public Main main;
        public int visibility;
        public Wind wind;
        public Clouds clouds;
        public int dt;
        public Sys sys;
        public int id;
        public string name;
        public int cod;
    }
    #endregion
}