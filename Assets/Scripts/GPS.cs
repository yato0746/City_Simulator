using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;

public class GPS : MonoBehaviour
{
    public float latitude = 0f;
    public float longitude = 0f;

    public TextMeshProUGUI text;
    public TextMeshProUGUI GpsStateText;

    bool isLocated = false;

    public void Button_RequestPlace()
    {
        if (isLocated)
        {
            return;
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation) ||
            !Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
            return;
        }

        StartCoroutine(GetLocation());
    }

    IEnumerator GetLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 second;
        if (maxWait < 1)
        {
            text.text = "Timed out";
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            text.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            isLocated = true;

            text.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude;
        }

        Input.location.Stop();

        RealWorldWeather.Instance.latitude = "" +latitude;
        RealWorldWeather.Instance.longitude = "" +longitude;
        RealWorldWeather.Instance.useLatLng = true;
        RealWorldWeather.Instance.GetRealWeather();
    }
}
