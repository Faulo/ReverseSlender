using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(AudioManager))]
public class AudioManagerInspector : Editor
{
    private AudioManager audioManager;

    private void OnEnable()
    {
        audioManager = target as AudioManager;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty soundArray = serializedObject.FindProperty("sounds");
        SerializedProperty sampleAudioSourceProp = serializedObject.FindProperty("sampleSourceFor3DSettings");

        EditorGUILayout.PropertyField(sampleAudioSourceProp);

        GUILayout.Label("Sounds", "BoldLabel");

        for (int i = 0; i < soundArray.arraySize; i++)
        {
            Sound sound = audioManager.GetSound(i);

            var soundProperty = soundArray.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(soundProperty, true);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Play"))
            {
                audioManager.PlayTestingSound(i);
            }

            if (GUILayout.Button("Stop"))
            {
                audioManager.StopTestingSound();
            }

            if(sound.Is3DSound && sound.ParentObject != null)
            {
                if (GUILayout.Button("Select audio source"))
                {
                    Selection.activeGameObject = sound.source.gameObject;
                }
            }

            if (EditorApplication.isPlaying == false)
            {
                if (GUILayout.Button("RemoveSound"))
                {
                    audioManager.RemoveSound(i);
                }

            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(50);

        if (EditorApplication.isPlaying == false)
        {

            if (GUILayout.Button("Add new sound"))
            {
                audioManager.AddNewSound();
            }

            GUILayout.Space(25);

            if (GUILayout.Button("Create 3D audiosources"))
            {
                audioManager.Create3DAudioSources();
            }

        }
        serializedObject.ApplyModifiedProperties();
    }
}
