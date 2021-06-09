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
	void InitializeSingleton()
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
	}

	[Header("Weather API")]
	[SerializeField] string apiKey = "a1365a908f2f9bd2dfeb58d41177db68";
	[SerializeField] bool useLatLng = false;	
	[SerializeField] string latitude;
	[SerializeField] string longitude;
	[SerializeField] string city;
	string currentPlace = "";
	bool isBusy = false;
	public bool IsBusy { get { return isBusy; } }

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

	[Header("Weather sprites")]
	[SerializeField] Sprite defaultSprite;
	[SerializeField] Sprite thunderstormSprite;
	[SerializeField] Sprite drizzleSprite;
	[SerializeField] Sprite rainSprite;
	[SerializeField] Sprite snowSprite;
	[SerializeField] Sprite clearSprite;
	[SerializeField] Sprite cloudsSprite;
	// "Mist", "Smoke", "Haze", "Dust", "Fog", "Sand", "Ash", "Squall", "Tornado"

	delegate void SetWeatherDelegate();
	Dictionary<string, SetWeatherDelegate> setWeathers;

	[Header("Canvas")]
	[SerializeField] Button weatherButton;
	[SerializeField] TextMeshProUGUI currentPlaceText;
	[SerializeField] TextMeshProUGUI weatherText;
	[SerializeField] TextMeshProUGUI weatherPlusText;

	[Header("Specific location")]
	[SerializeField] TMP_InputField latitudeInput;
	[SerializeField] TMP_InputField longitudeInput;

	private void Awake()
	{
		InitializeSingleton();
	}

	private void Start()
	{
		setWeathers = new Dictionary<string, SetWeatherDelegate>()
		{
			{ "Default", SetDefaultWeather },
			{ "Thunderstorm", SetThunderstorm },
			{ "Drizzle", SetDrizzle},
			{ "Rain", SetRain },
			{ "Snow", SetSnow },
			{ "Clear", SetClear},
			{ "Clouds", SetClouds }
		};

        Button_RandomPlace();
    }

    #region Set Weathers
    public void SetWeather(string _mainWeather, bool _changeText = false)
    {
		try
		{
			if (setWeathers.ContainsKey(_mainWeather))
			{
				setWeathers[_mainWeather]();
			}
			else
			{
				setWeathers["Default"]();
			}
		}
		catch (Exception _e)
        {
			Debug.Log(_e.StackTrace);
        }

		if (_changeText)
        {
			weatherText.text = _mainWeather;
        }
    }

    void SetDefaultWeather()
    {
		weatherButton.image.sprite = defaultSprite;
		GameController.Instance.SetEnvironmentEffect(false, true, true, false);
    }

	void SetThunderstorm()
    {
		weatherButton.image.sprite = thunderstormSprite;
		GameController.Instance.SetEnvironmentEffect(true, true, true, false, true);
	}

	void SetDrizzle()
    {
		weatherButton.image.sprite = drizzleSprite;
		GameController.Instance.SetEnvironmentEffect(true, true, true, false);
	}

	void SetRain()
    {
		weatherButton.image.sprite = rainSprite;
		GameController.Instance.SetEnvironmentEffect(true, true, true, false, true);
	}

	void SetSnow()
    {
		weatherButton.image.sprite = snowSprite;
		GameController.Instance.SetEnvironmentEffect(false, true, false, true);
	}

	void SetClear()
    {
		weatherButton.image.sprite = clearSprite;
		GameController.Instance.SetEnvironmentEffect(false, true, false, false);
	}

	void SetClouds()
    {
		weatherButton.image.sprite = cloudsSprite;
		GameController.Instance.SetEnvironmentEffect(false, true, false, false, true);
	}
    #endregion

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
		city = "Location: (" + latitudeInput.text + ", " + longitudeInput.text + ")";
		useLatLng = true;

		GetRealWeather();
    }

	public void GetRealWeather () 
	{
		//if (webRequest.result == UnityWebRequest.Result.ConnectionError)
		//      {
		//	return;
		//      }

		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			Debug.Log("Error. Check internet connection!");
			return;
		}

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

	WeatherStatus ParseJson(string json) {
		WeatherStatus weather = new WeatherStatus();
		try {
			dynamic obj = JObject.Parse(json);

			weather.weatherId = obj.weather[0].id;
			weather.main = obj.weather[0].main;
			weather.description = obj.weather[0].description;
			weather.temperature = obj.main.temp;
			weather.pressure = obj.main.pressure;
			weather.windSpeed = obj.wind.speed;
		} catch (Exception e) {
			Debug.Log(e.StackTrace);
		}

		//Debug.Log ("Temp in °C: " + weather.Celsius ());
		//Debug.Log ("Wind speed: " + weather.windSpeed);
		//text.text = "Weather: " + weather.main + " " + weather.Celsius() + " Celsius";

		SetWeather(weather.main);
		Debug.Log("Main weather: " + weather.main);

		weatherText.text = "Weather: " + weather.main + ", " + weather.Celsius() + " Celsius";
		weatherPlusText.text = "Wind: " + weather.windSpeed + " km/h, " + weather.pressure + " Pa";
		currentPlaceText.text = currentPlace;

		if (weather.main == null)
        {
			weatherText.text = "UnIdentified";
			weatherPlusText.text = "";
		}

		isBusy = false;

		return weather;
	}

}