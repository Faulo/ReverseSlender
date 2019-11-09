using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 300;

    private Rigidbody body;
    private Cinemachine.CinemachineVirtualCamera cam;

    private Vector3 moveVector;
    private Vector3? startingVectorMovingLeftOrRight;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void GetInput()
    {
        moveVector = new Vector3();
        bool w = Input.GetKey(KeyCode.W);
        bool s = Input.GetKey(KeyCode.S);
        bool a = Input.GetKey(KeyCode.A);
        bool d = Input.GetKey(KeyCode.D);
        if (w)
            moveVector += cam.transform.forward;
        else if (s)
            moveVector += -cam.transform.forward;
        if (a)
        {
            if (startingVectorMovingLeftOrRight.HasValue == false)
                startingVectorMovingLeftOrRight = -cam.transform.right;
            moveVector += startingVectorMovingLeftOrRight.Value;
        }
        else if (d)
        {
            if (startingVectorMovingLeftOrRight.HasValue == false)
                startingVectorMovingLeftOrRight = cam.transform.right;
            moveVector += startingVectorMovingLeftOrRight.Value;
        }
        if (a == false || d == false)
            startingVectorMovingLeftOrRight = null;
    }

    private void HandleMovement()
    {
        body.velocity = moveVector.normalized * moveSpeed * Time.fixedDeltaTime;
    }
}
