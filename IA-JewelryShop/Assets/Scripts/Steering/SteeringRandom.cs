using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringRandom : MonoBehaviour
{
    public Transform min_down;
    public Transform min_up;
    public Transform max_down;
    public Transform max_up;

    public Transform target;

    SteeringArrive arrive;

    // Start is called before the first frame update
    void Start()
    {
        arrive = GetComponent<SteeringArrive>();
        RandomizePosition();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (arrive.Steer(target.position))
        {
            RandomizePosition();
        }

    }

    private void RandomizePosition()
    {
        float x = Random.Range(min_down.position.x, max_down.position.x);
        float y = 0.0f;
        float z = Random.Range(min_down.position.z, max_up.position.z);

        target.position = new Vector3(x,y,z);
    }
}
