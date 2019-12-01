using UnityEngine;
using UnityEngine.AI;

public class ShopKeeperBehaviour : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    public GameController game_controller;

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

    public bool go_cashier = true;
    public bool restock = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        max_stock_points = stock_points.childCount;
        max_client_points = client_points.childCount;

        current_point = stock_points.GetChild(Random.Range(0, max_stock_points));
        agent.SetDestination(current_point.position);
        time_between_moves = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

        arrived = (transform.position - agent.destination).magnitude <= min_distance;
        if (go_cashier)
        {
            current_point = cashier1;
            agent.SetDestination(current_point.position);
            agent.isStopped = false;
        }
        if (arrived)
        {
            time += time_rate * Time.deltaTime;
            agent.isStopped = true;
            animator.Play("walk", 0, 0.78f);
            animator.speed = 0.0f;
            //Quaternion.Lerp(transform.rotation, current_point.rotation, 0.5f);
            transform.rotation = current_point.rotation;
            transform.position = current_point.position;

            if (!game_controller.force_to_cashier && time >= time_between_moves)
            {
                if (!restock)
                {
                    current_point = client_points.GetChild(Random.Range(0,max_client_points));
                    agent.SetDestination(current_point.position);
                    restock = true;
                }
                else
                {
                    //current_point = stock_points[Random.Range(0, max)];
                    current_point = stock_points.GetChild(Random.Range(0, max_stock_points));
                    agent.SetDestination(current_point.position);
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
