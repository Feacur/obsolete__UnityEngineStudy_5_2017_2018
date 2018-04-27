using System.Collections;
using UnityEngine;
using UnityEngine.UI;

///
/// Код для экрана загрузки
/// * Имитация загрузки данных
/// * Переход на указанный экран
///
[RequireComponent(typeof(Screen))]
public class LoadingScreen : MonoBehaviour
{
	public Slider loadingSlider;
	public Text loadingText;

	//
	// "Injects"
	//

	private ScreensNavigation ScreensNavigation;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		this.ScreensNavigation = ScreensNavigation.instance;
	}

	//
	// Data setters
	//

	private void SetProgress(float fraction) {
		fraction = Mathf.Clamp01(fraction);
		this.loadingSlider.value = fraction;
		this.loadingText.text = $"{Mathf.RoundToInt(fraction * 100)}%";
	}

	//
	// Callbacks from Unity
	//

	private void OnEnable() {
		this.SetProgress(0);
	}
	
	//
	// LoadTo
	//

	public Coroutine LoadTo<T>() where T : MonoBehaviour {
		return StartCoroutine(LoadToCoroutine<T>());
	}
	
	private IEnumerator LoadToCoroutine<T>() where T : MonoBehaviour {
		const float loadingSeconds = 1;
		float time = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - time < loadingSeconds) {
			this.SetProgress((Time.realtimeSinceStartup - time) / loadingSeconds);
			yield return null;
		}

		this.SetProgress(1);
		yield return new WaitForSecondsRealtime(0.2f);

		ScreensNavigation.GoTo<T>();
	}
}
