using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController control;
	SpriteRenderer renderer;
	CapsuleCollider myCollider;
	CapsuleCollider parryCollider;
	Animator anim;

	public int team; 

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
	bool canRoll = true;
	[SerializeField] float rollSpeed;
	[SerializeField] float rollTime;
	float rollTimer;

	bool parrying = false;
	bool shouldParry = false;

	bool carrying = false;
	bool shouldCarry = false;
	GameObject bulletPickUp;
	GameObject pickUpGO;

	public GameObject bulletCarried;
	bool canThrow = false;
	[SerializeField] float throwOffset;

	int throwForce = 0;
	[SerializeField] float throwStepTime = 1f;
	float throwStepTimer = 0;

    void Start ()
    {
        control = GetComponent<CharacterController>();
		renderer = GetComponentInChildren<SpriteRenderer>();
		myCollider = GetComponent<CapsuleCollider>();
		parryCollider = GetComponentInChildren<CapsuleCollider>();
		anim = GetComponentInChildren<Animator>();

		lastDirection = new Vector3(-1,0,0);
		throwStepTimer = throwStepTime;
	}
	
	void Update ()
    {
		if (!parrying)
		{
			v = -Input.GetAxis("Vertical_P1");
			h = -Input.GetAxis("Horizontal_P1");

			if (Input.GetAxis("Dash_P1") > 0 && !rolling && canRoll)
			{
				rolling = true;
				rollTimer = rollTime;
				rollingDirection = lastDirection;
			}

			if (Input.GetAxis("Fire_P1") > 0)
			{
				if (!carrying)
				{
					if (shouldParry)
					{
						parrying = true;
						//anima parata ecc. ecc.
					}
					else if (shouldCarry)
					{
						carrying = true;
						bulletCarried = bulletPickUp;
						Destroy(pickUpGO);
						canThrow = false;
						shouldCarry = false;
					}
				}
				else
				{
					if (!rolling)
					{
						if (throwForce == 0 && canThrow)
							throwForce = 1;
						
						if (throwForce < 3)
						{
							if (throwStepTimer > 0)
							{
								throwStepTimer -= Time.deltaTime;
							}
							else
							{
								throwForce++;
								throwStepTimer = throwStepTime;
								print("PowerUp!");
							}
						}
					}
				}
			}
			else
			{
				if (throwForce > 0 && carrying)
				{
					GameObject i = Instantiate(bulletCarried, transform.position + lastDirection * throwOffset, transform.rotation);
					//i.transform.rotation = Quaternion.Euler(lastDirection);
					i.GetComponent<BulletForce>().Settings(lastDirection, throwForce, team);
					carrying = false;
					bulletCarried = null;
					throwForce = 0;
				}

				canThrow = true;
			}


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

		if (lastDirection.x >= 0)
			renderer.flipX = false;
		else
			renderer.flipX = true;

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


	public void SetShouldParry(bool b)
	{
		shouldParry = b;
	}


	public void SetShouldCarry(bool b, GameObject obj, GameObject pickup)
	{
		shouldCarry = b;
		bulletPickUp = obj;
		pickUpGO = pickup;
	}
}