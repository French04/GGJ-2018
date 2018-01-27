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
<<<<<<< HEAD
			player.SetShouldCarry(true, hit.GetComponent<PickUp>().GetBulletGO(), gameObject);
		}*/
=======
			player.SetShouldCarry(true, hit.GetComponent<PickUp>().GetBulletGO());
		}
>>>>>>> e5f85e5cee0694f31dfd833bf8b03727a887308f
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
