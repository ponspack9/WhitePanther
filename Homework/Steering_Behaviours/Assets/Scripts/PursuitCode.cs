using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitCode : MonoBehaviour
{
    public float maxSpeed;
    public float maxForce;

    private Vector3 pos = new Vector3();
    private Vector3 vel = new Vector3();

    private Rigidbody enemy_b;
    private Transform enemy_t;
    private GameObject target = new GameObject();

    // Start is called before the first frame update
    private void Start()
    {
        enemy_b = GetComponent<Rigidbody>();
        enemy_t = GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player");
        SetAll(); //Want to call it before first frame, in order to avoid problems
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        SetAll();
        Seek(target.transform.position);
    }    

    void SetAll()
    {
        pos.Set(enemy_t.position.x, enemy_t.position.y, enemy_t.position.z);
        vel.Set(enemy_b.velocity.x, enemy_b.velocity.y, enemy_b.velocity.z);        
    }

    void Seek(Vector3 objective)
    {
        Vector3 desired = new Vector3(objective.x - pos.x, 0.0f, objective.z - pos.z);

        desired = Vector3.Normalize(desired);
        desired *= maxSpeed;

        Vector3 steer = new Vector3(desired.x - vel.x, 0.0f, desired.z - vel.z);

        enemy_b.AddForce(steer);
    }
}
