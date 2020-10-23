using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;//a singleton pattern?//this stores a reference to itself// a build manager within a build manager  
    //this makes sure that there is only one build manager in the scene

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;//creates a copy of itself inside itself?//sets the instance to this script */
    }

    
    public GameObject buildEffect;
    public GameObject sellEffect;
    public GameObject upgradeEffect;

    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    public NodeUI nodeUI; 

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

    public void SelectNode (Node node)
    {

        if (selectedNode == node)
        {
            DeselectNode();
            return; 
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node); 
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild (TurretBlueprint turret)//this is the function that is called from the shop
    {
        turretToBuild = turret;

        DeselectNode(); 
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    } 
}
