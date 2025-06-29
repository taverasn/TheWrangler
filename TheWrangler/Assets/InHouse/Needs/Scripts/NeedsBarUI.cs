using NUnit;
using System.Collections;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class NeedsBarUI : MonoBehaviour
{
    [field:SerializeField] public NeedsType needsType { get; private set; }
    [SerializeField] private Slider slider;
    [SerializeField] private float smoothTime = 0.5f;
    private float interpolationSpeed = 0f;
    private float time = 0f;
    private float reachValue = 0f;
    public void SetSliderValue(float value, bool immediate)
    {
        if (immediate)
        {
            slider.value = value;
        }
        else
        {
            interpolationSpeed = Mathf.Abs(value - slider.value) / smoothTime;
            reachValue = value;
        }
    }

    public void Update()
    {
        if (slider.value != reachValue)
        {
            time += Time.deltaTime;
            slider.value = Mathf.MoveTowards(slider.value, reachValue, interpolationSpeed * Time.deltaTime);
            
            if (slider.value == reachValue)
            {
                time = 0;
                reachValue = 0;
                interpolationSpeed = 0;
            }
        }
    }
}
