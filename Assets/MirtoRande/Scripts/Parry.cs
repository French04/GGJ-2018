using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour {

	PlayerController player;
	public GameObject hitSpark;


	void Start()
	{
		player = GetComponentInParent<PlayerController>();
	}


	void OnTriggerEnter(Collider hit)
	{
		if(hit.CompareTag("Bullet") && player.GetIsParrying()) 
		{
            player.SetShouldParry(true);
            Destroy(hit.gameObject);
			Instantiate(hitSpark, hit.gameObject.transform.position, Quaternion.identity);
        }
			
	}


	void OnTriggerExit(Collider hit)
	{
		if (hit.CompareTag("Bullet"))
			player.SetShouldParry(false);
	}
}
