using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    public Transform points;
    private Transform current_point;
    private int iterator = 0;

    private float time_rate = 0.5f;
    public float time_between_moves = 2.0f;

    public float time = 0.0f;
    private bool arrived = false;
    private float min_distance = 0.25f;
    private int max = 0;

    public bool quiet = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        max = points.childCount;

        current_point = points.GetChild(iterator++);
        agent.SetDestination(current_point.position);
        time_between_moves = Random.Range(1.5f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        arrived = (transform.position - agent.destination).magnitude <= min_distance;

        if (quiet && ! arrived)
        {
            current_point = points.GetChild(0);
            agent.SetDestination(current_point.position);
            iterator = 0;
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

            if (!quiet && time >= time_between_moves)
            {
                time = 0.0f;
                current_point = points.GetChild(iterator++);
                if (iterator >= max) iterator = 0;
                agent.SetDestination(current_point.position);
                time_between_moves = Random.Range(1.5f, 2.5f);
                agent.isStopped = false;
                animator.speed = 1.0f;

            }
        }
    }
}
