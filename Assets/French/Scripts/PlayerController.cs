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
	Vector3 rollingDirection;
	[SerializeField] float moveSpeed;

	bool rolling = false;
	[SerializeField] float rollSpeed;
	[SerializeField] float rollTime;
	float rollTimer;

	public GameObject bullet;

    void Start ()
    {
        control = GetComponent<CharacterController>();
		renderer = GetComponent<SpriteRenderer>();

		lastDirection = new Vector3(-1,0,0);
	}
	
	void Update ()
    {
        v = -Input.GetAxis("Vertical_P1");
		h = -Input.GetAxis("Horizontal_P1");

		if (Input.GetAxis("Dash_P1") > 0 && !rolling)
		{
			rolling = true;
			rollTimer = rollTime;
			rollingDirection = lastDirection;
		}

		if (Input.GetAxis("Fire_P1") > 0 && !rolling)
		{
			Instantiate(bullet, transform.position, transform.rotation);
		}

		ManageMovement();
	}


	void ManageMovement()
	{
		moveVector = new Vector3(h, 0, v);
		moveVector.Normalize();

		if(moveVector.magnitude > 0)
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
        if(!control.isGrounded)
        {
            vSpeed -= gravity;
            vSpeed = Mathf.Min(vSpeed, maxVSpeed);
        }
		moveVector.y = vSpeed;
    }
}