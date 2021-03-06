﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    // After the Enemy Ship is spawned, wait for 'delay' then continuously fire() at fireRate
    public GameObject shot;
    public Transform shotSpawn;
    public float delay;
    public float fireRate;

	// Use this for initialization
	void Start () {
        // Call Fire() function after 'delay' at the rate of 'fireRate'
        InvokeRepeating("Fire", delay, fireRate);
	}

    void Fire() {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }
}
