using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	PlayerController player;
    public GameObject bullet;
    public BulletForce.BulletType bulletType;
	public Texture2D bulletIcon;

	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
			player = hit.GetComponent<PlayerController>();
			player.SetShouldCarry(true, bullet, gameObject, bulletType, bulletIcon);
		}
	}

	void OnTriggerExit(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
			player.SetShouldCarry(false, null, gameObject, bulletType, bulletIcon);
		}
	}
}
