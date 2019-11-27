using UnityEngine;
using System.Collections;

public class SteeringArrive : SteeringPriority {

	public float min_distance = 0.1f;
	public float slow_distance = 5.0f;
	public float time_to_target = 0.1f;

    public bool arrived = false;

    private RuntimeAnimatorController animator;

	Move move;

	// Use this for initialization
	void Start () { 
		move = GetComponent<Move>();
	}

	// Update is called once per frame
	void Update () 
	{
        Steer(move.target.transform.position);
	}

	public bool Steer(Vector3 target)
	{
		if(!move)
			move = GetComponent<Move>();

		// Velocity we are trying to match
		float ideal_speed = move.max_mov_speed;
		Vector3 diff = target - transform.position;

		if(diff.magnitude < min_distance)
        {
            move.SetMovementVelocity(Vector3.zero);
            arrived = true;
            return true;
        }
        arrived = false;

        // Decide which would be our ideal velocity
        if (diff.magnitude < slow_distance)
            ideal_speed = move.max_mov_speed * (diff.magnitude / slow_distance);

		// Create a vector that describes the ideal velocity
		Vector3 ideal_movement = diff.normalized * ideal_speed;

		// Calculate acceleration needed to match that velocity
		Vector3 acceleration = ideal_movement - move.current_velocity;
		acceleration /= time_to_target;

		// Cap acceleration
		if(acceleration.magnitude > move.max_mov_acceleration)
		{
			acceleration = acceleration.normalized * move.max_mov_acceleration;
		}

       // GetComponent<Animator>().speed = acceleration.normalized;

        move.AccelerateMovement(acceleration);
        return false;
	}

	void OnDrawGizmosSelected() 
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, min_distance);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, slow_distance);
	}
}
