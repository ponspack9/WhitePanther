using NodeCanvas.Framework;
using UnityEngine;

public class ClientPickCashier : ActionTask
{

    protected override void OnExecute()
    {
        //Client client = agent.gameObject.GetComponent<Client>();
        //Vector3 queue_spot = Cashier.PickAvailableForClient(client.queue, out client.queue, out client.queue_pos);

        //if (client.queue == -2)
        //{
        //    client.is_buying = false;
        //}
        //else if ( client.queue >= 0)
        //{

        //    queue_spot.z += (client.queue_pos == 1) ? 4.0f : 3.0f * client.queue_pos;
        //    client.target_cashier = queue_spot;

        //}

        //if (client.j > 0)
        //{
        //    tmp.z += (client.j == 1) ? 4.0f : 3.0f * client.j;
        //    client.target_cashier = tmp;
        //    client.is_in_queue = true;
        //    if (client.j == 1)
        //    {
        //        client.is_actually_buying = true;
        //    }
        //}
        //else
        //{
        //    client.is_in_queue = false;
        //    client.is_actually_buying = false;
        //    client.is_buying = false;
        //    // no empty space to queue -> client angry and leaves
        //    client.is_leaving = true;
        //}

        EndAction();
    }
}

public class ClientLeaveCashier : ActionTask
{
    protected override void OnExecute()
    {
        Client client = agent.GetComponent<Client>();
        if (client.queue_pos == 1)
            client.sprite.sprite = Resources.Load<Sprite>("happyface");

        Cashier.LeaveCashierClient(client.queue, client.queue_pos);

        EndAction();
    }
}

public class SKPickCashier : ActionTask
{
    protected override void OnExecute()
    {
        int c = -1;
        Vector3 where = Cashier.PickAvailableForSK(out c);
        ShopKeeper sk = agent.GetComponent<ShopKeeper>();

        if (c >= 0)
        {
            sk.target_cashier = where;
            sk.queue = c;
        }
        else
        {
            sk.is_reclamed = false;
        }

        EndAction();
    }
}

public class SKLeaveCashier : ActionTask
{
    protected override void OnExecute()
    {
        Cashier.LeaveCashierSK(agent.GetComponent<ShopKeeper>().queue);

        EndAction();
    }
}

public class Cashier : MonoBehaviour
{
    //public Vector2 my_pos = new Vector2(-1, -1);

    public static bool[,] cashiers;
    public static int num_cashiers = 3; // FIXED
    public static int num_rows = 5;
    public static bool initialized = false;

    private static Vector3 cashier_1 = new Vector3(14, 0, 11);
    private static Vector3 cashier_2 = new Vector3(8, 0, 11);
    private static Vector3 cashier_3 = new Vector3(3, 0, 11);

    // Start is called before the first frame update
    void Start()
    {
        if (initialized) return;

        cashiers = new bool[num_cashiers, num_rows];

        for (int i = 0; i < num_cashiers;i++)
        {
            for (int j = 0; j < num_rows ; j++)
            {
                cashiers[i,j] = false;
            }
        }

        initialized = true;

    }

    public static void ResetCashiers()
    {
        for (int i = 0; i < num_cashiers; i++)
        {
            for (int j = 1; j< num_rows;j++)
                cashiers[i, j] = false;
        }
    }

    public static bool IsThereFreeCashier()
    {
        for (int i = 0; i < num_cashiers; i++)
            if (cashiers[i, 0] == false)
                return true;
        return false;
    }

    public static Vector3 PickAvailableForClient(out int queue, out int queue_row)
    {

        for (int i = 0; i < num_cashiers; i++)
        {
            for (int j = num_rows-1; j > 0; j--)
            {
                if (cashiers[i, j] == false)
                {
                    cashiers[i, j] = true;
                    queue = i;
                    queue_row = j;

                    switch (i)
                    {
                    case 0:
                        return cashier_1;
                    case 1:
                        return cashier_2;
                    case 2:
                        return cashier_3;
                    }
                }
            }
        }

        queue = -2;
        queue_row = -2;
        return Vector3.zero;
    }
    public static Vector3 GetVectorQueue(int queue, int queue_pos)
    {
        Vector3 ret = Vector3.zero;

        switch (queue)
        {
            case 0:
                ret = cashier_1;
                break;
            case 1:
                ret = cashier_2;
                break;
            case 2:
                ret = cashier_3;
                break;
            default:
                ret = Vector3.zero;
                break;
        }

        ret.z += (queue_pos == 1) ? 3.5f : 2.75f * queue_pos;

        return ret;
    }
    public static int AdvanceQueue(int queue, int queue_row)
    {
        //int row = (queue_row >= 2) ? queue_row - 1 : 1;
        //if (cashiers[queue, row] == false)
        //{
        //    return row;
        //}

        for (int j = queue_row; j > 0; j--)
        {
            if (cashiers[queue, j] == false)
            {
                cashiers[queue, j] = true;
                if (j != queue_row) cashiers[queue, queue_row] = false;
                return j;
            }
        }
        return queue_row;
    }
    public static void LeaveCashierClient(int cashier,int row)
    {
        cashiers[cashier, row] = false;
    }

    public static Vector3 PickAvailableForSK(out int cashier)
    {
        for (int i = 0; i < num_cashiers; i++)
        {
            if (cashiers[i, 0] == false)
            {
                cashiers[i, 0] = true;
                cashier = i;
                switch (i)
                {
                    case 0:
                        return cashier_1;
                    case 1:
                        return cashier_2;
                    case 2:
                        return cashier_3;
                    default:
                        return Vector3.zero;
                }
            }
        }
        cashier = -1;
        return Vector3.zero;
    }

    public static void LeaveCashierSK(int cashier)
    {
        if (cashier >= 0)
            cashiers[cashier, 0] = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
