using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "AudioClip", menuName = "ScriptableObjects/Audio/AudioClip")]
public class AudioClipSO : ScriptableObject
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    public bool randomizeAudios;
    public bool loopAudio;
    public float clipVolume = 1;
    public float spacialBlend;

    public AudioClip _AudioClip {
        get
        {
            if(!randomizeAudios)
                return audioClip;
            else
            {
                int rand = Random.Range(0, audioClips.Count);
                return audioClips[rand];
            }
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(AudioClipSO))]
public class AudioClipSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //var audioClipSO = (AudioClipSO)target;

        if(!serializedObject.FindProperty("randomizeAudios").boolValue)
        {
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClip"));
            EditorGUI.indentLevel = 0;
        }
        else
        {
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClips"));
            EditorGUI.indentLevel = 0;
        }

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("loopAudio"));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Clip Volume");
        GUILayout.Space(20);
        GUILayout.Label(serializedObject.FindProperty("clipVolume").floatValue.ToString("N1"));
        serializedObject.FindProperty("clipVolume").floatValue = GUILayout.HorizontalSlider(serializedObject.FindProperty("clipVolume").floatValue, 0, 1);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spacial Blend");
        GUILayout.Space(20);
        GUILayout.Label(serializedObject.FindProperty("spacialBlend").floatValue.ToString("N1"));
        serializedObject.FindProperty("spacialBlend").floatValue = GUILayout.HorizontalSlider(serializedObject.FindProperty("spacialBlend").floatValue, 0, 1);
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
