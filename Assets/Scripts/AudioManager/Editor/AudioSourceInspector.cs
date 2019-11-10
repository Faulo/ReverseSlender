//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(AudioSource))]
//public class AudioSourceInspector : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        if (GUILayout.Button("Select Audiomanager"))
//        {
//            AudioManager audioManager = FindObjectOfType<AudioManager>();
//            if (audioManager != null)
//            {
//                Selection.activeGameObject = audioManager.gameObject;
//            }
//        }
//    }
//}
