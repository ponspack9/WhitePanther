using UnityEngine;
using System.Collections;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;


[RequireComponent(typeof(BGCcMath))]
public class SteeringFollowPath : SteeringPriority
{

    Move move;
    SteeringArrive seek;

    //public float ratio_increment = 0.1f;
    //public float min_distance = 1.0f;
    //float ratio = 0.0f;

    //private float curveLength = 0.0f;

    //private Vector3 closest_point = Vector3.zero;


    public float distance = 0.0f;
    public float speed = 5.0f;

    public BGCurve curve;
    public BGCcMath math;

    // Use this for initialization
    void Start()
    {
        move = GetComponent<Move>();
        seek = GetComponent<SteeringArrive>();

        //math = curve.GetComponent<BGCcMath>();

        //path = GetComponent<BansheeGz.BGSpline.Components.BGCcMath>();
    }

    // Update is called once per frame
    void Update()
    {

        distance += speed * Time.deltaTime;

        Vector3 tangent;

        //move.target.transform.position = math.CalcPositionAndTangentByDistance(distance, out tangent);

        move.target.transform.SetPositionAndRotation(
            math.CalcPositionAndTangentByDistance(distance, out tangent),
            Quaternion.LookRotation(tangent)
            );

        //ratio += ratio_increment * Time.deltaTime;

        //int section = math.CalcSectionIndexByDistanceRatio(ratio);

        //seek.Steer(curve.Points[section].PositionWorld);

        //move.SetMovementVelocity(math.CalcPositionByDistanceRatio(ratio) - move.aim.transform.position);

        // TODO 2: Check if the tank is close enough to the desired point
        // If so, create a new point further ahead in the path
    }

    void OnDrawGizmosSelected()
    {

        if (isActiveAndEnabled)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.green;
            // Useful if you draw a sphere were on the closest point to the path
        }

    }
}
