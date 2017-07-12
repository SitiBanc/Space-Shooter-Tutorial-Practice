using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // We want our bolt to move along the z-axis toward the hazards that is 'transform.forward'
        rb.velocity = transform.forward * speed;
    }
}
