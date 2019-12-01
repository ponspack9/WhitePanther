﻿using UnityEngine;
using UnityEngine.AI;

public class ShopKeeperBehaviour : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    public Transform stock_points;
    public Transform client_points;
    //private Transform[] client_points;
    //private Transform[] stock_points;

    private Transform current_point;

    public Transform cashier1;
    public Transform cashier2;

    private float time_rate = 0.5f;
    public float time_between_moves = 1.0f;

    public float time = 0.0f;
    private bool arrived = false;
    private float min_distance = 0.25f;
    private int max_stock_points = 0;
    private int max_client_points = 0;

    private bool go_cashier = true;
    public bool night = false;
    public bool restock = false;

    // Start is called before the first frame update
    void Start()
    {
        //move = GetComponent<Move>();
        //steer = GetComponent<SteeringArrive>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        max_stock_points = stock_points.childCount;
        max_client_points = client_points.childCount;
        //stock_points = new Transform[max];
        //for (int i = 0; i < max; i++)
        //{
        //    stock_points[i] = stock_pointsParent.GetChild(i).transform;
        //}

        //current_point = stock_points[Random.Range(0, max)];
        current_point = stock_points.GetChild(Random.Range(0, max_stock_points));
        agent.SetDestination(current_point.position);
        time_between_moves = Random.Range(0.5f, 1.5f);
        //move.target.transform.position = points[Random.Range(0, max)].position;
    }

    // Update is called once per frame
    void Update()
    {
        //arrived = time >= time_between_moves;
        arrived = (transform.position - agent.destination).magnitude <= min_distance;

        if (arrived)
        //if (steer.arrived)
        {
            time += time_rate * Time.deltaTime;
            agent.isStopped = true;
            animator.Play("walk", 0, 0.78f);
            animator.speed = 0.0f;
            //Quaternion.Lerp(transform.rotation, current_point.rotation, 0.5f);
            transform.rotation = current_point.rotation;
            transform.position = current_point.position;

            if (time >= time_between_moves)
            {
                if (night && !restock)
                {
                    current_point = client_points.GetChild(Random.Range(0,max_client_points));
                    agent.SetDestination(current_point.position);
                    restock = true;
                    go_cashier = false;
                }
                else if (go_cashier)
                {
                    current_point = cashier1;
                    agent.SetDestination(current_point.position);
                    go_cashier = false;
                }
                else
                {
                    //current_point = stock_points[Random.Range(0, max)];
                    current_point = stock_points.GetChild(Random.Range(0, max_stock_points));
                    agent.SetDestination(current_point.position);
                    go_cashier = true;
                    restock = false;
                }
                time_between_moves = Random.Range(0.5f, 1.5f);
                animator.speed = 1.0f;
                agent.isStopped = false;
                time = 0.0f;
            }
        }
    }
}