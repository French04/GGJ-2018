using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    Rigidbody MyRigid;
    PlayerController player;
    Vector3 direction;
    public int speed = 2;

    //CharacterController myController;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        MyRigid = GameObject.FindObjectOfType<Rigidbody>();
        MyRigid.AddForce(player.lastDirection * speed * 500);
		print(speed);
        //myController = transform.GetComponent<CharacterController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Destroy(this.gameObject);
        }
    }

    public void SetSpeed(int value)
    {
        speed = value;
    }
}
