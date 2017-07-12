using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    // Add explosion effect upon detroying GameObject
    public GameObject explosion;
    public GameObject playerExplosion;

    // When other Collider（bolt） enter（asteroid） destroy both the bolt and the asteroid
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary")) {
            // return to Unity（end the execution this function）
            return;
        }
        // Add explosion effect
        Instantiate(explosion, transform.position, transform.rotation);
        if (other.CompareTag("Player")) {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        }
        // Destroy the other GameObject（bolt）
        Destroy(other.gameObject);
        // Destroy the GameObject this script is attached to（asteroid）
        Destroy(gameObject);
    }
}
