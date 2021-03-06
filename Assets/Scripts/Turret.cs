﻿using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour//Objective: find a target, within a range and use nearest target. rotate to aim at the target.
{
    private Transform target;//our nearest enemy in range
    private Enemy targetEnemy;

    [Header("General")]

    public float range = 15f;//our range distance

    [Header("Use Bullets (default)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;

    [Header("Use Laser")]
    public bool useLaser = false;

    public int damageOverTime = 30;
    public float slowpct = .5f; 

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setup Fields")]

    public string enemyTag = "enemy";//Tags with the name "enemy" can now be referenced in code
    public Transform partToRotate;// a empty parent object to rotate the turret head 
    public float turnSpeed = 10f;// speed the turret rotates
   
    public Transform firePoint; 

 

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);//this makes sure search for target only happens every .5s
    }

    void UpdateTarget()//find an enemy and make it a target
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);//finds all of our enemies/ this grabs our enemy objects and creates an array called enemies
        float shortestDistance = Mathf.Infinity;//if we havent found an enemy we have an infinite distance to that enemy?
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);// this finds distance between turret and enemy and stores it in the float variable 
            if (distanceToEnemy < shortestDistance)//checks to see if that distance is shorter than any we found before
            {
                shortestDistance = distanceToEnemy;//if it is we want to set the shortest distance equal to this distance
                nearestEnemy = enemy;// then tell the computer the nearest enemy we found is this nearest one
            }
        }

        if (nearestEnemy != null/*if we have found an enemy*/ && shortestDistance <= range)//if enemy is in our range
        {
            target = nearestEnemy.transform;//nearest enemy is now the target
            targetEnemy = nearestEnemy.GetComponent<Enemy>(); 
        }
        else
        {
            target = null;//once target is no longer in range target is null
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false; 
                }
                    
            }

            return;//if we dont have a target, dont do anything.
        }
            

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        } 
    }

    void LockOnTarget()
    {
        //target lock on with rotation 
        Vector3 dir = target.position - transform.position;     //to find direction between to points you subtract the end point by the start point (B - A)
        Quaternion lookRotation = Quaternion.LookRotation(dir);//how to rotate to look that direction
        Vector3 rotation = Quaternion.Lerp/*smooths the transition between states*/(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//euler angles are the x,y,and z rotation.
        //converts the Vector3 rotation to eulerAngles so we can decide to rotate only on the y axis 
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser ()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowpct);//he has slowAmount here and idk why its throwing an error for me but slowpct seems to work the same for now

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true; 
        }
            
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized; 

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);
    }

    void OnDrawGizmosSelected()//draws a visual range for our turret when its selected
    {
        Gizmos.color = Color.red;//this makes the range red
        Gizmos.DrawWireSphere(transform.position, range);//this draws a wire sphere 
    }
}
