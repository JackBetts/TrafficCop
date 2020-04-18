using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public int roadsToSpawn;
    public GameObject roadPrefab;

    private void Start()
    {
        for (int i = 0; i < roadsToSpawn; i++)
        {
            transform.Translate(new Vector3(0, 0, 200));
            Instantiate(roadPrefab, transform.position, Quaternion.identity); 
        }
    }
}
