using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {
    //public float dodge; // Max dodge distance
    public float smoothing;
    public float tilt;
    // Use Vector2 to effectively store min and max value（for Random.Range()）
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Boundary boundary;

    private float targetManeuver;   // Target maneuver velocity
    private Rigidbody rb;
    private Transform playerTransform;

	void Start () {
        rb = GetComponent<Rigidbody>();
        // Find player GameObjecct after the instance is istantiated
        if (GameObject.FindWithTag("Player") != null)
            playerTransform = GameObject.FindWithTag("Player").transform;
        else
            playerTransform = transform;
        StartCoroutine(Evade());
	}
	
    IEnumerator Evade() {
        // After the enemy ship is spawned, wait for a random amount of time to start maneuvering
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        // Keep maneuvering after a random amount of time
        while (true) {
            // Maneuver toward the player ship
            /* 
             * Randomly select a targetManeuver
             * Using -Mathf.Sign(transform.position.x) to avoid enemy ship from moving toward the edge of our game area
             * But by doing so, the ship will always moves toward the center of our game area
             * targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
             */
            // Check if playerTransform is null（destroyed）
            if (playerTransform == null) {
                // Set targetManeuver to enemy ship's position（don't move） and break out the loop
                targetManeuver = transform.position.x;
                break;
            }
            targetManeuver = playerTransform.position.x;
            // Randomly select the maneuver time（how long the maneuvering will last）
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            // Reset the targetManeuver to zero thus stop the ship from maneuvering（till the next loop）
            targetManeuver = 0;
            // Wait for a random amount of time after maneuvering
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

	void FixedUpdate () {
        // Smoothly move the current speed toward the targetManeuver
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManeuver, 0.0f, rb.velocity.z);
        // Clamp the ship inside the game area（safety check）
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
        // Tilt the ship while moving based on the velocity of the ship multiply the tilt factor
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
