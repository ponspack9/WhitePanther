using UnityEngine;

abstract public class SteeringPriority : MonoBehaviour
{
    [Range(0, SteeringConf.num_priorities)]
    public int priority = 0;

    Vector3 movement_velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}

static public class SteeringConf
{
    public const int num_priorities = 5;
}