using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    Rigidbody myRigid;
    PlayerController player;
    Vector3 myDirection;
    public int mySpeed = 2;
	[SerializeField] float speedMult;

    public int myTeam = 0;
    public BulletType myBulletType;

    float t;
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
        t = 0;
        player = GameObject.FindObjectOfType<PlayerController>();
		myRigid = GetComponent<Rigidbody>();

		myRigid.velocity = myDirection * mySpeed * speedMult;
        leadingDirection = transform.InverseTransformDirection(myRigid.velocity);

        score = GameObject.FindObjectOfType<GameScore>();
    }

    void FixedUpdate()
    {
        t += Time.deltaTime;
        if (myBulletType == BulletType.Raccomandata)
        {
            /*effectDirection.x = leadingDirection.x + raccomandataAmplitude * Mathf.Sin(raccomandataFrequency * t);
            effectDirection.z = leadingDirection.z + raccomandataAmplitude * Mathf.Cos(raccomandataFrequency * t);*/
			myRigid.velocity = myDirection * mySpeed * speedMult;
            //.velocity = transform.TransformDirection(effectDirection);
        }
        else if (myBulletType == BulletType.Piccione)
        {
            effectDirection.x = leadingDirection.x + pidgeonAmplitude * Mathf.Sin(pidgeonFrequency * t);
            effectDirection.z = leadingDirection.z;
            myRigid.velocity = transform.TransformDirection(effectDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Destroy(this.gameObject);
            score.UpdateScore(1);
        }
        else if (other.CompareTag("Goal2"))
        {
            Destroy(this.gameObject);
            score.UpdateScore(2);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //After a collision on the wall, update the leading direction
        leadingDirection = transform.InverseTransformDirection(myRigid.velocity);
    }

    public void Settings(Vector3 direction, int speed, int team, BulletType bulletType)
    {
        myDirection = direction;
        mySpeed = speed;
        myTeam = team;
        myBulletType = bulletType;
    }

    public int GetTeam()
    {
        return myTeam;
    }
}
