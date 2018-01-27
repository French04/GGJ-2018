using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	PlayerController player;
    InputController control;
    public GameObject bullet;
    public BulletForce.BulletType bulletType;
	public Texture2D bulletIcon;
    MeshRenderer myRender;

    public float enabledTime;
    float takeItMoment;
    float startAnimation;

    bool isPickUppable = true;

    void Start()
    {
        myRender = GetComponent<MeshRenderer>();
        startAnimation = enabledTime - 1f;
    }

    private void Update()
    {
        if (enabledTime <= Time.time - takeItMoment && myRender.enabled == false)
        {            
            myRender.enabled = true;
            isPickUppable = true;
        }
        if (startAnimation <= Time.time - takeItMoment && myRender.enabled == false)
        {
            
            
        }
        
    }
    
    void OnTriggerStay(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
            player = hit.GetComponent<PlayerController>();
            control = hit.GetComponent<InputController>();

            if(isPickUppable)
			    player.SetShouldCarry(true, bullet, gameObject, bulletType, bulletIcon);

            if (player.carrying == true && control.isFiring())
            {
                myRender.enabled = false;
                isPickUppable = false;
                takeItMoment = Time.time;
            }

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
