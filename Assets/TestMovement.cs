using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour {
    InputController input;

    CharacterController control;
    SpriteRenderer renderer;

    float v;
    float h;
    [SerializeField] float gravity = 2;
    float vSpeed;
    [SerializeField] float maxVSpeed =5;

    Vector3 moveVector;
    [HideInInspector] public Vector3 lastDirection;
    [HideInInspector] public Vector3 newDirection;
    Vector3 rollingDirection;
    [SerializeField] float moveSpeed = 5;

    bool rolling = false;
    [SerializeField] float rollSpeed;
    [SerializeField] float rollTime;
    float rollTimer;

    public GameObject bullet;

    void Start()
    {
        input = GetComponent<InputController>();
        control = GetComponent<CharacterController>();
        renderer = GetComponent<SpriteRenderer>();

        lastDirection = new Vector3(-1, 0, 0);
    }

    void Update()
    {
        //Get player input
        newDirection = input.getDirection();
        

        if (input.isDashing() && !rolling)
        {
            rolling = true;
            rollTimer = rollTime;
            rollingDirection = lastDirection;
        }

        if (input.isFiring() && !rolling)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }

        ManageMovement();
    }


    void ManageMovement()
    {
        moveVector = newDirection;
        moveVector.Normalize();

        if (moveVector.magnitude > 0)
            lastDirection = moveVector;

        if (!rolling)
            moveVector *= moveSpeed;
        else
        {
            moveVector = rollingDirection * rollSpeed;
            if (rollTimer > 0)
                rollTimer -= Time.deltaTime;
            else
                rolling = false;
        }

        Gravity();

        control.Move(moveVector * Time.deltaTime);
    }


    void Gravity()
    {
        if (!control.isGrounded)
        {
            vSpeed -= gravity;
            vSpeed = Mathf.Min(vSpeed, maxVSpeed);
        }
        moveVector.y = vSpeed;
    }
}

