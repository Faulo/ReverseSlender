﻿using UnityEngine;
using UnityEngine.Experimental.VFX;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerSettings settings = default;
    [SerializeField]
    private VisualEffect ghostVFX = default;
    [SerializeField]
    private VisualEffect bigMonsterVFX = default;

    private float terrainHeightUnderPlayer;

    private Rigidbody body;
    private Cinemachine.CinemachineVirtualCamera cam;
    private SphereCollider sphereCollider;

    private Vector3 moveVector;
    private Vector3? startingVectorMovingLeft;
    private Vector3? startingVectorMovingRight;

    private RaycastHit[] groundHits = new RaycastHit[1];

    private const string GHOST_ATTRACTIVETARGETPOSITION_NAME = "AttractiveTargetPosition";
    private const string BIGMONSTERSCARE_EVENTNAME = "OnScare";
    private const string BIGMONSTERSTOPSCARE_EVENTNAME = "StopScare";

    public bool InScareMode { get; private set; }

    private Vector3 bigMonstaStartPos;

    private AudioSource moveWhisperSource;
    private const string MOVEWHISPER_SOUNDNAME = "Whispering";
    private const string AMBIENCE_SOUNDNAME = "Ambience";
    private const string WIND_SOUNDNAME = "Wind";
    private const string VANISH_SOUNDNAME = "Vanish";
    private const string SCARE_SOUNDNAME = "Scare";
    private float moveWhisperFadeInOutDuration = .5f;

    private bool moving;
    public bool Moving
    {
        get => moving;
        private set
        {
            if (moving == value)
                return;
            if (moveWhisperSource == null)
                moveWhisperSource = AudioManager.Instance.GetAudioSource(MOVEWHISPER_SOUNDNAME);
            moving = value;
            if (moving == true)
            {
                if (moveWhisperSource.isPlaying == false)
                    moveWhisperSource.Play();
                moveWhisperSource.volume = 0f;
                moveWhisperSource.LerpVolume(AudioManager.Instance.GetOriginalVolume(MOVEWHISPER_SOUNDNAME), moveWhisperFadeInOutDuration, this);
            }
            else
            {
                moveWhisperSource.LerpVolume(0f, moveWhisperFadeInOutDuration, this);
            }
        }
    }

    private bool alternativeControls = true;

    private const string SCAREBUTTON_NAME = "Scare";

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        sphereCollider = GetComponent<SphereCollider>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        ghostVFX.SendEvent(BIGMONSTERSTOPSCARE_EVENTNAME);

        CheckForGround();
        Vector3 targetPos = body.position;
        Vector3 newPosition = new Vector3(targetPos.x,
                                  Mathf.Clamp(targetPos.y,
                                              terrainHeightUnderPlayer + settings.minDistanceAboveGround,
                                              terrainHeightUnderPlayer + settings.maxDistanceAboveGround),
                                  targetPos.z);
        body.MovePosition(newPosition);
        ghostVFX.transform.position = body.position;
    }

    private void Update()
    {
        GetInput();
        CheckForGround();
        ghostVFX.SetVector3(GHOST_ATTRACTIVETARGETPOSITION_NAME, ghostVFX.transform.InverseTransformPoint(body.position));

        bool startScare = Input.GetButtonDown(SCAREBUTTON_NAME) || (Input.GetAxis(SCAREBUTTON_NAME) >= .8f && InScareMode == false);
        bool endScare = Input.GetButtonUp(SCAREBUTTON_NAME) || (Input.GetAxis(SCAREBUTTON_NAME) < .5f && InScareMode == true);

        if (startScare)
        {
            DoSomeScaring();
        }

        if (endScare)
        {
            EndTheScaring();
        }

        if (InScareMode)
        {
            bigMonsterVFX.transform.position = Vector3.Lerp(bigMonsterVFX.transform.position, transform.position, settings.bigMonstaLerpSpeed);
        }

        if (moveWhisperSource == null)
            moveWhisperSource = AudioManager.Instance.GetAudioSource(MOVEWHISPER_SOUNDNAME);

        Moving = moveVector.magnitude > 0;

    }

    private void FixedUpdate()
    {
        HandleMovement();
        ghostVFX.transform.position = Vector3.Lerp(ghostVFX.transform.position, transform.position, settings.ghostVFXLerpSpeed);
        sphereCollider.center = transform.InverseTransformPoint(ghostVFX.transform.position) * .3f;
    }

    private void DoSomeScaring()
    {
        InScareMode = true;
        bigMonsterVFX.SendEvent(BIGMONSTERSCARE_EVENTNAME);
        ghostVFX.SendEvent(BIGMONSTERSCARE_EVENTNAME);
        bigMonstaStartPos = transform.position + (cam.transform.position - transform.position).normalized * 6f;
        bigMonsterVFX.transform.position = bigMonstaStartPos;
        Quaternion targetRotation = new Quaternion();
        targetRotation.SetLookRotation(cam.transform.forward, Vector3.up);
        bigMonsterVFX.transform.rotation = targetRotation;

        AudioManager.Instance.PlaySound(SCARE_SOUNDNAME);
        AudioManager.Instance.GetAudioSource(AMBIENCE_SOUNDNAME).LerpVolume(0.01f, .5f, this);
        AudioManager.Instance.GetAudioSource(WIND_SOUNDNAME).LerpVolume(0.01f, .5f, this);
    }

    private void EndTheScaring()
    {
        InScareMode = false;
        bigMonsterVFX.SendEvent(BIGMONSTERSTOPSCARE_EVENTNAME);
        ghostVFX.SendEvent(BIGMONSTERSTOPSCARE_EVENTNAME);
        AudioManager.Instance.PlaySound(VANISH_SOUNDNAME);
        AudioManager.Instance.GetAudioSource(SCARE_SOUNDNAME).LerpVolume(0f, .5f, this);
        AudioManager.Instance.GetAudioSource(AMBIENCE_SOUNDNAME).LerpVolume(AudioManager.Instance.GetOriginalVolume(AMBIENCE_SOUNDNAME), .5f, this);
        AudioManager.Instance.GetAudioSource(WIND_SOUNDNAME).LerpVolume(AudioManager.Instance.GetOriginalVolume(WIND_SOUNDNAME), .5f, this);
    }

    private void GetInput()
    {
        moveVector = new Vector3();

        float inputXAxis = Input.GetAxis("Horizontal");
        float inputYAxis = Input.GetAxis("Vertical");

        float simpleDeadZone = .25f;

        bool moveForward = inputYAxis > simpleDeadZone;// Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveBack = inputYAxis < -simpleDeadZone;// Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool moveLeft = inputXAxis < -simpleDeadZone;// Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool moveRight = inputXAxis > simpleDeadZone;// Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (moveForward)
            moveVector += cam.transform.forward;
        else if (moveBack)
            moveVector += -cam.transform.forward;
        if (moveLeft)
        {
            if (alternativeControls)
            {
                moveVector += -cam.transform.right;
            }
            else
            {
                if (startingVectorMovingLeft.HasValue == false)
                    startingVectorMovingLeft = -cam.transform.right;
                moveVector += startingVectorMovingLeft.Value;
            }
        }
        else if (moveRight)
        {
            if (alternativeControls)
            {
                moveVector += cam.transform.right;
            }
            else
            {
                if (startingVectorMovingRight.HasValue == false)
                    startingVectorMovingRight = cam.transform.right;
                moveVector += startingVectorMovingRight.Value;
            }
        }
        if (moveLeft == false)
            startingVectorMovingLeft = null;
        if (moveRight == false)
            startingVectorMovingRight = null;
    }

    private void HandleMovement()
    {
        if (moveVector.magnitude <= 0.01f)
            return;

        Vector3 targetPos = body.position + moveVector.normalized * (settings.moveSpeed * (InScareMode ? settings.scareMoveSpeedModifier : 1f)) * Time.fixedDeltaTime;
        Vector3 newPosition = new Vector3(targetPos.x,
                                          Mathf.Clamp(targetPos.y,
                                                      terrainHeightUnderPlayer + settings.minDistanceAboveGround,
                                                      terrainHeightUnderPlayer + settings.maxDistanceAboveGround),
                                          targetPos.z);
        body.MovePosition(newPosition);
    }

    private void CheckForGround()
    {
        float rayLength = Mathf.Infinity;
        if (Physics.RaycastNonAlloc(transform.position, Vector3.down, groundHits, rayLength) > 0)
            terrainHeightUnderPlayer = groundHits[0].point.y;
    }
}
