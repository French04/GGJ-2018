using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController control;

    float v;
    float h;
    [SerializeField] float gravity;
    float vSpeed;
    [SerializeField] float maxVSpeed;

    Vector3 moveVector;
	[SerializeField] float moveSpeed;


    void Start ()
    {
        control = GetComponent<CharacterController>();
	}
	
	void Update ()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

		ManageMovement();
	}


	void ManageMovement()
	{
		moveVector = new Vector3(h, 0, v);
		moveVector.Normalize();
		moveVector *= moveSpeed;

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