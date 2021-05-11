using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using TMPro;

public class GPS : MonoBehaviour
{
    private static GPS instance;
    public static GPS Instance { get { return instance; } }

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
    }

    public float Latitude { get { return latitude; } }
    public float Longitude { get { return longitude; } }

    [SerializeField] float latitude = 0f;
    [SerializeField] float longitude = 0f;

    [SerializeField] bool isShowLocation = false;
    [SerializeField] TextMeshProUGUI GpsText;
    [SerializeField] Button RequestGpsButton;
    [SerializeField] Button MyPlaceButton;

    public void Button_RequestGPS()
    {
        RequestGpsButton.interactable = false;

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation) ||
            !Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);

            GpsText.text = "GPS: Request user permission";
            RequestGpsButton.interactable = true;

            return;
        }
#endif

        StartCoroutine(GetLocation());
    }

    IEnumerator GetLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            GpsText.text = "GPS: Location service is not enabled";
            RequestGpsButton.interactable = true;

            yield break;
        }

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
            GpsText.text = "GPS: Timed out";
            RequestGpsButton.interactable = true;
            Input.location.Stop();

            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GpsText.text = "GPS: Unable to determine device location";
            RequestGpsButton.interactable = true;
            Input.location.Stop();

            yield break;
        }

        // You have GPS, now you can request Weather
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;

        if (isShowLocation)
        {
            GpsText.text = "GPS: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude;
        }
        else
        {
            GpsText.text = "GPS: ON - Located";
        }

        RequestGpsButton.gameObject.SetActive(false);
        MyPlaceButton.gameObject.SetActive(true);

        Input.location.Stop();
    }
}
