using System.Collections;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.UI;

public class NeedsBarUI : MonoBehaviour
{
    [field:SerializeField] public NeedsType needsType { get; private set; }
    [SerializeField] private Slider slider;
    [SerializeField] private float smoothTime = 0.2f;

    public void SetSliderValue(float value, bool immediate)
    {
        if (immediate)
        {
            slider.value = value;
        }
        else
        {
            StartCoroutine(SmoothSlider(slider.value,value, smoothTime));
        }
    }
    IEnumerator SmoothSlider(float start, float end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            slider.value = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        slider.value = end;
    }
}
