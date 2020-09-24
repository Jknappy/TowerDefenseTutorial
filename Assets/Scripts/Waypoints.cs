using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;//creates a field (simply a variable that is declared inside a class) of points within the Transform class(position,rotation, and scale of an object) 
    //gives the points variable the blueprints of having a Transform // this variable was created so the waypoints array can be accessed in other code.  
    void Awake()
    {
        points = new Transform[transform.childCount]; //this assigns the transform of the children object of waypoints to our new variable "points". this is an array
        for (int i = 0; i < points.Length; i++)//this loops through the array 
        {
            points[i] = transform.GetChild(i); //this makes it so the points variable is equal to what ever the next waypoint is 
        }
        
    }
}
