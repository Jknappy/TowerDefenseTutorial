﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed; //public variable that initializes the speed of our enemy, public makes it so we can change it in unity  

    public float health = 100;

    public int reward = 50;

    public GameObject deathEffect;

    void Start()
    {
        speed = startSpeed; 
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow (float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die ()
    {
        PlayerStats.Money += reward;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(gameObject);                   
    }      
}
