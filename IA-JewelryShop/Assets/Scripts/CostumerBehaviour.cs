using UnityEngine;
using UnityEngine.AI;

public class CostumerBehaviour : MonoBehaviour
{
    private Move move;
    private SteeringArrive steer;

    public Transform pointsParent;
    private Transform[] points;

    public NavMeshAgent agent;

    public float time_rate = 0.3f;
    public float time_between_moves = 5.0f;

    public float time = 0.0f;
    public bool arrived = false;
    public float min = 0.5f;
    int max = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        steer = GetComponent<SteeringArrive>();

        max = pointsParent.childCount;

        points = new Transform[max];
        for (int i = 0; i < max; i++)
        {
            points[i] = pointsParent.GetChild(i).transform;
        }

        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(points[Random.Range(0, max)].position);
        //move.target.transform.position = points[Random.Range(0, max)].position;
    }

    // Update is called once per frame
    void Update()
    {
        time += time_rate * Time.deltaTime;
        //arrived = time >= time_between_moves;
        arrived = (transform.position - agent.destination).magnitude <= min;

        if (arrived)
        //if (steer.arrived)
        {
            time = 0.0f;
            agent.SetDestination(points[Random.Range(0, max)].position);
        }
    }
}
