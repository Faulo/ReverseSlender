using System.Collections;
using UnityEngine;

public class BryceHeartBeat : MonoBehaviour
{
    [Range(60, 160)] public int bpm = 60;
    public int Bpm
    {
        get => bpm;
        set => bpm = Mathf.Clamp(value, 60, 160);
    }

    [SerializeField] private AnimationCurve heartBeatAnimCurve;
    [SerializeField] private Material mat;

    private const string HEARTBEATNAME = "Heartbeat";

    private float heartVisibility;
    public float HeartVisibility
    {
        get => heartVisibility;
        set
        {
            heartVisibility = Mathf.Clamp01(value);
            mat.SetFloat("_UpperBodyMinAlpha", heartVisibility);
        }
    }

    private float AnimationLength => 60f / Bpm;

    private Vector3 targetScale = Vector3.one * 2f;

    public bool alive = true;

    private void Start()
    {
        _ = StartCoroutine(HeartBeatRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            alive = false;
        }
    }

    private IEnumerator HeartBeatRoutine()
    {
        while (alive)
        {
            AudioManager.Instance.PlaySound(HEARTBEATNAME);
            yield return StartCoroutine(transform.ScaleRoutine(targetScale, AnimationLength, heartBeatAnimCurve));
        }

        transform.localScale = Vector3.one;
        _ = StartCoroutine(transform.ScaleRoutine(targetScale * 2.5f, 1f, heartBeatAnimCurve));
        AudioManager.Instance.PlaySoundOverridePitch(HEARTBEATNAME, .85f);
        AudioManager.Instance.PlaySound("DeathChoir");
    }
}
