using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] GameObject dropdownObjects;
    [SerializeField] GameObject dropdownWeathers;

    public void Button_DropDown()
    {
        dropdownObjects.SetActive(!dropdownObjects.activeSelf);
    }

    public void Button_WeatherDropdown()
    {
        dropdownWeathers.SetActive(!dropdownWeathers.activeSelf);
    }

    public void Button_SelectWeather(string _weather)
    {
        RealWorldWeather.Instance.SetWeather(_weather, true);
        dropdownWeathers.SetActive(false);
    }
}
