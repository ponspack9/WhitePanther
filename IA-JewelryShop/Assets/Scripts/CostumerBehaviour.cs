﻿using UnityEngine;

public class CostumerBehaviour : MonoBehaviour
{
    private Move move;
    private SteeringArrive steer;

    public Transform parent;
    private Transform[] points;

    int max = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        steer = GetComponent<SteeringArrive>();

        max = parent.childCount;

        points = new Transform[max];
        for (int i = 0; i < max; i++)
        {
            points[i] = parent.GetChild(i).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (steer.arrived)
        {
            move.target.transform.position = points[Random.Range(0,max)].position;
        }
    }
}