using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	PlayerController player;
    public GameObject bullet;

   
	void Start()
	{
		player = GameObject.FindObjectOfType<PlayerController>();
	}


	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
			player.SetShouldCarry(true, bullet, gameObject);
		}
	}

	void OnTriggerExit(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
			player.SetShouldCarry(false, null, gameObject);
		}
	}
}
