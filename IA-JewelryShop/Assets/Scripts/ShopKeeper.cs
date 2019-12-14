using NodeCanvas.Framework;
using UnityEngine;


public class PlaceStock : ActionTask
{
    protected override void OnExecute()
    {
        //GameController.C_points.Remove(client.current_point.gameObject);
        GameController.SK_needs_restock.Remove(agent.GetComponent<ShopKeeper>().current_place_point.gameObject);
        ShopKeeper sk = agent.GetComponent<ShopKeeper>();

        sk.stock_item_objs.transform.GetChild(sk.item_being_placed).gameObject.SetActive(false);

        EndAction();
    }
}

public class PickStock : ActionTask
{
    protected override void OnExecute()
    {
        ShopKeeper sk = agent.GetComponent<ShopKeeper>();

        sk.item_being_placed = Random.Range(0, 10);

        sk.stock_item_objs.transform.GetChild(sk.item_being_placed).gameObject.SetActive(true);

        GameController.total_stock--;

        EndAction();
    }
}
public class ShopKeeper : MonoBehaviour
{

    public bool is_reclamed = true;

    public int queue = -2;
    public Vector3 target_cashier = Vector3.zero;
    public Transform current_place_point;
    public GameObject stock_item_objs;
    public Transform hand;
    public int item_being_placed = 0;

    void Start()
    {
        
    }

    void Update()
    {
        stock_item_objs.transform.position= hand.position;
    }
}
