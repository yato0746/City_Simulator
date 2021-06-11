using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private static GameController instance;
	public static GameController Instance { get { return instance; } }
	
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

	[SerializeField] GameObject rainEffect;
    [SerializeField] GameObject windEffect;
    [SerializeField] GameObject dustEffect;
    [SerializeField] GameObject snowEffect;
	[SerializeField] Light mlight;
	
	const float darkIntensity = 0.53f;
	const float shiningIntensity = 1.06f;
	float desiredLightIntensity = 1.06f;

	public bool IsRaining
    {
		get { return rainEffect.activeSelf; }
    }

    private void Awake()
    {
		InitializeSingleton();
    }

	public void SetEnvironmentEffect(bool _rain, bool _wind, bool _dust, bool _snow, bool _darkLight = false)
    {
		rainEffect.SetActive(_rain);
		windEffect.SetActive(_wind);
		dustEffect.SetActive(_dust);
		snowEffect.SetActive(_snow);

		if (_darkLight)
		{
			desiredLightIntensity = darkIntensity;
		}
		else
        {
			desiredLightIntensity = shiningIntensity;
        }
    }

    private void Update()
    {
		mlight.intensity = Mathf.Lerp(mlight.intensity, desiredLightIntensity, 4f * Time.deltaTime);
    }
}
