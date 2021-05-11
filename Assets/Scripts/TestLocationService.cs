using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class TestLocationService : MonoBehaviour
{
    [SerializeField] Text _status;
    [SerializeField] Text _latitude;
    [SerializeField] Text _longitude;

    private void Awake()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
#endif
    }

    private void Start()
    {
        StartCoroutine(GPSLoc());

    }

    void Fail()
    {
        _latitude.text = "";
        _longitude.text = "";

    }

    IEnumerator GPSLoc()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            _status.text = "No location service enable";
            Fail();
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

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            _status.text = "Timed out";
            Fail();
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            _status.text = "Unable to determine device location";   
            Fail();
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            _status.text = "Connected";
            _latitude.text = "Latitude: " + Input.location.lastData.latitude.ToString();
            _longitude.text = "Longitude: " + Input.location.lastData.longitude;

            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
}