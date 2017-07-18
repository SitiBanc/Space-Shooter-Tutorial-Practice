using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
    public float lifetime;

    // Wait for 'lifetime' after the gameObject is instantiated than destroy it
    void Start() {
        Destroy(gameObject, lifetime);
    }
}
