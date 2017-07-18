using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    // Add explosion effect upon detroying GameObject
    public GameObject selfExplosion;
    public GameObject playerExplosion;
    // Add score after destroy the asteroid
    public int scoreValue;  // The score of destroying an asteroid
    private GameController gameController;  // Hold reference to GameController object

    /* 
     * Since this script is attached to a prefab（asteroid）
     * we can't reference a GameController instance in the inspector,
     * we must find the GameController instance upon each asteroid instanciated
     */
    void Start() {
        // Find the GameController instance through tag
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        // Just in case something goes wrong and we can't find the gameController
        else {
            Debug.Log("Cannot find 'GameController' script.");
        }
    }
     
    void OnTriggerEnter(Collider other) {
        // Ignore the collison with boundary or enemy（collision between asteroid and enemy  bolt or enemy ship）
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy")) {
            // return to Unity（end the execution this function）
            return;
        }

        /* 
         * Add self explosion effect if there's one
         * When this script is attached to asteroid or enemy ship（not enemy bolt）
         */
        if (selfExplosion != null) {
            Instantiate(selfExplosion, transform.position, transform.rotation);
        }

        // Add explosion effect of player ship and execute the GameController.GameOver() function if （asteroid） collide to player ship
        if (other.CompareTag("Player")) {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            Debug.Log("Destroyed by " + gameObject.name + " .");
            gameController.GameOver();
        }

        // Add score
        gameController.UpdateScore(scoreValue);

        // Destroy the other GameObject（bolt）
        Destroy(other.gameObject);

        // Destroy the GameObject this script is attached to（asteroid）
        Destroy(gameObject);
    }
}
