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

    Vector3 MoveVector;


    // Use this for initialization
    void Start ()
    {
        control = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        transform.position += new Vector3(-v, 0, h);
	}

    void Gravity()
    {
        if(!control.isGrounded)
        {
            vSpeed -= gravity;
            vSpeed = Mathf.Min(vSpeed, maxVSpeed);
        }
    }
}