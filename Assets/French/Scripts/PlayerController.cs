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
	bool shouldParry = false;
    bool lastParryState = false;

	bool carrying = false;
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

	[SerializeField] GameObject charge1;
	[SerializeField] GameObject charge2;
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
        if (!parrying)
        {
            moveVector = -inputController.getDirection();
            if (inputController.isDashing())
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
            else
            {
                canRoll = true;
            }

            if (inputController.isFiring())
            {
                if (!carrying)
                {
                    if (shouldParry)
                    {
                        parrying = true;
                        anim.SetTrigger("Parry");
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
                    GameObject i = Instantiate(bulletCarried, bulletPoint.position + lastDirection * throwOffset, transform.rotation);
                    //i.transform.rotation = Quaternion.Euler(lastDirection);
                    i.GetComponent<BulletForce>().Settings(lastDirection, throwForce, team, actualBulletType);
                    carrying = false;
                    bulletCarried = null;
                    throwForce = 0;
                }

                moveVector = -inputController.getDirection();
                if (inputController.isDashing())
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
                            carrying = true;
                            bulletCarried = bulletPickUp;
                            Destroy(pickUpGO);
                            canThrow = false;
                            shouldCarry = false;
                        }
                        else
                        {
                            parrying = true;
                            DoParry(parrying);
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
                        GameObject i = Instantiate(bulletCarried, bulletPoint.position + lastDirection * throwOffset, transform.rotation);
                        //i.transform.rotation = Quaternion.Euler(lastDirection);
                        i.GetComponent<BulletForce>().Settings(lastDirection, throwForce, team, actualBulletType);
                        carrying = false;
                        bulletCarried = null;
                        throwForce = 0;
						anim.SetTrigger("Throw");
                    }

                    canThrow = true;
                }


            }

            if (!inputController.isFiring())
            {
                parrying = false;
                DoParry(false);
            }



            ManageMovement();
        }
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
			moveVector *= moveSpeed;
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

		if (lastDirection.x >= 0)
		{
			renderer.flipX = false;
			pivot.localScale = new Vector3(1,1,1);
		}
		else
		{
			renderer.flipX = true;
			pivot.localScale = new Vector3(-1, 1, 1);
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

    public void DoParry(bool state) {
        //TODO: Parry animation
        if (state && !lastParryState)
        {

            anim.SetTrigger("Parry");

        }
        else if (!state && lastParryState) {
            //Parry off
        }
        lastParryState = state;
    }
}
