using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Move : SteeringPriority {

	public GameObject target;
	public GameObject self;

	public Slider arrow;

	public float max_mov_speed = 5.0f;
	public float max_mov_acceleration = 0.1f;
	public float max_rot_speed = 10.0f; // in degrees / second
	public float max_rot_acceleration = 0.1f; // in degrees


	[Header("-------- Read Only --------")]
	public Vector3 current_velocity = Vector3.zero;
	public float current_rotation_speed = 0.0f; // degrees

    
	
	// Update is called once per frame
	void Update () 
	{
		// cap velocity
		if(current_velocity.magnitude > max_mov_speed)
		{
            current_velocity = current_velocity.normalized * max_mov_speed;
            current_velocity.y = 0.0f;
		}

        // cap rotation
        current_rotation_speed = Mathf.Clamp(current_rotation_speed, -max_rot_speed, max_rot_speed);

        //  if (current_velocity.x != 0 || current_velocity.y != 0)
        //  {
        //   //      //aim.transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, angle * Mathf.Rad2Deg, Time.deltaTime * max_rot_speed), 0);

        //  }
        transform.rotation *= Quaternion.AngleAxis(current_rotation_speed * Time.deltaTime, Vector3.up);


        // finally move
        transform.position += current_velocity * Time.deltaTime;
	}

    public bool isMoving()
    {
        return current_velocity.x != 0 || current_velocity.z != 0;
    }

    // Methods for behaviours to set / add velocities
    public void SetMovementVelocity(Vector3 velocity)
    {
        current_velocity = velocity;
    }

    public void AccelerateMovement(Vector3 acceleration)
    {
        current_velocity += acceleration;
    }

    public void SetRotationVelocity(float rotation_speed)
    {
        current_rotation_speed = rotation_speed;
    }

    public void AccelerateRotation(float rotation_acceleration)
    {
        current_rotation_speed += rotation_acceleration;
    }
}

// Rotation----------------
//aim.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

//aim.transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, angle * Mathf.Rad2Deg, Time.deltaTime * max_rot_speed), 0);

// strech it
// arrow.value = current_velocity.magnitude * 4;

// final rotate
//transform.rotation *= Quaternion.AngleAxis(current_rotation_speed * Time.deltaTime, Vector3.up);

//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//public class Move : MonoBehaviour
//{

//    public GameObject target;
//    public GameObject aim;
//    public Slider arrow;

//    public float max_mov_speed = 5.0f;
//    public float max_mov_acceleration = 0.1f;
//    public float max_rot_speed = 10.0f; // in degrees / second
//    public float max_rot_acceleration = 0.1f; // in degrees

//    [Header("-------- Read Only --------")]
//    public Vector3 movement = Vector3.zero;
//    public float rotation = 0.0f; // degrees

//    // Methods for behaviours to set / add velocities
//    public void SetMovementVelocity(Vector3 velocity)
//    {
//        movement = velocity;
//    }

//    public void AccelerateMovement(Vector3 velocity)
//    {
//        movement += velocity;
//    }

//    public void SetRotationVelocity(float rotation_velocity)
//    {
//        rotation = rotation_velocity;
//    }

//    public void AccelerateRotation(float rotation_acceleration)
//    {
//        rotation += rotation_acceleration;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // cap velocity
//        if (movement.magnitude > max_mov_speed)
//        {
//            movement.Normalize();
//            movement *= max_mov_speed;
//        }

//        // cap rotation
//        Mathf.Clamp(rotation, -max_rot_speed, max_rot_speed);

//        // rotate the arrow
//        float angle = Mathf.Atan2(movement.x, movement.z);
//        //aim.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);
//        aim.transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, angle * Mathf.Rad2Deg, Time.deltaTime * max_rot_speed), 0);

//        // strech it
//        arrow.value = movement.magnitude * 4;

//        // final rotate
//        transform.rotation *= Quaternion.AngleAxis(rotation * Time.deltaTime, Vector3.up);

//        // finally move
//        movement.y = 0.0f;
//        transform.position += movement * Time.deltaTime;
//    }
//}
