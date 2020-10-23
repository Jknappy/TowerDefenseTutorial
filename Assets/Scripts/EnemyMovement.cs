using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;//target will be the current waypoint being pursued by the enemy, private makes it so it can only be modified in code 
    private int wavepointIndex = 0;//this creates a variable for the enemy wave so we can destroy it once it reaches the end

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        target = Waypoints.points[0];//initializes target as a field with our waypoint class (blueprint) we made  
    }

    void Update()
    {
        Vector3 dir/*we want the direction the enemy needs to go*/ = target.position/*gets and sets targets position*/- transform.position;/*gets and sets player position*/ //this gets the direction from enemy to waypoint. to get a direction vector from one point to another you subtract them.  
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);//this moves the enemy in the determined direction at a set speed, in a space relative to world. Not sure what world refers to 

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)//checks if within .4 of the waypoint then to get next waypoint. this "<= .4" is so the computer has a range to work with instead of a single point  
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed; 
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)// just a loop to destroy the enemy once it reaches the end of the waypoints array. 
        {
            EndPath();
            return;//returns to the top 
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }

}
