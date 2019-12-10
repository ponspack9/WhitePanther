using NodeCanvas.Framework;
using UnityEngine;

public class ClientPickCashier : ActionTask
{

    protected override void OnExecute()
    {
        Client client = agent.gameObject.GetComponent<Client>();
        Vector3 tmp = Cashier.PickAvailableForClient(client.i, out client.i, out client.j);

        if (client.j > 0)
        {
            tmp.z += (client.j == 1) ? 4.0f : 3.0f * client.j;
            client.target_cashier = tmp;
            client.is_in_queue = true;
            if (client.j == 1)
            {
                client.is_actually_buying = true;
            }
        }
        else
        {
            client.is_in_queue = false;
            client.is_actually_buying = false;
            client.is_buying = false;
            // no empty space to queue -> client angry and leaves
            client.is_leaving = true;
        }

        EndAction();
    }
}

public class ClientLeaveCashier : ActionTask
{
    Cashier cashier;

    protected override void OnExecute()
    {
        
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
            sk.cashier = c;
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
        Cashier.LeaveCashierSK(agent.GetComponent<ShopKeeper>().cashier);

        EndAction();
    }
}

public class Cashier : MonoBehaviour
{
    //public Vector2 my_pos = new Vector2(-1, -1);

    public static bool[,] cashiers;
    public static int num_cashiers = 3;
    public static int num_rows = 6;
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

    public static bool IsThereFreeCashier()
    {
        for (int i = 0; i < num_cashiers; i++)
            if (cashiers[i, 0] == false)
                return true;
        return false;
    }

    public static Vector3 PickAvailableForClient(int queue, out int index, out int jndex)
    {

        for (int i = queue; i < num_cashiers; i++)
        {
            for (int j = 1; j < num_rows; j++)
            {
                if (cashiers[i, j] == false)
                {
                    cashiers[i, j] = true;
                    index = i;
                    jndex = j;

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

        index = 0;
        jndex = 0;
        return Vector3.zero;
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
