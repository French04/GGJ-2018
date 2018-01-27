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
	[SerializeField] Transform pivot;
	[SerializeField] Transform bulletPoint;


	public int team;


    InputController inputController;
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
	[SerializeField] float parryTime;
	float parryTimer;
	bool shouldParry = false;
    bool lastParryState = false;

	[HideInInspector] public bool carrying = false;
	bool shouldCarry = false;
	GameObject bulletPickUp;
	GameObject pickUpGO;
	Texture2D bulletIcon;
	public GameObject bulletCarried;
	bool canThrow = false;
	[SerializeField] float throwOffset;

	int throwForce = 0;
	[SerializeField] float throwStepTime = 1f;
	float throwStepTimer = 0;

    BulletForce.BulletType actualBulletType;

	[SerializeField] GameObject[] particleCharge;
	ParticleSystem particleSmoke;

    void Start ()
    {
        inputController = GetComponent<InputController>();
        control = GetComponent<CharacterController>();
		renderer = GetComponentInChildren<SpriteRenderer>();
		myCollider = GetComponent<CapsuleCollider>();
		parryCollider = GetComponentInChildren<CapsuleCollider>();
		anim = GetComponentInChildren<Animator>();
		particleSmoke = GetComponent<ParticleSystem>();

		lastDirection = new Vector3(Mathf.Pow(-1, team),0,0);
		throwStepTimer = throwStepTime;
	}

	void Update()
	{

		ManageInput();
		ManageMovement();
        
    }


	void ManageInput()
	{
		moveVector = -inputController.getDirection();

		if (!parrying)
		{
			if (inputController.isDashing())
			{
				Dash();
			}
			else
			{
				canRoll = true;
			}

			if (inputController.isFiring())
			{
				if (!carrying)
				{
					if (shouldCarry)
					{
						Carry();
					}
					else
					{
						Parry(true);
					}
				}
				else
				{
					ChargeShot();
				}
			}
			else
			{
				Throw();
			}
		}
		else
		{
			if (parryTimer > 0)
			{
				parryTimer -= Time.deltaTime;
			}
			else
			{
                Parry(false);
			}
		}
	}



	void Dash()
	{
		if (!rolling && canRoll)
		{
			rolling = true;
			particleSmoke.Play();
			rollTimer = rollTime;
			rollingDirection = lastDirection;
			canRoll = false;
			anim.SetBool("Rolling", true);
		}
	}


	void Carry()
	{
		carrying = true;
		bulletCarried = bulletPickUp;
		Destroy(pickUpGO);
		canThrow = false;
		shouldCarry = false;
	}


	void Parry(bool state)
	{
		
        if (state && !lastParryState)
        {
            parrying = true;
            anim.SetBool("Parry", true);
            parryTimer = parryTime;

        }
        else if (!state && lastParryState)
        {
            parrying = false;
            anim.SetBool("Parry", false);
        }
        lastParryState = state;
    }


	void ChargeShot()
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

			switch (throwForce)
			{
				case 0:
					particleCharge[0].SetActive(false);
					particleCharge[0].SetActive(false);
					break;
				case 1:
					particleCharge[0].SetActive(true);
					particleCharge[0].SetActive(false);
					break;
				case 2:
					particleCharge[1].SetActive(false);
					particleCharge[0].SetActive(false);
					break;
				case 3:
					particleCharge[0].SetActive(false);
					particleCharge[1].SetActive(false);
					break;
			}
		}
	}



	void Throw()
	{
		if (throwForce > 0 && carrying)
		{
			GameObject i = Instantiate(bulletCarried, bulletPoint.position + lastDirection * throwOffset, transform.rotation);
			i.GetComponent<BulletForce>().Settings(lastDirection, throwForce, team, actualBulletType);
			carrying = false;
			bulletCarried = null;
			throwForce = 0;
			anim.SetTrigger("Throw");
		}

		canThrow = true;
	}


	void ManageMovement()
	{
		moveVector.Normalize();

		if (moveVector.magnitude > 0)
		{
			lastDirection = moveVector;
			anim.SetBool("Running", true);
		}
		else
		{
			anim.SetBool("Running", false);
		}

		if (!rolling)
		{
			if (!parrying)
				moveVector *= moveSpeed;
			else
				moveVector = Vector3.zero;
		}
		else
		{
			moveVector = rollingDirection * rollSpeed;
			if (rollTimer > 0)
				rollTimer -= Time.deltaTime;
			else
			{
				rolling = false;
				anim.SetBool("Rolling", false);
				if(particleSmoke.isPlaying)
					particleSmoke.Stop();
			}
		}

		Gravity();

		if (!parrying)
		{
			if (lastDirection.x >= 0)
			{
				renderer.flipX = false;
				pivot.localScale = new Vector3(1, 1, 1);
			}
			else
			{
				renderer.flipX = true;
				pivot.localScale = new Vector3(-1, 1, 1);
			}
		}

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


	public void SetShouldCarry(bool b, GameObject obj, GameObject pickup, BulletForce.BulletType bulletType, Texture2D icon)
	{
		shouldCarry = b;
		bulletPickUp = obj;
		pickUpGO = pickup;
        actualBulletType = bulletType;
		bulletIcon = icon;
    }

    
}
