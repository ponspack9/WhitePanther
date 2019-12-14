using NodeCanvas.Framework;
using UnityEngine;


public class SpawnDelieveryGuy : ActionTask
{
    protected override void OnExecute()
    {
        Client client = agent.GetComponent<Client>();

        //GameController.C_points.Remove(client.current_point.gameObject);
        GameController.SK_needs_restock.Add(client.current_point.gameObject);

        EndAction();
    }
}

public class van : MonoBehaviour
{
    public bool can_leave = false;
    private bool coming = true;

    private float speed = 6.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("rot: " + transform.rotation.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        if (coming)
        {
            //Vector3 p = transform.position;
            transform.Translate(0,0,-speed * Time.deltaTime);

            if (transform.position.x >= 9)
            {
                coming = false;
                //Instantiate guy
            }
        }
        else if (can_leave)
        {
            transform.Translate(0,0, -speed * Time.deltaTime);

            if (transform.position.x >= 130)
            {
                Destroy(gameObject);
            }
        }
    }
}
