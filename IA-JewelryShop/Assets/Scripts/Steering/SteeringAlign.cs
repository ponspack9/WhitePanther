using UnityEngine;
using UnityEngine.AI;

public class SteeringAlign : MonoBehaviour {

	private float min_angle = 0.5f;
	private float slow_angle = 5f;
    public float rot_speed = 30.0f;

	Move move;
    private CostumerBehaviour cb;

    // Use this for initialization
    void Start () {
        cb = GetComponent<CostumerBehaviour>();
    }

	// Update is called once per frame
	void Update () 
	{
        if (cb.arrived)
        {
            //float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
            float angle = Mathf.Atan2(cb.agent.velocity.x, cb.agent.velocity.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up), Time.deltaTime * rot_speed);

        }
        //else
        //{
        //    // Orientation we are trying to match
        //    float delta_angle = Vector3.SignedAngle(transform.forward, move.target.transform.forward, new Vector3(0.0f, 1.0f, 0.0f));

        //    float diff_absolute = Mathf.Abs(delta_angle);

        //    if (diff_absolute <= min_angle)
        //    {
        //        move.SetRotationVelocity(0.0f);
        //        return;
        //    }

        //    float ideal_rotation_speed = move.max_rot_speed;

        //    if (diff_absolute <= slow_angle)
        //        ideal_rotation_speed *= (diff_absolute / slow_angle);

        //    //float angular_acceleration = ideal_rotation_speed / time_to_accel;

        //    //Invert rotation direction if the angle is negative
        //    if (delta_angle < 0)
        //        ideal_rotation_speed = -ideal_rotation_speed;

        //    move.AccelerateRotation(Mathf.Clamp(ideal_rotation_speed, -move.max_rot_acceleration, move.max_rot_acceleration));
        //}
	}
}
