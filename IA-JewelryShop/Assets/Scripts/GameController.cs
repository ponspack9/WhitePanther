using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{

    public bool is_day = true;
    public bool game_over = false;
    public Text game_over_stats;
    public Text game_over_status;
    public Button restart;
    public Button quit;
    public GameObject game_over_panel;

    [Header("Time --------------------------------------------------------")]
    private int day = 1;
    private int hour = 8;
    private int minute = 0;
    private float time_flow = 7f;
    private float time = 0.0f;
    public float time_between_client = 7.0f;
    private float time_clients = 0.0f;


    [Header("Shop status --------------------------------------------------------")]
    private float fame = 60.0f;
    private float money = 50000.0f;
    public float fame_per_sale = 2.0f;
    public float fame_per_angry = 0.5f;
    public float money_per_sale = 150.0f;
    
    [Header("Read only")]
    public float chance_to_buy = 0.25f;
    public float chance_to_leave = 0.25f;
    public float chance_to_keep = 0.50f;

    [Header("Client --------------------------------------------------------")]
    public GameObject C_object;
    public Transform C_spawn1;
    public Transform C_spawn2;
    public Mesh[] C_meshes = new Mesh[10];
    public Material[] C_materials = new Material[30];
    public GameObject C_points_parent;
    public List<GameObject> C;
    public List<GameObject> C_points;
    public List<GameObject> C_points_all;

    [Header("Shop Keeper --------------------------------------------------------")]
    public GameObject SK_object;
    public Material[] SK_materials = new Material[3];
    public GameObject SK_points_parent;
    public GameObject SK_restock_icon;
    public Button SK_button0;
    public Button SK_button1;
    public Button SK_button2;
    public Button SK_button3;
    public Button SK_add;
    public List<GameObject> SK;
    public List<GameObject> SK_points;
    public List<GameObject> SK_restock;
    static public List<GameObject> SK_needs_restock;
    static public List<GameObject> SK_needs_restock_icon;
    public float SK_cashier_time = 6.0f;
    public float SK_cost = 1000.0f;
    public float SK_money = 1000.0f;


    [Header("Shop Extension --------------------------------------------------------")]
    public GameObject extra_showers_parent;
    public GameObject extra_points_parent;
    public Material material_shop_interiors;
    public Material green;
    private float fade_timer = 1.0f;


    [Header("Stats --------------------------------------------------------")]
    public int sales_total = 0;
    public int sales_day = 0;
    public int sales_best = 0;
    public int sales_desired = 0;
    public float fame_best = 0;
    public float money_best = 0;
    public float money_invested = 0;
    public float money_salaries = 0;

    [Header("Canvas --------------------------------------------------------")]
    public Text text_fame;
    public Text text_money;
    public Text text_chance_to_buy;
    public Text text_chance_to_leave;
    public Text text_chance_to_keep;

    public Text text_day;
    public Text text_hour;
    public Text text_minute;

    public Button button_upgrade;
    public Button button_stock;
    private bool upgrading = false;
    public List<int> upgraded;
    public List<float> upgrade_cost;

    public GameObject panel_restock;
    public Text text_stats;
    public Text text_to_buy;

    private string currentToolTipText = "";
    private bool SK_hover = false;

    [Header("Stock --------------------------------------------------------")]
    public GameObject stock_van;
    private bool is_can_buy_stock = false;
    private bool pay_workers = false;
    public bool is_stock_left = true;
    public int stock_price = 150;
    static public int total_stock = 25;
    public InputField stock_input;
    public Button stock_buy;
    private bool Upgrade_hover = false;
    private bool StockIcon_hover = false;
    private bool SKEmployee_hover = false;
    private bool Money_hover = false;
    private bool Fame_hover = false;
    private bool SalesRate_hover = false;


    private void Start()
    {
        // Canvas ----------------------------
        restart.onClick.AddListener(Restart);
        quit.onClick.AddListener(Quit);

        // time
        stock_buy.onClick.AddListener(BuyStock);
        stock_input.contentType = InputField.ContentType.IntegerNumber;
        // END Canvas ----------------------------

        // Upgrades ----------------------------
        button_upgrade.onClick.AddListener(ToggleUpgrade);
        upgraded = new List<int>();
        upgrade_cost = new List<float>();
        upgrade_cost.Add(1500.0f);
        upgrade_cost.Add(1000.0f);
        upgrade_cost.Add(750.0f);
        upgrade_cost.Add(1000.0f);
        upgrade_cost.Add(750.0f);
        upgrade_cost.Add(1500.0f);
        upgrade_cost.Add(1250.0f);
        upgrade_cost.Add(1250.0f);
        upgrade_cost.Add(1000.0f);
        upgrade_cost.Add(1250.0f);
        upgrade_cost.Add(1500.0f);
        upgrade_cost.Add(1000.0f);

        button_stock.onClick.AddListener(ToggleStock);

        // END Upgrades ----------------------------

        // Client ----------------------------
        C = new List<GameObject>();
        C_points = new List<GameObject>();
        C_points_all = new List<GameObject>();
        for (int i = 0; i < C_points_parent.transform.childCount; i++)
        {
            C_points_all.Add(C_points_parent.transform.GetChild(i).gameObject);
        }

        for (int i=0;i< 10; i++)
        {
            SpawnClient();
        }
        // END Client ----------------------------

        // Shop Keeper ----------------------------
        SK = new List<GameObject>();
        SK_points = new List<GameObject>();
        SK_needs_restock = new List<GameObject>();
        SK_needs_restock_icon = new List<GameObject>();
        SK_restock = new List<GameObject>();
        SK_add.onClick.AddListener(SKAdd);
        //SK_add.GetComponentInChildren<Text>().text = "Hire shop keeper : " + SK_cost;
        SK_button0.onClick.AddListener(SKChangeState0);
        SK_button1.onClick.AddListener(SKChangeState1);
        SK_button2.onClick.AddListener(SKChangeState2);
        SK_button3.onClick.AddListener(SKChangeState3);

        for (int i = 0; i < SK_points_parent.transform.childCount; i++)
        {
            SK_points.Add(SK_points_parent.transform.GetChild(i).gameObject);
        }
        Cashier.ResetCashiers();

        //SK_add.OnPointerEnter(new PointerEventData(new EventSystem()));
        // END Shop Keeper ----------------------------
    }

    private void Update()
    {
        currentToolTipText = "";

        if (game_over) return;

        SpawnClients();

        Restock();

        ShowAndApplyUpgrades();

        AdvanceTime();

        UpdateChances();

        ManageClients();

        UpdateStats();

        PayWorkers();

        if (Input.GetKeyDown(KeyCode.C))
            SpawnClient();
    }

    private void BuyStock()
    {
        int stock_to_buy = int.Parse(stock_input.text);
        int total_price = stock_price * stock_to_buy;

        if (!is_can_buy_stock || total_price > money) return;

        money -= total_price;
        total_stock += stock_to_buy;
        money_invested += total_price;

        stock_buy.gameObject.SetActive(false);
    }

    private void SpawnClients()
    {
        time_clients += Time.deltaTime;

        if (is_day && !game_over && time_clients >= time_between_client)
        {
            time_clients = 0;
            SpawnClient();
        }
    }

    private void SpawnClient()
    {
        int i = Random.Range(0, 30);
        Transform t = (i % 2 == 0) ? C_spawn1 : C_spawn2;

        GameObject client = Instantiate(C_object, t.position, t.rotation);

        client.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh = C_meshes[i / 3];

        client.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = C_materials[i];
        client.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = C_materials[i];

        C.Add(client);
    }

    private void UpdateStats()
    {
        if (fame_best < fame) fame_best = fame;
        if (money_best < money) money_best = money;
        if (sales_best < sales_day) sales_best = sales_day;

        text_stats.text = "Total sales: " + sales_total + "\n\n" +
            "Total money invested: " + money_invested + "\n\n" +
            "Total money in salaries: " + money_salaries + "\n\n\n"+
            "Most day sales: " + sales_best + "\n\n" +
            "Most fame: " + fame_best + "\n\n" +
            "Most money: " + money_best + "\n\n";


        text_to_buy.text = "Today desired sales: " + sales_desired + "\n\n" +
            "Today sales: " + sales_day + "\n\n" +
            "Unsuccessful sales: " + (sales_desired - sales_day) + "\n\n\n" +
            "Stock price: " + stock_price + "\n\n" + 
            "Remaining stock: " + total_stock + "\n\n";
    }

    private void OnGUI()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        if (SK_hover)
        {
            GUI.Box(new Rect(x, Screen.height - y, 175, 50), "Hire shop keeper : " + SK_cost + "\nCost per day: " + SK_money);
        }
        else if (Upgrade_hover)
        {
            GUI.Box(new Rect(x, Screen.height - y, 175, 25), "Show available upgrades");
        }
        else if (StockIcon_hover)
        {
            GUI.Box(new Rect(x, Screen.height - y, 225, 25), "See shop statistics and buy stock");
        }
        else if (SKEmployee_hover)
        {
            GUI.Box(new Rect(x, Screen.height - y, 150, 25), "Click to change state");
        }
        else if (Money_hover)
        {
            GUI.Box(new Rect(x, Screen.height - y, 50, 25), "Money");
        }
        else if (Fame_hover)
        {
            GUI.Box(new Rect(x, Screen.height - y, 200, 25), "Fame / Reputation out of 100");
        }
        else if (SalesRate_hover)
        {
            GUI.Box(new Rect(x, Screen.height - y, 225, 50), "Percentatge of a sale while a client \n is looking around the shop ");
        }
        else if (currentToolTipText != "")
        {

            GUI.Box(new Rect(x, Screen.height - y, 100, 25), currentToolTipText);
        }
    }

    private void PayWorkers()
    {
        if (pay_workers && is_day)
        {
            float m = SK_money * SK.Count;
            money -= m;
            money_invested += m;
            money_salaries += m;
            pay_workers = false;

            sales_day = 0;
            sales_desired = 0;

            Cashier.ResetCashiers();

            Instantiate(stock_van);
        }

    }
    private void ToggleStock()
    {
        panel_restock.SetActive(!panel_restock.activeSelf);
        
    }
    
    private void Restock()
    {
        is_stock_left = total_stock > 0;

        // Destroying previous icons
        for (int i = 0; i < SK_needs_restock_icon.Count; i++)
        {
            Destroy(SK_needs_restock_icon[i]);
        }
        SK_needs_restock_icon.Clear();

        // Creating current icons
        for (int i = 0; i < SK_needs_restock.Count; i++)
        {
            Transform t = SK_needs_restock[i].transform;


            GameObject obj = Instantiate(SK_restock_icon,
                new Vector3(t.position.x, t.position.y + 5.0f, t.position.z),
                Quaternion.LookRotation(Camera.main.transform.position - t.position, Vector3.up));

            //obj.transform.LookAt(Camera.main.transform);
            SK_needs_restock_icon.Add(obj);
        }

        C_points.Clear();
        SK_restock.Clear();
        for (int i = 0; i < C_points_all.Count; i++)
        {
            bool to_restock = false;
            for (int j=0;j< SK_needs_restock.Count && !to_restock; j++)
            {
                if (SK_needs_restock[j].transform.position == C_points_all[i].transform.position)
                {
                    to_restock = true;
                }
            }

            if (to_restock)
            {
                SK_restock.Add(C_points_all[i]);
            }
            else
            {
                C_points.Add(C_points_all[i]);
            }
        }
    }

    private void ToggleUpgrade()
    {
        upgrading = !upgrading;
        //SK_add.gameObject.SetActive(upgrading && SK.Count < 4);
        ResetUpgradingColor();
    }

    private void ResetUpgradingColor()
    {
        for (int j = 0; j < extra_showers_parent.transform.childCount; j++)
        {
            if (upgraded.Contains(j)) continue;

            Transform parent = extra_showers_parent.transform.GetChild(j);

            parent.gameObject.SetActive(upgrading);

            for (int i = 0; i < parent.childCount; i++)
            {
                parent.GetChild(i).GetComponent<MeshRenderer>().material = green;
            }
        }
    }

    private void ShowAndApplyUpgrades()
    {
        if (!upgrading) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        ResetUpgradingColor();
        
     
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("extra")))
        {

            Transform parent = hit.transform.parent;

            int s = parent.name.IndexOf('(')+1;
            int e = parent.name.Length - 1;
            int index = int.Parse(parent.name.Substring(s, e-s));

            if (!upgraded.Contains(index))
            {
                currentToolTipText = "Cost: " + upgrade_cost[index];
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform child = parent.GetChild(i);
                    // Fading to show hovered
                    Color tmp = child.GetComponent<MeshRenderer>().material.color;
                    tmp.g += Mathf.Sin(fade_timer += Time.deltaTime * 5.0f);
                    child.GetComponent<MeshRenderer>().material.color = tmp;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (money >= upgrade_cost[index])
                    {
                        parent.gameObject.SetActive(true);
                        for (int i = 0; i < parent.childCount; i++)
                        {
                            Transform child = parent.GetChild(i);
                            child.GetComponent<MeshRenderer>().material = material_shop_interiors;
                            child.GetComponent<MeshRenderer>().material.color = Color.white;
                            child.GetComponent<NavMeshObstacle>().enabled = true;
                        }

                        //Adding the points of the new shower
                        parent = extra_points_parent.transform.GetChild(index);
                        for (int i = 0; i < parent.childCount; i++)
                        {
                            C_points_all.Add(parent.GetChild(i).gameObject);
                            //SK_points.Add(parent.GetChild(i).gameObject);
                        }
                        
                        money_invested += upgrade_cost[index];
                        money -= upgrade_cost[index];
                        upgraded.Add(index);
                    }
                }
            }
        }
    }

    private void AdvanceTime()
    {

        time += Time.deltaTime * time_flow;

        if (time >= 1)
        {
            time = 0.0f;
            minute++;
        }
        if (minute >= 60)
        {
            minute = 0;
            hour++;
            if (hour >= 24)
            {
                hour = 0;
                day++;
            }
        }

        is_day = hour <= 20 && hour >= 8;
        is_can_buy_stock = hour >= 20 && hour <= 23;
        stock_buy.gameObject.SetActive(is_can_buy_stock);
        if (hour == 0) pay_workers = true;

        // Updating canvas
        text_day.text = (!is_day) ? "Night " + day.ToString() : "Day " + day.ToString();
        text_hour.text = (hour < 10) ? "0" + hour.ToString() : hour.ToString();
        text_minute.text = (minute < 10) ? "0" + minute.ToString() : minute.ToString();

    }

    private void UpdateChances()
    {
        // It is in per 10 ( not per cent %)
        chance_to_buy = Mathf.Log10(fame) + Mathf.Sqrt(fame) / 4;
        chance_to_leave = (10.0f - chance_to_buy) * 0.2f;
        chance_to_keep = (10.0f - chance_to_buy) * 0.8f;

        text_chance_to_buy.text = (chance_to_buy * 10.0f).ToString("F2") + "%";
        //text_chance_to_leave.text = "Leave rate: " + (chance_to_leave * 10.0f).ToString("F2") + "%";
        //text_chance_to_keep.text = "Curiosity rate: " + (chance_to_keep * 10.0f).ToString("F2") + "%";
        text_fame.text = (fame).ToString("F2");
        text_money.text = ((int)(money)).ToString();

        time_between_client = 8.0f - Mathf.Log10(fame)*2;

        game_over = fame >= 90 || fame <= 5;

        if (game_over)
        {
            game_over_panel.SetActive(true);
            game_over_status.text = (fame >= 90) ? "CONGRATULATIONS, YOU WON!" : "GAME OVER";
            game_over_stats.text = text_stats.text;

        }
    }

    private void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void Quit()
    {
        Application.Quit();
    }
    private void ManageClients()
    {
        for (int i = 0; i < C.Count; i++)
        {
            if (C[i].transform.position.z >= 45)
            {
                Destroy(C[i]);
                C.Remove(C[i]);
            }
            else
            {
                Client client = C[i].GetComponent<Client>();
                if (client.is_buying)
                    ManageClient(client);
            }

        }
    }

    private void ManageClient(Client client)
    {
        // Client can't find a place in the queue, leaves angry
        if (client.queue == -2)
        {
            client.is_buying = false;
            client.is_leaving = true;

            return;
        }
        // First iteration of buying, pick a queue to stay
        else if (client.queue == -1)
        {
            Cashier.PickAvailableForClient(out client.queue, out client.queue_pos);
        }
        // Client already has a queue and need to go closer
        else
        {
            if (client.queue_pos == 1)
            {
                client.is_leaving = true;
                client.GetComponent<Animator>().SetInteger("idle_animation", 3);

            }
            else
            {
                client.queue_pos = Cashier.AdvanceQueue(client.queue, client.queue_pos);
                client.GetComponent<Animator>().SetInteger("idle_animation", Random.Range(0, 5));
            }
        }
        client.gameObject.name = "Client [" + client.queue + "," + client.queue_pos + "]";
        client.target_cashier = Cashier.GetVectorQueue(client.queue, client.queue_pos);
        if (client.queue_pos > 0) client.is_anyone_attending = Cashier.cashiers[client.queue, 0];
    }


    // Shop Keeper ----------------------------
    public void SKAdd()
    {
        if (money < SK_cost) return;

        switch (SK.Count)
        {
            case 0:
                SK_button0.gameObject.SetActive(true);
                break;
            case 1:
                SK_button1.gameObject.SetActive(true);
                break;
            case 2:
                SK_button2.gameObject.SetActive(true);
                break;
            case 3:
                SK_button3.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        GameObject sk = Instantiate(SK_object);

        sk.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = SK_materials[SK.Count%3];
        sk.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = SK_materials[SK.Count%3];
        SK.Add(sk);

        money -= SK_cost;
        money_invested += SK_cost;

        if (SK.Count > 3)
        {
            SK_add.gameObject.SetActive(false);
            SK_hover = false;
        }
        else
        {
            SK_cost += 1000 * SK.Count;
        }

    }
    public void SKChangeState0()
    {
        bool reclaimed = SK[0].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[0].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        //SK_button0.GetComponentInChildren<Text>().text = (!reclaimed) ? "Serving clients" : "Filling stock";
        SK_button0.image.sprite = (!reclaimed) ? Resources.Load<Sprite>("sk") : Resources.Load<Sprite>("skstock");
    }
    public void SKChangeState1()
    {
        bool reclaimed = SK[1].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[1].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        SK_button1.image.sprite = (!reclaimed) ? Resources.Load<Sprite>("sk") : Resources.Load<Sprite>("skstock");
    }
    public void SKChangeState2()
    {
        bool reclaimed = SK[2].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[2].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        SK_button2.image.sprite = (!reclaimed) ? Resources.Load<Sprite>("sk") : Resources.Load<Sprite>("skstock");
    }
    public void SKChangeState3()
    {
        bool reclaimed = SK[3].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[3].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        SK_button3.image.sprite = (!reclaimed) ? Resources.Load<Sprite>("sk") : Resources.Load<Sprite>("skstock");

    }

    // END Shop Keeper ----------------------------

    public void OnPointerEnterSK_add()
    {
        SK_hover = true;
    }
    public void OnPointerExitSK_add()
    {
        SK_hover = false;
    }

    public void OnPointerEnterUpgrade()
    {
        Upgrade_hover = true;
    }
    public void OnPointerExitUpgrade()
    {
        Upgrade_hover = false;
    }

    public void OnPointerEnterStockIcon()
    {
        StockIcon_hover = true;
    }
    public void OnPointerExitStockIcon()
    {
        StockIcon_hover = false;
    }
    public void OnPointerEnterSKEmployee()
    {
        SKEmployee_hover = true;
    }
    public void OnPointerExitSKEmployee()
    {
        SKEmployee_hover = false;
    }

    public void OnPointerEnterMoney()
    {
        Money_hover = true;
    }
    public void OnPointerExitMoney()
    {
        Money_hover = false;
    }

    public void OnPointerEnterSalesRate()
    {
        SalesRate_hover = true;
    }
    public void OnPointerExitSalesRate()
    {
        SalesRate_hover = false;
    }

    public void OnPointerEnterFame()
    {
        Fame_hover = true;
    }
    public void OnPointerExitFame()
    {
        Fame_hover = false;
    }

   
}
