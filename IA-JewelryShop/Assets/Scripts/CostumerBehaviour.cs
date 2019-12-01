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
    public Transform cashier1;
    public Transform cashier2;

    private float time_rate = 0.5f;
    public float time_between_moves = 5.0f;
    public float time_buying = 0.0f;

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
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        max_time = Random.Range(40.0f, 80.0f);

        buying = Random.Range(0, 100.0f) <= game_controller.cosutmer_buying_prob;

        max = pointsParent.childCount;
        points = new Transform[max];
        for (int i = 0; i < max; i++)
        {
            points[i] = pointsParent.GetChild(i).transform;
        }

        current_point = points[Random.Range(0, max)];
        agent.SetDestination(current_point.position);
        time_between_moves = Random.Range(1.5f, 4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        arrived = (transform.position - agent.destination).magnitude <= min_distance;
       
        max_timer += Time.deltaTime;

        if ((int)max_timer % 20 == 0)
        {
            buying = Random.Range(0, 100.0f) <= game_controller.cosutmer_buying_prob;
            max_timer += 1;
        }
        if (!leave)
            leave = !buying && max_timer >= max_time;
        
        if (leave)
        {
            agent.SetDestination(new Vector3(14,0,44));
            agent.isStopped = false;
            animator.speed = 1.0f;
        }
        else if (buying && !arrived)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            current_point = cashier1;
            agent.SetDestination(current_point.position);
            agent.isStopped = false;
            animator.speed = 1.0f;

        }
        else if (arrived)
        {
            animator.Play("walk", 0, 0.78f);
            animator.speed = 0.0f;
            //Quaternion.Lerp(transform.rotation, current_point.rotation, 1.0f);
            transform.rotation = current_point.rotation;
            transform.position = current_point.position;

            if (buying)
            {
                time_buying += Time.deltaTime;

                if (time_buying >= 6.0f)
                {
                    agent.SetDestination(new Vector3(14, 0, 44));
                    agent.isStopped = false;
                    animator.speed = 1.0f;
                    buying = false;
                    time_buying = -1.0f;
                }
            }
            else
            {
                if (time_buying < 0) leave = true;
                time += time_rate * Time.deltaTime;
                agent.isStopped = true;
                


                if (time >= time_between_moves && !buying)
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
}
