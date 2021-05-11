using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFPS : MonoBehaviour
{
    private static GameFPS instance;
    public static GameFPS Instance { get { return instance; } }

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

    [SerializeField] int vSyncCount = 0;
    [SerializeField] int framesPerSecond = 60;
    [SerializeField] bool displayFps = true;
    float frequency = 1.0f;

    [Space]
    [SerializeField] TextMeshProUGUI textMeshPro;

    private void Start()
    {
        // If vSyncCount = 0, it means your machine can run at desiredFPS FrameRate
        QualitySettings.vSyncCount = vSyncCount;
        if (vSyncCount == 0)
        {
            Application.targetFrameRate = framesPerSecond;
        }

        StartCoroutine(DisplayFPS());
    }

    private IEnumerator DisplayFPS()
    {
        // Capture frame-per-second
        while (displayFps)
        {
            int _lastFrameCount = Time.frameCount;
            float _lastTime = Time.realtimeSinceStartup;

            yield return new WaitForSeconds(frequency);

            int _frameCount = Time.frameCount - _lastFrameCount;
            float timeSpan = Time.realtimeSinceStartup - _lastTime;

            float currentFPS = _frameCount / timeSpan;

            // Display it
            if (textMeshPro)
            {
                textMeshPro.text = "FPS: " + Mathf.Round(currentFPS * 100) / 100f;
            }
        }
    }
}
