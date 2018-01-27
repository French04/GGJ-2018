using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    Rigidbody myRigid;
    PlayerController player;
    Vector3 myDirection;
    public int mySpeed = 2;

    public int myTeam = 0;
    public BulletType myBulletType;

    float t;
    public float amplitude = 15f;
    public float frequency = 15f;

    //CharacterController myController;

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
        myRigid = GameObject.FindObjectOfType<Rigidbody>();

        if(myBulletType == BulletType.Lettera)
        {
            myRigid.velocity = myDirection * mySpeed * 10;
        }        
    }

    void FixedUpdate()
    {
        if(myBulletType == BulletType.Raccomandata)
        {
            t += Time.deltaTime;
            var locVel = transform.InverseTransformDirection(myRigid.velocity);
            locVel.x = amplitude * Mathf.Sin(frequency * t);
            locVel.z = amplitude * Mathf.Cos(frequency * t);
            myRigid.velocity = transform.TransformDirection(locVel);
        }
        else if (myBulletType == BulletType.Piccione)
        {
            t += Time.deltaTime;
            var locVel = transform.InverseTransformDirection(myRigid.velocity);
            locVel.x = amplitude * Mathf.Sin(frequency * t);
            myRigid.velocity = transform.TransformDirection(locVel);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Destroy(this.gameObject);
        }
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
