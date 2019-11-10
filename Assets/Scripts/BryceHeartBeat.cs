using System.Collections;
using UnityEngine;

public class BryceHeartBeat : MonoBehaviour
{
    [Range(60, 180)] public int bpm = 60;
    [SerializeField] private AnimationCurve heartBeatAnimCurve;
    [SerializeField] private Material mat;

    private float heartVisibility;
    public float HeartVisibility
    {
        get { return heartVisibility; }
        set
        {
            heartVisibility = Mathf.Clamp01(value);
            mat.SetFloat("_UpperBodyMinAlpha", heartVisibility);
        }
    }

    private float AnimationLength => 60f / bpm;

    private Vector3 targetScale = Vector3.one * 2f;

    public bool alive = true;

    private void Start()
    {
        StartCoroutine(HeartBeatRoutine());
    }

    private IEnumerator HeartBeatRoutine()
    {
        while (alive)
        {
            yield return StartCoroutine(transform.ScaleRoutine(targetScale, AnimationLength, heartBeatAnimCurve));
        }
    }
}
