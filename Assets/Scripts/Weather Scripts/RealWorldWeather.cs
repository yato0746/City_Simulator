using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class RealWorldWeather : MonoBehaviour {

	// a1365a908f2f9bd2dfeb58d41177db68

	/*
		In order to use this API, you need to register on the website.

		Source: https://openweathermap.org

		Request by city: api.openweathermap.org/data/2.5/weather?q={city id}&appid={your api key}
		Request by lat-long: api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={your api key}

		Api response docs: https://openweathermap.org/current
	*/

	//public static Dictionary

	private static RealWorldWeather instance;
	public static RealWorldWeather Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
			instance = this;
        }
		else if (instance != null)
        {
			Destroy(this);
			Debug.Log("Object is already existed, destroy GameObject");
        }
    }

    public TextMeshProUGUI text;

	public string apiKey = "a1365a908f2f9bd2dfeb58d41177db68";

	public string city;
	public bool useLatLng = false;
	public string latitude;
	public string longitude;

	bool isBusy = false;
	public bool IsBusy { get { return isBusy; } }

  //  private void Start()
  //  {
		
		//GetRealWeather();
  //  }

    public void GetRealWeather () 
	{
		if (isBusy)
        {
			return;
        }

		isBusy = true;

		string uri = "api.openweathermap.org/data/2.5/weather?";
		if (useLatLng) {
			uri += "lat=" + latitude + "&lon=" + longitude + "&appid=" + apiKey;
		} else {
			uri += "q=" + city + "&appid=" + apiKey;
		}
		StartCoroutine (GetWeatherCoroutine (uri));
	}

	IEnumerator GetWeatherCoroutine (string uri) {
		using (UnityWebRequest webRequest = UnityWebRequest.Get (uri)) {
			yield return webRequest.SendWebRequest ();
			if (/*webRequest.isNetworkError*/webRequest.result == UnityWebRequest.Result.ConnectionError) {
				Debug.Log ("Web request error: " + webRequest.error);
			} else {
				ParseJson (webRequest.downloadHandler.text);
			}
		}
	}

	WeatherStatus ParseJson (string json) {
		WeatherStatus weather = new WeatherStatus ();
		try {
			dynamic obj = JObject.Parse (json);

			weather.weatherId = obj.weather[0].id;
			weather.main = obj.weather[0].main;
			weather.description = obj.weather[0].description;
			weather.temperature = obj.main.temp;
			weather.pressure = obj.main.pressure;
			weather.windSpeed = obj.wind.speed;
		} catch (Exception e) {
			Debug.Log (e.StackTrace);
		}

		//Debug.Log ("Temp in °C: " + weather.Celsius ());
		//Debug.Log ("Wind speed: " + weather.windSpeed);
		text.text = "Weather: " + weather.main + " " + weather.Celsius() + " Celsius";

		isBusy = false;

		return weather;
	}

}