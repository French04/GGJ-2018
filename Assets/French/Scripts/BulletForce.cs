using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletForce : MonoBehaviour
{
    PlayerController player;
    Vector3 direction;
    public int speed = 2;

    private void OnEnable()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        direction = player.lastDirection;
    }

    private void Update()
    {
        transform.position += direction * Time.deltaTime * speed; 
    }
}
