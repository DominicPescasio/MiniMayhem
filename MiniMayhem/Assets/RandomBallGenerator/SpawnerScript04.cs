using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript04 : MonoBehaviour
{

    public Spawn04[] spheres;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SpawnObjects();
    
    }

    void SpawnObjects()
    {
        int i  = Random.Range(0, 100);

        for (int j = 0; j < spheres.Length; j++)
        {
            if (i >= spheres[j].minProbabillityRange && i <= spheres[j].maxProbabillityRange)
            {
                Instantiate(spheres[j].spawnObject, transform.position, transform.rotation);
                break;
            }

        }
         
     }
}
[System.Serializable]
public class Spawn04
{
    public GameObject spawnObject;
    public int minProbabillityRange = 0;
    public int maxProbabillityRange = 0;
}
