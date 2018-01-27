using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonMovement : MonoBehaviour {

    Rigidbody rig;
    float t;
    public float amplitude = 15f;
    public float frequency = 15f;
    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        t = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t += Time.deltaTime;
        var locVel = transform.InverseTransformDirection(rig.velocity);
        locVel.x = amplitude * Mathf.Sin(frequency * t);
        rig.velocity = transform.TransformDirection(locVel);
    }

}
