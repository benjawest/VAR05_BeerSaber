using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioHighPassFilter))]
[RequireComponent(typeof(AudioDistortionFilter))]
[RequireComponent(typeof(AudioReverbFilter))]
[RequireComponent(typeof(AudioEchoFilter))]
[RequireComponent(typeof(AudioChorusFilter))]
public class SoundFilterTester : MonoBehaviour
{
    public bool useLowPassFilter = false;
    public bool useHighPassFilter = false;
    public bool useDistortion = false;
    public bool useReverb = false;
    public bool useEcho = false;
    public bool useChorus = false;

    public float cutoffFrequency = 1000f;
    public float distortionLevel = 0.5f;
    public float reverbLevel = 0.5f;
    public float echoDelay = 0.5f;
    public float echoDecay = 0.5f;
    public float chorusRate = 0.5f;
    public float chorusDepth = 0.5f;

    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;
    private AudioHighPassFilter highPassFilter;
    private AudioDistortionFilter distortionFilter;
    private AudioReverbFilter reverbFilter;
    private AudioEchoFilter echoFilter;
    private AudioChorusFilter chorusFilter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        highPassFilter = GetComponent<AudioHighPassFilter>();
        distortionFilter = GetComponent<AudioDistortionFilter>();
        reverbFilter = GetComponent<AudioReverbFilter>();
        echoFilter = GetComponent<AudioEchoFilter>();
        chorusFilter = GetComponent<AudioChorusFilter>();

        UpdateAudioComponents();
    }

    private void Update()
    {
        UpdateAudioComponents();
    }

    private void UpdateAudioComponents()
    {
        if (lowPassFilter != null)
        {
            lowPassFilter.enabled = useLowPassFilter;
            lowPassFilter.cutoffFrequency = cutoffFrequency;
        }

        if (highPassFilter != null)
        {
            highPassFilter.enabled = useHighPassFilter;
            highPassFilter.cutoffFrequency = cutoffFrequency;
        }

        if (distortionFilter != null)
        {
            distortionFilter.enabled = useDistortion;
            distortionFilter.distortionLevel = distortionLevel;
        }

        if (reverbFilter != null)
        {
            reverbFilter.enabled = useReverb;
            reverbFilter.reverbLevel = reverbLevel;
        }

        if (echoFilter != null)
        {
            echoFilter.enabled = useEcho;
            echoFilter.delay = echoDelay;
            echoFilter.decayRatio = echoDecay;
        }

        if (chorusFilter != null)
        {
            chorusFilter.enabled = useChorus;
            chorusFilter.rate = chorusRate;
            chorusFilter.depth = chorusDepth;
        }
    }
}

[CustomEditor(typeof(SoundFilterTester))]
public class SoundFilterTesterEditor : Editor
{
    private SerializedProperty useLowPassFilter;
    private SerializedProperty useHighPassFilter;
    private SerializedProperty useDistortion;
    private SerializedProperty useReverb;
    private SerializedProperty useEcho;
    private SerializedProperty useChorus;

    private SerializedProperty cutoffFrequency;
    private SerializedProperty distortionLevel;
    private SerializedProperty reverbLevel;
    private SerializedProperty echoDelay;
    private SerializedProperty echoDecay;
    private SerializedProperty chorusRate;
    private SerializedProperty chorusDepth;

    private void OnEnable()
    {
        useLowPassFilter = serializedObject.FindProperty("useLowPassFilter");
        useHighPassFilter = serializedObject.FindProperty("useHighPassFilter");
        useDistortion = serializedObject.FindProperty("useDistortion");
        useReverb = serializedObject.FindProperty("useReverb");
        useEcho = serializedObject.FindProperty("useEcho");
        useChorus = serializedObject.FindProperty("useChorus");

        cutoffFrequency = serializedObject.FindProperty("cutoffFrequency");
        distortionLevel = serializedObject.FindProperty("distortionLevel");
        reverbLevel = serializedObject.FindProperty("reverbLevel");
        echoDelay = serializedObject.FindProperty("echoDelay");
        echoDecay = serializedObject.FindProperty("echoDecay");
        chorusRate = serializedObject.FindProperty("chorusRate");
        chorusDepth = serializedObject.FindProperty("chorusDepth");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SoundFilterTester soundFilterTester = (SoundFilterTester)target;

        EditorGUILayout.PropertyField(useLowPassFilter);
        EditorGUILayout.PropertyField(useHighPassFilter);
        EditorGUILayout.PropertyField(useDistortion);
        EditorGUILayout.PropertyField(useReverb);
        EditorGUILayout.PropertyField(useEcho);
        EditorGUILayout.PropertyField(useChorus);

        EditorGUILayout.Slider(cutoffFrequency, 0f, 22000f);
        EditorGUILayout.Slider(distortionLevel, 0f, 1f);
        EditorGUILayout.Slider(reverbLevel, 0f, 1f);
        EditorGUILayout.Slider(echoDelay, 0f, 1f);
        EditorGUILayout.Slider(echoDecay, 0f, 1f);
        EditorGUILayout.Slider(chorusRate, 0f, 20f);
        EditorGUILayout.Slider(chorusDepth, 0f, 1f);

        if (GUILayout.Button("Play"))
        {
            soundFilterTester.GetComponent<AudioSource>().Play();
        }

        if (GUILayout.Button("Pause"))
        {
            soundFilterTester.GetComponent<AudioSource>().Pause();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
