using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPickUp : MonoBehaviour {

    public GameObject pickUp;
    public Transform respawnPosition;
    public Collider respawnCollider;
    Transform pickUpTransform;
    public bool TakeIt;

    public float respawnTime;

    float respawnMoment;

    private void Start()
    {
        TakeIt = true;
    }

    private void Update()
    {
        if (respawnTime <= Time.time - respawnMoment && TakeIt == true)
        {
            Instantiate(pickUp, respawnPosition.position, Quaternion.identity);
            respawnMoment = Time.time;
        }

    }
}
