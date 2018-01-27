using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour {

	PlayerController player;


	void Start()
	{
		player = GetComponentInParent<PlayerController>();
	}


	void OnTriggerEnter(Collider hit)
	{
		if(hit.CompareTag("Bullet"))
			player.SetShouldParry(true);
		
		/*if (hit.CompareTag("PickUp"))
		{
			player.SetShouldCarry(true, hit.GetComponent<PickUp>().GetBulletGO(), gameObject);
		}*/
	}


	void OnTriggerExit(Collider hit)
	{
		if (hit.CompareTag("Bullet"))
			player.SetShouldParry(false);

		/*if (hit.CompareTag("PickUp"))
		{
			player.SetShouldCarry(false, null, gameObject);
		}*/
	}
}
