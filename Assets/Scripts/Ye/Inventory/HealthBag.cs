﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBag : MonoBehaviour {

    GameObject player;
    // HealthSystem HBHS;
    HealthSystem PlayerHS;
    public float HealRatio = 0.2f; 

	// Use this for initialization
	void Start () {
        // HBHS = GetComponent<HealthSystem>();
        // PlayerHS = player.GetComponent<HealthSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        //PlayerHS = player.GetComponent<HealthSystem>();
        /*
        if (HBHS.objHealth == 0)
        {
            PlayerHS.Heal( HealRatio * PlayerHS.maxHealth );
        }
        */
    }

    // when collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // heal
        PlayerHS = collision.gameObject.GetComponent<HealthSystem>();
        PlayerHS.Heal(HealRatio * PlayerHS.maxHealth);
        /*
        PlayerHS.Heal(HealRatio * PlayerHS.maxHealth);
        */

        // destory
        Destroy(gameObject);
    }
}