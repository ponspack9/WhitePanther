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

    public static Vector3 PickAvailableForSK()
    {
        for (int i = 0; i < num_cashiers; i++)
        {
            if (cashiers[i, 0] == false)
            {
                cashiers[i, 0] = true;
                return Vector3.up;
            }
        }

        return Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
