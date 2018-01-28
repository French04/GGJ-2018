using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    Rigidbody myRigid;
    PlayerController player;
	SpriteRenderer renderer;
	Sprite icon;
	AudioSource audioSource;
	[SerializeField] AudioClip[] audioClips;
	[SerializeField] GameObject scoreParticle;

    Vector3 myDirection;
    public int mySpeed = 2;
	[SerializeField] float speedMult;
	[SerializeField] float speedBase;
	[SerializeField] float lifeTime;

    public int myTeam = 0;
    public BulletType myBulletType;

    public float pidgeonAmplitude = 15f;
    public float pidgeonFrequency = 15f;
    public float raccomandataAmplitude = 15f;
    public float raccomandataFrequency = 15f;

    private Vector3 leadingDirection; //Direction on which calculate trajectories perturbation
    private Vector3 effectDirection; //Actual velocity
    //CharacterController myController;

    GameScore score;

    public enum BulletType
    {
        Raccomandata,
        Lettera,
        Piccione
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
		myRigid = GetComponent<Rigidbody>();
		renderer = GetComponent<SpriteRenderer>();
        leadingDirection = transform.InverseTransformDirection(myRigid.velocity);
		myRigid.velocity = myDirection * speedBase + myDirection * mySpeed * speedMult;
        score = GameObject.FindObjectOfType<GameScore>();
		transform.rotation = Quaternion.Euler(-60, 0, 0);
		audioSource = GetComponent<AudioSource> ();

		switch (myBulletType)
		{
			case BulletType.Lettera:
				audioSource.PlayOneShot(audioClips[0 + (mySpeed-1)]);
				break;
			case BulletType.Raccomandata:
				audioSource.PlayOneShot(audioClips[3 + (mySpeed - 1)]);
				break;
			case BulletType.Piccione:
				audioSource.PlayOneShot(audioClips[6 + (mySpeed - 1)]);
				break;
		}
    }

    void FixedUpdate()
    {
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0)
			Destroy(gameObject);

		if (myBulletType == BulletType.Piccione)
        {
			myRigid.velocity = myDirection * speedBase + myDirection * mySpeed * speedMult;
			myRigid.velocity += new Vector3(myDirection.z, 0, -myDirection.x) * Mathf.Sin(Time.time * raccomandataFrequency) * raccomandataAmplitude;
        }

		else if (myBulletType == BulletType.Raccomandata)
        {
			myRigid.velocity = myDirection * speedBase + myDirection * mySpeed * speedMult;
			myRigid.velocity += new Vector3(myDirection.x, 0, myDirection.z) * Mathf.Sin(Time.time * raccomandataFrequency) * raccomandataAmplitude;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
			Instantiate(scoreParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            score.UpdateScore(1);
        }
        else if (other.CompareTag("Goal2"))
        {
			Instantiate(scoreParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            score.UpdateScore(2);
        }
    }

	public void OnCollisionEnter(Collision collision)
    {
		//After a collision on the wall, update the leading direction
		myBulletType = BulletType.Lettera;
		leadingDirection = transform.InverseTransformDirection(myRigid.velocity);

		print("It just works");
    }

	public void Settings(Vector3 direction, int speed, int team, BulletType bulletType, Sprite s)
    {
        myDirection = direction;
        mySpeed = speed;
        myTeam = team;
        myBulletType = bulletType;
		icon = s;
		renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = icon;
		if (myDirection.x < 0)
			renderer.flipX = true;
    }

    public int GetTeam()
    {
        return myTeam;
    }
}
