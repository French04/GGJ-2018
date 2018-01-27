using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour {

    public Transform player;

    Transform myTransform;

	void Start () {
        player = GetComponentInParent<Transform>();
        myTransform = GetComponent<Transform>();
    }
	

	void Update () {
        myTransform.position = new Vector3(player.position.x, 0.01f, player.position.z);
	}
}
