using System.Collections;
using UnityEngine;

public class BryceHeartBeat : MonoBehaviour
{
    [Range(60, 180)] public int bpm = 60;
    [SerializeField] private AnimationCurve heartBeatAnimCurve;

    private float AnimationLength => 60f / bpm;

    private Vector3 targetScale = Vector3.one * 2f;

    private void Start()
    {
        StartCoroutine(HeartBeatRoutine());
    }

    private IEnumerator HeartBeatRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(transform.ScaleRoutine(targetScale, AnimationLength, heartBeatAnimCurve));
        }
    }
}
