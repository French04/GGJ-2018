using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	PlayerController player;
    public GameObject bullet;


	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
			player = hit.GetComponent<PlayerController>();
			player.SetShouldCarry(true, bullet, gameObject);
		}
	}

	void OnTriggerExit(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
			//player = hit.GetComponent<PlayerController>();
			player.SetShouldCarry(false, null, gameObject);
		}
	}
}
