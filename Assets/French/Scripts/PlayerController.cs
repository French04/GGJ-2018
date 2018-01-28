using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    CharacterController control;
	SpriteRenderer myRenderer;
	CapsuleCollider myCollider;
	CapsuleCollider parryCollider;
	private Animator anim;
	public Transform pivot;
	public Transform bulletPoint;
	AudioSource audioSource;
	public AudioClip[] audioClips;

	public int team;

    public InputController inputController;
    public float gravity;
    float vSpeed;
    public float maxVSpeed;

    Vector3 moveVector;
	public Vector3 lastDirection;
	Vector3 rollingDirection;
	public float moveSpeed;

	bool rolling = false;
	bool canRoll = true;
	public float rollSpeed;
	public float rollTime;
	float rollTimer;
	public float rollCoolDownTime;
	float rollCoolDownTimer;

	bool parrying = false;
	public float parryTime;
	float parryTimer;
	bool shouldParry = false;
    bool lastParryState = false;

	public bool carrying = false;
	bool shouldCarry = false;
	GameObject bulletPickUp;
	GameObject pickUpGO;

	public GameObject bulletRendererGO;
	SpriteRenderer bulletRenderer;
	Sprite bulletSprite;
	public GameObject bulletCarried;
	bool canThrow = false;
	public float throwOffset;

	int throwForce = 0;
	public float throwStepTime = 1f;
	float throwStepTimer = 0;

    BulletForce.BulletType actualBulletType;

	public GameObject[] particleCharge;
	ParticleSystem particleSmoke;

    
    public bool canMove;

    GameScore gameScore;
    PauseMenu pauseMenu;
    bool lastPauseButtonState = false;
    bool currentPauseButtonState = false;

	private void Awake()
	{
		myRenderer = GetComponentInChildren<SpriteRenderer>();
		if (Mathf.Pow(-1, team) < 0)
			myRenderer.flipX = true;
	}


	void Start ()
    {
        canMove = false;
        inputController = GetComponent<InputController>();
        control = GetComponent<CharacterController>();
		myCollider = GetComponent<CapsuleCollider>();
		parryCollider = GetComponentInChildren<CapsuleCollider>();
		anim = GetComponentInChildren<Animator>();
		particleSmoke = GetComponent<ParticleSystem>();
		bulletRenderer = bulletRendererGO.GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();

		lastDirection = new Vector3(Mathf.Pow(-1, team), 0, 0);

		throwStepTimer = throwStepTime;

        gameScore = FindObjectOfType<GameScore>();
        pauseMenu = FindObjectOfType<PauseMenu>();

    }

	void Update()
	{
        if (canMove)
        {
		    ManageInput();
		    ManageMovement();
        }
        CheckPause();
        

       
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

			if (!rolling)
			{
				if (rollCoolDownTimer > 0)
				{
					rollCoolDownTimer -= Time.deltaTime;
				}
				else
				{
					canRoll = true;
				}
			}


			if (inputController.isFiring())
			{
				if (!carrying)
				{
					if (shouldCarry)
					{
						Carry();
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

			if (inputController.isParrying() && !carrying)
			{
				Parry(true);
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
			rollCoolDownTimer = rollCoolDownTime;
			audioSource.PlayOneShot(audioClips[0]);
		}
	}


	void Carry()
	{
		carrying = true;
		bulletCarried = bulletPickUp;
		Destroy(pickUpGO);
		canThrow = false;
		bulletRenderer.sprite = bulletSprite;
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
					particleCharge[1].SetActive(false);
					particleCharge[2].SetActive(false);
					break;
				case 1:
					particleCharge[0].SetActive(true);
					particleCharge[1].SetActive(false);
					particleCharge[2].SetActive(false);
					break;
				case 2:
					audioSource.clip = audioClips[1];
					if(!audioSource.isPlaying)
						audioSource.Play();
					particleCharge[0].SetActive(false);
					particleCharge[1].SetActive(true);
					particleCharge[2].SetActive(false);
					break;
				case 3:
					audioSource.clip = audioClips[2];
					if (!audioSource.isPlaying)
						audioSource.Play();
					particleCharge[0].SetActive(false);
					particleCharge[1].SetActive(false);
					particleCharge[2].SetActive(true);
					break;
			}
		}
	}



	void Throw()
	{
		if (carrying)
		{
			GameObject i = Instantiate(bulletCarried, bulletPoint.position + lastDirection * throwOffset, transform.rotation);
			i.GetComponent<BulletForce>().Settings(lastDirection, throwForce, team, actualBulletType, bulletSprite);
			carrying = false;
			bulletCarried = null;
			throwForce = 1;
			particleCharge[0].SetActive(false);
			particleCharge[1].SetActive(false);
			particleCharge[2].SetActive(false);
			bulletRenderer.sprite = null;
			audioSource.Stop();
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
			if (lastDirection.x > 0)
			{
				myRenderer.flipX = false;
				pivot.localScale = new Vector3(1, 1, 1);
			}
			else if (lastDirection.x < 0)
			{
				myRenderer.flipX = true;
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


	public void SetShouldCarry(bool b, GameObject obj, GameObject pickup, BulletForce.BulletType bulletType, Sprite icon)
	{
		shouldCarry = b;
		bulletPickUp = obj;
		pickUpGO = pickup;
        actualBulletType = bulletType;
		bulletSprite = icon;
    }


	public bool GetIsParrying()
	{
		return parrying;
	}

    public void CheckPause() {
        currentPauseButtonState = inputController.isPause();
        if (!lastPauseButtonState && currentPauseButtonState) {
            //Triggering pause menu
            if (!gameScore.gameOver)
            {
                //Open or close pause menu
                pauseMenu.PauseSwitch(inputController);
            }
            else {
                //Reload match
                SceneManager.LoadScene(1);
            }
        }
        //Keep state for the next frame
        lastPauseButtonState = currentPauseButtonState;
    }
}
