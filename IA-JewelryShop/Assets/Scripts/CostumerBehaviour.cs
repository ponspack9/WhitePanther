using UnityEngine;
using UnityEngine.AI;

public class CostumerBehaviour : MonoBehaviour
{
    //private Move move;
    //private SteeringArrive steer;
    private Animator animator;
    private NavMeshAgent agent;

    public GameController game_controller;

    public Transform pointsParent;
    private Transform[] points;
    private Transform current_point;

    private float time_rate = 0.5f;
    public float time_between_moves = 5.0f;

    public float time = 0.0f;
    public bool arrived = false;
    private float min_distance = 0.25f;
    private int max = 0;

    private float max_timer = 0.0f;
    private float max_time = 0;

    public bool leave = false;
    public bool buying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //move = GetComponent<Move>();
        //steer = GetComponent<SteeringArrive>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        max_time = Random.Range(10.0f, 20.0f);

        max = pointsParent.childCount;
        points = new Transform[max];
        for (int i = 0; i < max; i++)
        {
            points[i] = pointsParent.GetChild(i).transform;
        }

        current_point = points[Random.Range(0, max)];
        agent.SetDestination(current_point.position);
        time_between_moves = Random.Range(1.5f, 4.5f);
        //move.target.transform.position = points[Random.Range(0, max)].position;
    }

    // Update is called once per frame
    void Update()
    {
        //arrived = time >= time_between_moves;
        arrived = (transform.position - agent.destination).magnitude <= min_distance;
        max_timer += Time.deltaTime;
        if (!leave)
            leave = !buying && max_timer >= max_time;

        if (leave)
        {
            agent.SetDestination(new Vector3(14,0,44));
            agent.isStopped = false;
            animator.speed = 1.0f;
        }
        else if (arrived)
        //if (steer.arrived)
        {
            time += time_rate * Time.deltaTime;
            agent.isStopped = true;
            animator.Play("walk", 0, 0.78f);
            animator.speed = 0.0f;
            //Quaternion.Lerp(transform.rotation, current_point.rotation, 1.0f);
            transform.rotation = current_point.rotation;
            transform.position = current_point.position;

            if (time >= time_between_moves)
            {
                time = 0.0f;
                current_point = points[Random.Range(0, max)];
                agent.SetDestination(current_point.position);
                time_between_moves = Random.Range(1.5f, 4.5f);
                agent.isStopped = false;
                animator.speed = 1.0f;

            }
        }
    }
}
