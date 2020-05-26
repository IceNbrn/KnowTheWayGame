using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public GameObject turret;

    // Start is called before the first frame update
    void Start()
    {        
        InvokeRepeating("SpawnObject", 1, 0.5f);
    }

    void SpawnObject()
    {   
        float x = turret.transform.position.x;
        float y = turret.transform.position.y;
        float z = turret.transform.position.z;

        Instantiate(target, new Vector3(x, y, z), Quaternion.identity);
    }
}
