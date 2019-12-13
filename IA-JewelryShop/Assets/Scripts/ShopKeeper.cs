using NodeCanvas.Framework;
using UnityEngine;


public class PlaceStock : ActionTask
{
    protected override void OnExecute()
    {
        //GameController.C_points.Remove(client.current_point.gameObject);
        GameController.SK_needs_restock.Remove(agent.GetComponent<ShopKeeper>().current_place_point.gameObject);

        EndAction();
    }
}
public class ShopKeeper : MonoBehaviour
{

    public bool is_reclamed = true;

    public int queue = -2;
    public Vector3 target_cashier = Vector3.zero;
    public Transform current_place_point;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
