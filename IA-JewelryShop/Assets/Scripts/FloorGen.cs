using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGen : MonoBehaviour
{
    
    public GameObject tile;
    public Transform parent;
    Vector3 position = new Vector3(0,0,0);
    public int num_tiles_x = 5;
    public int num_tiles_y = 5;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < num_tiles_x; i++)
        {
            for (int j = 0; j < num_tiles_y; j++)
            {
                Instantiate(tile, position, Quaternion.identity,parent);
                position.z += 5;
            }
            position.z = parent.position.z;
            position.x += 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
