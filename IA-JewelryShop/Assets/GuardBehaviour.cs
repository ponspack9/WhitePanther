using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    public Transform pointsParent;
    private Transform[] points;
    private Transform current_point;
    private int iterator = 0;


    private float time_rate = 0.5f;
    public float time_between_moves = 2.0f;

    public float time = 0.0f;
    private bool arrived = false;
    private float min_distance = 0.25f;
    private int max = 0;

    // Start is called before the first frame update
    void Start()
    {
        //move = GetComponent<Move>();
        //steer = GetComponent<SteeringArrive>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        max = pointsParent.childCount;
        points = new Transform[max];
        for (int i = 0; i < max; i++)
        {
            points[i] = pointsParent.GetChild(i).transform;
        }

        current_point = points[iterator++];
        agent.SetDestination(current_point.position);
        time_between_moves = Random.Range(1.5f, 2.5f);
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
            animator.speed = 0.0f;
            //Quaternion.Lerp(transform.rotation, current_point.rotation, 0.5f);
            transform.rotation = current_point.rotation;
            transform.position = current_point.position;

            if (time >= time_between_moves)
            {
                time = 0.0f;
                current_point = points[iterator++];
                agent.SetDestination(current_point.position);
                time_between_moves = Random.Range(1.5f, 2.5f);
                agent.isStopped = false;
                animator.speed = 1.0f;

            }
        }
    }
}
