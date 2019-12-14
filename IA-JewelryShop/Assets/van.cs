using NodeCanvas.Framework;
using UnityEngine;


public class LeaveDeliveryGuy : ActionTask
{
    protected override void OnExecute()
    {
        van _van = agent.GetComponentInParent<van>();

        _van.can_leave = true;

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
                transform.GetChild(0).gameObject.SetActive(true);
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
