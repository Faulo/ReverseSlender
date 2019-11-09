using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 300;
    [SerializeField] private float maxDistanceAboveGround = 5f;
    [SerializeField] private float minDistanceAboveGround = 0f;
    private float terrainHeightUnderPlayer;

    private Rigidbody body;
    private Cinemachine.CinemachineVirtualCamera cam;

    private Vector3 moveVector;
    private Vector3? startingVectorMovingLeft;
    private Vector3? startingVectorMovingRight;

    private RaycastHit[] groundHits = new RaycastHit[1];

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInput();
        CheckForGround();
    }

    private void FixedUpdate()
    {
        HandleMovement();
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
        Vector3 targetPos = body.position + moveVector.normalized * moveSpeed * Time.fixedDeltaTime;
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
