using UnityEngine;
using UnityEngine.Experimental.VFX;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 300;
    [SerializeField] private float scareMoveSpeedModifier = .5f;
    [SerializeField] private float maxDistanceAboveGround = 5f;
    [SerializeField] private float minDistanceAboveGround = 0f;

    [SerializeField, Range(.01f, .25f)] private float ghostVFXLerpSpeed = 0.01f;
    [SerializeField, Range(.01f, .25f)] private float bigMonstaLerpSpeed = 0.01f;
    private float terrainHeightUnderPlayer;

    private Rigidbody body;
    private Cinemachine.CinemachineVirtualCamera cam;

    private Vector3 moveVector;
    private Vector3? startingVectorMovingLeft;
    private Vector3? startingVectorMovingRight;

    private RaycastHit[] groundHits = new RaycastHit[1];

    [SerializeField] private VisualEffect ghostVFX;
    [SerializeField] private VisualEffect bigMonsterVFX;
    private const string GHOST_ATTRACTIVETARGETPOSITION_NAME = "AttractiveTargetPosition";
    private const string BIGMONSTERSCARE_EVENTNAME = "OnScare";
    private const string BIGMONSTERSTOPSCARE_EVENTNAME = "StopScare";

    public bool InScareMode { get; private set; }

    private Vector3 bigMonstaStartPos;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        ghostVFX.SendEvent(BIGMONSTERSTOPSCARE_EVENTNAME);
    }

    private void Update()
    {
        GetInput();
        CheckForGround();
        ghostVFX.SetVector3(GHOST_ATTRACTIVETARGETPOSITION_NAME, ghostVFX.transform.InverseTransformPoint(body.position));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoSomeScaring();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndTheScaring();
        }

        if (InScareMode)
        {
            bigMonsterVFX.transform.position = Vector3.Lerp(bigMonsterVFX.transform.position, transform.position, bigMonstaLerpSpeed);
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ghostVFX.transform.position = Vector3.Lerp(ghostVFX.transform.position, transform.position, ghostVFXLerpSpeed);
    }

    private void DoSomeScaring()
    {
        bigMonsterVFX.SendEvent(BIGMONSTERSCARE_EVENTNAME);
        ghostVFX.SendEvent(BIGMONSTERSCARE_EVENTNAME);
        InScareMode = true;
        bigMonstaStartPos = transform.position + (cam.transform.position - transform.position).normalized * 6f;
        bigMonsterVFX.transform.position = bigMonstaStartPos;
    }

    private void EndTheScaring()
    {
        bigMonsterVFX.SendEvent(BIGMONSTERSTOPSCARE_EVENTNAME);
        ghostVFX.SendEvent(BIGMONSTERSTOPSCARE_EVENTNAME);
        InScareMode = false;
    }


    private void GetInput()
    {
        moveVector = new Vector3();

        bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveBack = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (moveForward)
            moveVector += cam.transform.forward;
        else if (moveBack)
            moveVector += -cam.transform.forward;
        if (moveLeft)
        {
            if (startingVectorMovingLeft.HasValue == false)
                startingVectorMovingLeft = -cam.transform.right;
            moveVector += startingVectorMovingLeft.Value;
        }
        else if (moveRight)
        {
            if (startingVectorMovingRight.HasValue == false)
                startingVectorMovingRight = cam.transform.right;
            moveVector += startingVectorMovingRight.Value;
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
        Vector3 targetPos = body.position + moveVector.normalized * (moveSpeed * (InScareMode ? scareMoveSpeedModifier : 1f)) * Time.fixedDeltaTime;
        Vector3 newPosition = new Vector3(targetPos.x,
                                          Mathf.Clamp(targetPos.y,
                                                      terrainHeightUnderPlayer + minDistanceAboveGround,
                                                      terrainHeightUnderPlayer + maxDistanceAboveGround),
                                          targetPos.z);
        body.MovePosition(newPosition);
    }

    private void CheckForGround()
    {
        float rayLength = Mathf.Infinity;
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);
        if (Physics.RaycastNonAlloc(transform.position, Vector3.down, groundHits, rayLength) > 0)
            terrainHeightUnderPlayer = groundHits[0].point.y;
    }
}
