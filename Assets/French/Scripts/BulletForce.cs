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

    //CharacterController myController;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        myRigid = GameObject.FindObjectOfType<Rigidbody>();
        myRigid.AddForce(myDirection * mySpeed * 500);
        //myController = transform.GetComponent<CharacterController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Destroy(this.gameObject);
        }
    }

    public void Settings(Vector3 direction, int speed, int team)
    {
        myDirection = direction;
        mySpeed = speed;
        myTeam = team;
    }
}
