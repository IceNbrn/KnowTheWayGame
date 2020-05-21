using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public GameObject sphere;

    // Update is called once per frame
    void Update()
    {
        sphere.GetComponent<Rigidbody>().velocity = new Vector3(600f,0f,0f) * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(sphere);
    }
}
