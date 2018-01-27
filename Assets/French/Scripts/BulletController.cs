using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject linebullet;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            Instantiate(linebullet, transform.position, transform.rotation);
        }
	}
}
