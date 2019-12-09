using System.Collections;
using NodeCanvas.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RandomLook : ActionTask
{
    protected override void OnExecute()
    {

        agent.GetComponent<Client>().current_index = Random.Range(0, 55);
        agent.GetComponent<Client>().current_point = agent.GetComponent<Client>().client_points.GetChild(Random.Range(0, agent.GetComponent<Client>().client_points.childCount - 1));

        EndAction();
    }
}


public class Client : MonoBehaviour
{
    public bool is_buying = false;
    public bool is_leaving = false;
    public bool is_in_queue = false;
    public bool is_actually_buying = false;

    public int i = 0;
    public int j = 0;

    public Vector3 target_cashier = Vector3.zero;

    private SpriteRenderer sprite;

    public int current_index = 5;
    public Transform current_point;
    public Transform client_points;

    //public float random_chance_buy = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        current_index = Random.Range(0, 55);
        current_point = client_points.GetChild(Random.Range(0, client_points.childCount - 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (is_buying)
        {
            sprite.enabled = true;
        }
    }
}
