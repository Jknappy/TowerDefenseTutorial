using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;//a singleton pattern?//this stores a reference to itself// a build manager within a build manager  
    //this makes sure that there is only one build manager in the scene
    //public static BuildManager instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = FindObjectOfType<BuildManager>();
    //        }
    //        return _instance;
    //    }
    //}

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;//creates a copy of itself inside itself?//sets the instance to this script */
    }

    public GameObject standardTurretPrefab;//this is our standard turret prefab we make a reference to 
    public GameObject anotherTurretPrefab;

    private GameObject turretToBuild;

    public GameObject GetTurretToBuild()//allows us to call this from the node script  
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret)
    {
        turretToBuild = turret; 
    }
}
