using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController control;
	SpriteRenderer renderer;

    float v;
    float h;
    [SerializeField] float gravity;
    float vSpeed;
    [SerializeField] float maxVSpeed;

    Vector3 moveVector;
	[HideInInspector] public Vector3 lastDirection;
	[SerializeField] float moveSpeed;

	bool rolling = false;
	[SerializeField] float rollSpeed;
	[SerializeField] float rollTime;
	float rollTimer;

    void Start ()
    {
        control = GetComponent<CharacterController>();
		renderer = GetComponent<SpriteRenderer>();

		lastDirection = new Vector3(-1,0,0);
	}
	
	void Update ()
    {
        v = -Input.GetAxis("Vertical");
        h = -Input.GetAxis("Horizontal");

		if (Input.GetAxis("Jump") > 0)
		{
			rolling = true;
			rollTimer = rollTime;
		}

		ManageMovement();
	}


	void ManageMovement()
	{
		moveVector = new Vector3(h, 0, v);
		moveVector.Normalize();

		if(moveVector.magnitude > 0 && !rolling)
			lastDirection = moveVector;

		if (!rolling)
			moveVector *= moveSpeed;
		else
		{
			moveVector = lastDirection * rollSpeed;
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
        if(!control.isGrounded)
        {
            vSpeed -= gravity;
            vSpeed = Mathf.Min(vSpeed, maxVSpeed);
        }
		moveVector.y = vSpeed;
    }
}