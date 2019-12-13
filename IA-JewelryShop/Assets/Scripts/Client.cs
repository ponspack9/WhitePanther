using System.Collections;
using NodeCanvas.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LeaveAngry : ActionTask
{
    protected override void OnExecute()
    {
        agent.GetComponent<Client>().sprite.sprite = Resources.Load<Sprite>("angryface");

        EndAction();
    }
}

public class PickObject : ActionTask
{
    protected override void OnExecute()
    {
        Client client = agent.GetComponent<Client>();

        //GameController.C_points.Remove(client.current_point.gameObject);
        GameController.SK_needs_restock.Add(client.current_point.gameObject);

        EndAction();
    }
}


public class Client : MonoBehaviour
{
    public bool is_buying = false;
    public bool is_first = false;
    public bool is_queue_step = false;
    public bool is_anyone_attending = false;

    public bool is_leaving = false;
    public bool is_in_queue = false;
    public bool is_actually_buying = false;

    public int queue = -1;
    public int queue_pos = -1;

    public Vector3 target_cashier = Vector3.zero;

    public SpriteRenderer sprite;

    public int current_index = 5;
    public Transform current_point;
    public Transform client_points;


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
        sprite.gameObject.transform.LookAt(Camera.main.transform);
        if (is_buying)
        {
            sprite.enabled = true;
        }
    }
}
