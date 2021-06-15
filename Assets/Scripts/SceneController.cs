using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance;

    bool isOnAndroid = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        Instance = instance;
        DontDestroyOnLoad(this);

#if UNITY_ANDROID
        isOnAndroid = true;
        Screen.fullScreen = false;
#endif
    }

    private void OnApplicationPause(bool appPaused)
    {
        if (!isOnAndroid || Application.isEditor)
        {
            return;
        }

        if (!appPaused)
        {
            // Returning to Application
            Debug.Log("Application Resumed");
            StartCoroutine(LoadSceneFromFCM());
        }
        else
        {
            // Leaving Application
            Debug.Log("Application Paused");
        }
    }

    IEnumerator LoadSceneFromFCM()
    {
        AndroidJavaClass _unityPlayer = new AndroidJavaClass("com.untiy3d.player.UnityPlayer");
        AndroidJavaObject _curActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject _curIntent = _curActivity.Call<AndroidJavaObject>("getIntent");

        string _sceneToLoad = _curIntent.Call<string>("getStringExtra", "sceneToOpen");

        Scene _curScene = SceneManager.GetActiveScene();

        if (!string.IsNullOrEmpty(_sceneToLoad) && _sceneToLoad != _curScene.name)
        {
            Debug.Log("Loading Scene: " + _sceneToLoad);
            Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
            Handheld.StartActivityIndicator();
            yield return new WaitForSeconds(0f);

            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
