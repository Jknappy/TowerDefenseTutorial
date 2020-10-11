using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//mouse hover color, user input, keeps track of node if turret already exists, allows input to build turret.  
public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;//the position the turret is spawned on the node 

    private GameObject turret;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager; 


    void Start()
    {
        rend = GetComponent<Renderer>();//storing this method in the rend field
        startColor = rend.material.color;//gets the defualt start material color

        buildManager = BuildManager.instance; 
    } 

    void OnMouseDown()//when pressed down while hovering over the node
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetTurretToBuild() == null)
            return;

        if (turret != null)//if turret exists 
        {
            Debug.Log("Can't build there! - TODO: Display on screen.");
            return;
        }

        GameObject turretToBuild = buildManager.GetTurretToBuild();// goes into gamemanager and gets the getturrettobuild method 
        turret = Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);//builds turret at the selected node location 
    }

    void OnMouseEnter()// this is a callback function like start or update. this is called everytime the mouse enters the confines of the collider of the object.
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetTurretToBuild() == null)
            return;
        
        rend.material.color = hoverColor;//sets the color to our public mouse hover color 
    }

    private void OnMouseExit()//when mouse exits the collider 
    {
       rend.material.color = startColor;//sets color back to start color when mouse isnt hovering over the object 
    }
}
