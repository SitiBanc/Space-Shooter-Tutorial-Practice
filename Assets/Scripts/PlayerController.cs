using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To make Unity Serialize the Boundary class and show it on the Inspector
[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
    // Move the player ship
    public float speed;
    public float xTilt, zTilt;
    private Rigidbody rb;
    public Boundary boundary;
    // Shoot the lazer bolt
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire = 0.0f;
    private AudioSource audioSource;
    // Set player ship under protection after respawned
    public float protectionTime;
    public float blinkRate;
    public bool isProtected;
    private float endTime;
    private Renderer rend;
    private Collider meshCollider;
    private GameObject jet_core;
    private GameObject jet_flare;

    void Start() {
        rb = GetComponent<Rigidbody>();

        rend = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        jet_core = GameObject.Find("part_jet_core");
        jet_flare = GameObject.Find("part_jet_flare");
        
        /*
         *audioSource = GetComponent<AudioSource>();
         */

        if (isProtected) {
            Debug.Log("Player respawned.");
            RespawnProtect();
        }
    }

    void Update() {
        // Forbid player from shooting during protected period
        if (!isProtected) {
            // Shoot the lazer bolt
            if (Input.GetButton("Fire1") && Time.time > nextFire) {
                nextFire = Time.time + fireRate;
                // We don't want our bolt tilt（change rotation） like our ship does and we need to  force the y position to 0.0f（tilt the ship may change the y position of the ship）
                Instantiate(shot, new Vector3(shotSpawn.position.x, 0.0f, shotSpawn.position.z), Quaternion.Euler(0.0f, 0.0f, shotSpawn.rotation.x));
                // Add sound effect
                /*
                 *audioSource.Play();
                 */
            }
        }

        if (isProtected && Time.time >= endTime) {
            CancelInvoke("Blinking");
            LeaveProtection();
        }
    }

    void FixedUpdate() {
        // Move the player ship
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.velocity = movement * speed;
        // Constrain our ship within the game area by restrict the trasnform position of the ship
        rb.position = new Vector3 (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
        // Tilt the ship while moving based on the velocity of our ship multiply the tilt factor
        rb.rotation = Quaternion.Euler(rb.velocity.z * zTilt, 0.0f, rb.velocity.x * -xTilt);
    }

    void Blinking() {
        // Add blinking effect to player ship (and the engine vfx)
        if (rend.isVisible) {
            rend.enabled = false;
            jet_core.SetActive(false);
            jet_flare.SetActive(false);
        }
        else {
            rend.enabled = true;
            jet_core.SetActive(true);
            jet_flare.SetActive(true);
        }
    }

    void RespawnProtect() {
        Debug.Log("Player is now under respawn protection!");
        // disable collider
        meshCollider.enabled = false;
        endTime = Time.time + protectionTime;
        InvokeRepeating("Blinking", 0.2f, blinkRate);
    }

    void LeaveProtection() {
        // enable collider and set player ship visible
        meshCollider.enabled = true;
        rend.enabled = true;
        jet_core.SetActive(true);
        jet_flare.SetActive(true);
        isProtected = false;
        Debug.Log("Player is now back in normal mode!");
    }
}
