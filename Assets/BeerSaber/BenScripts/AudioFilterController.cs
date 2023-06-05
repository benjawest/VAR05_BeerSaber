using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class AudioFilterController : MonoBehaviour
{
    public enum FilterType
    {
        LowPass,
        Distortion
    }

    [System.Serializable]
    public class FilterSettings
    {
        public FilterType filterType;
        public bool enabled;
        public float minValue;
        public float maxValue;
        public float currentValue;
    }

    public FilterSettings[] filters;
    
    [Range(0f, 1f)]
    public float inputRange = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Map the input range to the desired effect ranges
        float rotationAngle = inputRange * 180f;
        float mappedValue = Mathf.Lerp(0f, 1f, inputRange);

        // Apply the selected filters based on the filter settings
        foreach (var filter in filters)
        {
            if (filter.enabled)
            {
                switch (filter.filterType)
                {
                    case FilterType.LowPass:
                        float lowPassCutoff = Mathf.Lerp(filter.minValue, filter.maxValue, mappedValue);
                        ApplyLowPassFilter(lowPassCutoff);
                        break;
                    case FilterType.Distortion:
                        float distortionLevel = Mathf.Lerp(filter.minValue, filter.maxValue, mappedValue);
                        ApplyDistortionFilter(distortionLevel);
                        break;
                        // Add more cases for additional filter types
                }
            }
        }
    }

    private void ApplyLowPassFilter(float cutoffFrequency)
    {
        AudioLowPassFilter lowPassFilter = GetComponent<AudioLowPassFilter>();
        lowPassFilter.enabled = true;
        lowPassFilter.cutoffFrequency = cutoffFrequency;
    }

    private void ApplyDistortionFilter(float distortionLevel)
    {
        AudioDistortionFilter distortionFilter = GetComponent<AudioDistortionFilter>();
        distortionFilter.enabled = true;
        distortionFilter.distortionLevel = distortionLevel;
    }
}
