﻿using System.Collections;
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
		if(hit.CompareTag("Bullet") && player.GetIsParrying()) 
		{
			Debug.Log("parata");
            player.SetShouldParry(true);
            Destroy(hit.gameObject);
        }
			
	}


	void OnTriggerExit(Collider hit)
	{
		if (hit.CompareTag("Bullet"))
			player.SetShouldParry(false);
	}
}
