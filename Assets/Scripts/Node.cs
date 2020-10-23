using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//mouse hover color, user input, keeps track of node if turret already exists, allows input to build turret.  
public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;

    public Vector3 positionOffset;//the position the turret is spawned on the node 

    [HideInInspector]
    public GameObject turret;

    [HideInInspector]
    public TurretBlueprint turretBlueprint;

    [HideInInspector]
    public bool isUpgraded = false; 

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager; 


    void Start()
    {
        rend = GetComponent<Renderer>();//storing this method in the rend field
        startColor = rend.material.color;//gets the defualt start material color

        buildManager = BuildManager.instance; 
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    void OnMouseDown()//when pressed down while hovering over the node
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;       

        if (turret != null)//if turret exists 
        {
            buildManager.SelectNode(this); 
            return;
        }

        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret (TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money to build that!");
        }

        PlayerStats.Money -= blueprint.cost;

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret build!" + PlayerStats.Money);
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        //get rid of the old turret 
        Destroy(turret); 

        //building a new one 
        GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject effect = Instantiate(buildManager.upgradeEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true; 

        Debug.Log("Turret upgraded!");
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        //Spawn a cool effect
        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null; 
    }

    void OnMouseEnter()// this is a callback function like start or update. this is called everytime the mouse enters the confines of the collider of the object.
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;//sets the color to our public mouse hover color
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
        
         
    }

    private void OnMouseExit()//when mouse exits the collider 
    {
       rend.material.color = startColor;//sets color back to start color when mouse isnt hovering over the object 
    }
}
