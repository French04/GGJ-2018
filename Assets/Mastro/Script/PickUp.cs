using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	PlayerController player;
    InputController control;
    public GameObject bullet;
    public BulletForce.BulletType bulletType;
	public Sprite bulletIcon;
    MeshRenderer myRender;
    Collider myCollider;
    ParticleSystem smoke;

    public float enabledTime;
    float takeItMoment;

    bool isPickUppable = true;
    bool playAnimation = false;

    void Start()
    {
        myRender = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider>();
        smoke = GetComponent<ParticleSystem>();
        
    }

    private void Update()
    {
        if (enabledTime <= Time.time - takeItMoment && isPickUppable == false)
        {
            myCollider.enabled = true;
            myRender.enabled = true;            
            smoke.Play();
            isPickUppable = true;
            Debug.Log("Spawn");
        }      
    }
    
    void OnTriggerStay(Collider hit)
	{
		if (hit.CompareTag("Player"))
		{
            player = hit.GetComponent<PlayerController>();
            control = hit.GetComponent<InputController>();

            //if in shooting and i don't have a bullet
            if (control.isFiring() && player.carrying == false)
            {
                //if the pickup is grabbable
                if(isPickUppable)
			        player.SetShouldCarry(true, bullet, null, bulletType, bulletIcon);

                myRender.enabled = false;
                myCollider.enabled = false;
                isPickUppable = false;
                takeItMoment = Time.time;                           
            }

		}
	}
}
