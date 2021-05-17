using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class RealWorldWeather : MonoBehaviour {
	/*
		In order to use this API, you need to register on the website.

		Source: https://openweathermap.org

		Request by city: api.openweathermap.org/data/2.5/weather?q={city id}&appid={your api key}
		Request by lat-long: api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={your api key}

		Api response docs: https://openweathermap.org/current
	*/

	private static RealWorldWeather instance;
	public static RealWorldWeather Instance { get { return instance; } }

	[SerializeField] string apiKey = "a1365a908f2f9bd2dfeb58d41177db68";

	[SerializeField] Image weatherImage;
	[SerializeField] List<Sprite> spriteList = new List<Sprite>();
	private Dictionary<string, Sprite> weatherSprites;

	[SerializeField] string city;
	private Dictionary<int, string> cityStrings = new Dictionary<int, string>()
	{
        { 1, "London" },
        { 2, "New York" },
        { 3, "Los Angeles" },
        { 4, "Chicago" },
        { 5, "Houston" },
        { 6, "Phoenix" },
        { 7, "San Diego" },
        { 8, "San Francisco" }
    };

	[SerializeField] bool useLatLng = false;
	[SerializeField] string latitude;
	[SerializeField] string longitude;

	bool isBusy = false;
	public bool IsBusy { get { return isBusy; } }

	string currentPlace = "";
	[SerializeField] TextMeshProUGUI currentPlaceText;
	[SerializeField] TextMeshProUGUI weatherText;
	[SerializeField] TextMeshProUGUI weatherPlusText;

	[Header("Specific location")]
	[SerializeField] TMP_InputField latitudeInput;
	[SerializeField] TMP_InputField longitudeInput;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy(this);
			Debug.Log("Instance already exists, destroy GameObject");
		}

		weatherSprites = new Dictionary<string, Sprite>()
		{
			{ "Thunderstorm", spriteList[0] },
			{ "Drizzle", spriteList[1] },
			{ "Rain", spriteList[2] },
			{ "Snow", spriteList[3] },
			{ "Clear", spriteList[4] },
			{ "Clouds", spriteList[5] },
			//
			{ "Mist", spriteList[6] },
			{ "Smoke", spriteList[7] },
			{ "Haze", spriteList[8] },
			{ "Dust", spriteList[9] },
			{ "Fog", spriteList[10] },
			{ "Sand", spriteList[11] },
			{ "Ash", spriteList[12] },
			{ "Squall", spriteList[13] },
			{ "Tornado", spriteList[14] },
		};

		Button_RandomPlace();
	}

	void SetWeatherImage(string _keyString)
    {
		if (weatherSprites.ContainsKey(_keyString))
        {
			weatherImage.sprite = weatherSprites[_keyString];
        }
		else
        {
			weatherImage.sprite = weatherSprites["Clouds"];
        }
    }

    public void Button_MyPlace()
    {
		latitude = "" + GPS.Instance.Latitude;
		longitude = "" + GPS.Instance.Longitude;
		city = "Your Town";
		useLatLng = true;

		GetRealWeather();
    }

	public void Button_RandomPlace()
    {
		int random = UnityEngine.Random.Range(1, cityStrings.Count + 1);
		city = cityStrings[random];
		useLatLng = false;

		GetRealWeather();
    }

	public void Button_RequestLocation()
    {
		latitude = latitudeInput.text;
		longitude = longitudeInput.text;
		city = "Location: " + latitudeInput.text + " " + longitudeInput.text;
		useLatLng = true;

		GetRealWeather();
    }

	public void GetRealWeather () 
	{
		if (IsBusy)
        {
			return;
        }

		currentPlace = city;
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
		//text.text = "Weather: " + weather.main + " " + weather.Celsius() + " Celsius";

		weatherText.text = "Weather: " + weather.main + ", " + weather.Celsius() + " Celsius";
		weatherPlusText.text = "Wind: " + weather.windSpeed + " km/h, " + weather.pressure + " Pa";

		currentPlaceText.text = currentPlace;
		SetWeatherImage(weather.main);
		isBusy = false;

		return weather;
	}

}