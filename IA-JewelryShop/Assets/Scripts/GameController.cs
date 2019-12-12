using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool is_day = true;
    private int initial_clients = 15;

    [Header("Time --------------------------------------------------------")]
    private int day = 1;
    public int hour = 0;
    public int minute = 0;
    private float time_flow = 6.5f;
    private float time = 0.0f;


    //[SerializeField]
    private float fame = 20.0f;
    //[SerializeField]
    private float money = 4000.0f;
    [Header("Shop status --------------------------------------------------------")]
    public float fame_per_sale = 1.5f;
    public float fame_per_angry = 0.5f;
    public float money_per_sale = 150.0f;
    
    [Header("Read only")]
    public float chance_to_buy = 0.25f;
    public float chance_to_leave = 0.25f;
    public float chance_to_keep = 0.50f;

    [Header("Client --------------------------------------------------------")]
    public GameObject C_object;
    public List<GameObject> C;

    [Header("Shop Keeper --------------------------------------------------------")]
    public GameObject SK_object;
    public Button SK_button0;
    public Button SK_button1;
    public Button SK_button2;
    public Button SK_button3;
    public Button SK_add;
    public List<GameObject> SK;
    public float SK_cashier_time = 7.0f;
    public float SK_money = 4000.0f;


    [Header("Canvas --------------------------------------------------------")]
    public Text text_fame;
    public Text text_money;
    public Text text_chance_to_buy;
    public Text text_chance_to_leave;
    public Text text_chance_to_keep;

    public Text text_day;
    public Text text_hour;
    public Text text_minute;
    public Slider slider_time_flow;


    private void Start()
    {

        // Canvas ----------------------------
        // time
        slider_time_flow.value = time_flow;
        // END Canvas ----------------------------

        // Client ----------------------------
        C = new List<GameObject>();
        for (int i = 0; i < initial_clients; i++)
        {
            C.Add(Instantiate(C_object));
        }
        // END Client ----------------------------

        // Shop Keeper ----------------------------
        SK = new List<GameObject>();
        SK_add.onClick.AddListener(SKAdd);
        SK_button0.onClick.AddListener(SKChangeState0);
        SK_button1.onClick.AddListener(SKChangeState1);
        SK_button2.onClick.AddListener(SKChangeState2);
        SK_button3.onClick.AddListener(SKChangeState3);
        // END Shop Keeper ----------------------------
    }

    private void Update()
    {
        AdvanceTime();

        // Fame and chances ----------------------------
        UpdateChances();

        // Client ----------------------------
        ManageClients();
    }

    private void AdvanceTime()
    {
        time_flow = slider_time_flow.value;

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
        // Updating canvas
        text_day.text = (!is_day) ? "Night " + day.ToString() : "Day " + day.ToString();
        text_hour.text = (hour < 10) ? "0" + hour.ToString() : hour.ToString();
        text_minute.text = (minute < 10) ? "0" + minute.ToString() : minute.ToString();
    }

    private void UpdateChances()
    {
        // It is in per 10 ( not per cent %)
        chance_to_buy = Mathf.Log10(fame) + Mathf.Sqrt(fame) / 2;
        chance_to_leave = (10.0f - chance_to_buy) * 0.25f;
        chance_to_keep = (10.0f - chance_to_buy) * 0.75f;

        text_chance_to_buy.text = "Sale rate: " + (chance_to_buy * 10.0f).ToString("F2") + "%";
        text_chance_to_leave.text = "Leave rate: " + (chance_to_leave * 10.0f).ToString("F2") + "%";
        text_chance_to_keep.text = "Curiosity rate: " + (chance_to_keep * 10.0f).ToString("F2") + "%";
        text_fame.text = "Fame: " + Mathf.Round(fame * 100f) / 100f; ;
        text_money.text = "Money: " + Mathf.Round(money * 100f) / 100f;
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
            //client.sprite.sprite = Resources.Load<Sprite>("angryface");

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
            }
            else
            {
                client.queue_pos = Cashier.AdvanceQueue(client.queue, client.queue_pos);
            }
        }
        client.gameObject.name = "Client [" + client.queue + "," + client.queue_pos + "]";
        client.target_cashier = Cashier.GetVectorQueue(client.queue, client.queue_pos);
        if (client.queue_pos > 0) client.is_anyone_attending = Cashier.cashiers[client.queue, 0];
    }


    // Shop Keeper ----------------------------
    public void SKAdd()
    {
        if (money < SK_money) return;

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
        SK.Add(Instantiate(SK_object));
        money -= SK_money;

        if (SK.Count > 3) SK_add.gameObject.SetActive(false);
        else SK_money += 100 * SK.Count;

    }
    public void SKChangeState0()
    {
        bool reclaimed = SK[0].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[0].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        SK_button0.GetComponentInChildren<Text>().text = (!reclaimed) ? "Serving clients" : "Filling stock";
    }
    public void SKChangeState1()
    {
        bool reclaimed = SK[1].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[1].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        SK_button1.GetComponentInChildren<Text>().text = (!reclaimed) ? "Serving clients" : "Filling stock";
    }
    public void SKChangeState2()
    {
        bool reclaimed = SK[2].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[2].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        SK_button2.GetComponentInChildren<Text>().text = (!reclaimed) ? "Serving clients" : "Filling stock";
    }
    public void SKChangeState3()
    {
        bool reclaimed = SK[3].GetComponent<ShopKeeper>().is_reclamed;
        if (!reclaimed && !Cashier.IsThereFreeCashier()) return;

        SK[3].GetComponent<ShopKeeper>().is_reclamed = !reclaimed;

        SK_button3.GetComponentInChildren<Text>().text = (!reclaimed) ? "Serving clients" : "Filling stock";
    }

    // END Shop Keeper ----------------------------





    

    //public float clients_time = 0.0f;

    //public bool night = false;

    //[Header("Fame --------------------------------------------------------")]
    //private float time_between_client = 3.0f;
    //private float clients_rate = 0.25f;

    //[Header("Shop --------------------------------------------------------")]
    //public List<GameObject> shop_keepers;
    //public List<GameObject> guards;
    //public List<GameObject> costumers;

    //private float cosutmer_buying_prob = 20.0f;

    //private bool someone_cashier1 = false;
    //private bool someone_cashier2 = false;

    //public bool force_to_cashier = false;

    //[Header("Prefabs --------------------------------------------------------")]
    //public GameObject costumer_prefab;
    //public GameObject guard_prefab;
    //public GameObject shopkeeper_prefab;

    //[Header("Canvas --------------------------------------------------------")]
    //public Text day_text;
    //public Text hour_text;
    //public Text minute_text;
    //public Slider timer_rate_slider;

    //public Text shop_keepers_text;
    //public Text guards_text;
    //public Text costumers_text;

    //public Text probability_text;

    //

    //public Vector3 costumer_start_pos = new Vector3(14, 0, 44);
    //public Vector3 guard_start_pos = new Vector3(14, 0, 44);
    //public Vector3 shop_keeper_start_pos = new Vector3(8, 0, 10);

    //// Start is called before the first frame update
    //void Start()
    //{
    //    shop_keepers = new List<GameObject>();
    //    guards       = new List<GameObject>();
    //    costumers    = new List<GameObject>();

    //    AddGuard();
    //    AddShopKeeper();
    //    AddCostumer();

    //    //minute = System.DateTime.Now.Minute;
    //    //hour = System.DateTime.Now.Hour;
    //    minute = 0;
    //    hour = 8;

    //    reclaim_shop_keeper.onClick.AddListener(ReclaimShopKeeper);

    //    timer_rate_slider.value = time_rate;
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    clients_time += clients_rate * Time.deltaTime;

    //    if (!night && clients_time >= time_between_client)
    //    {
    //        clients_time = 0.0f;
    //        AddCostumer();
    //    }


    //    AdvanceTime();

    //    night = hour >= 21 || hour <= 7;
    //    someone_cashier1 = false;

    //    for (int i=0;i<costumers.Count;i++)
    //    {
    //        if (costumers[i].transform.position == costumers[i].GetComponent<CostumerBehaviour>().cashier1.position)
    //        {
    //            someone_cashier1 = true;
    //        }
    //        costumers[i].GetComponent<CostumerBehaviour>().sale_prob = cosutmer_buying_prob;
    //        costumers[i].GetComponent<CostumerBehaviour>().leave = night;

    //        if ((costumers[i].GetComponent<CostumerBehaviour>().leave &&
    //            costumers[i].GetComponent<CostumerBehaviour>().arrived) || 
    //            costumers[i].transform.position.z > 45)
    //        {
    //            Destroy(costumers[i]);
    //            costumers.RemoveAt(i);
    //        }
    //    }

    //    shop_keepers[0].GetComponent<ShopKeeperBehaviour>().go_cashier = force_to_cashier;
    //    //shop_keepers[1].GetComponent<ShopKeeperBehaviour>().go_cashier = !night;

    //    guards[0].GetComponent<GuardBehaviour>().quiet = night;

    //    time_rate = timer_rate_slider.value;

    //    probability_text.text = "Probability of a sale: " + cosutmer_buying_prob.ToString() + "%";
    //    costumers_text.text = "Costumers: " + costumers.Count.ToString();
    //    shop_keepers_text.text = "Shop keepers: " + shop_keepers.Count.ToString();
    //    guards_text.text = "Guards: " + guards.Count.ToString();
    //    reclaim_shop_keeper.GetComponentInChildren<Text>().text = (force_to_cashier) ? "Shop keeper at cashier" : "Shop keeper restocking";


    //    if(someone_cashier1 && force_to_cashier && shop_keepers[0].GetComponent<ShopKeeperBehaviour>().arrived)
    //    {
    //        cosutmer_buying_prob += 0.025f * Time.deltaTime;
    //    }

    //}
    //private void ReclaimShopKeeper()
    //{
    //    if (!night)
    //        force_to_cashier = !force_to_cashier;
    //    else
    //        force_to_cashier = false;
    //}

    //private void AddCostumer()
    //{
    //    costumers.Add(Instantiate(costumer_prefab, costumer_start_pos, Quaternion.identity));
    //}
    //private void AddShopKeeper()
    //{
    //    shop_keepers.Add(Instantiate(shopkeeper_prefab, shop_keeper_start_pos, Quaternion.identity));
    //}
    //private void AddGuard()
    //{
    //    guards.Add(Instantiate(guard_prefab, guard_start_pos, Quaternion.identity));
    //}
    //private void AdvanceTime()
    //{
    //    time += Time.deltaTime * time_rate;

    //    if (time >= 1)
    //    {
    //        time = 0.0f;
    //        minute++;
    //    }
    //    if (minute >= 60)
    //    {
    //        minute = 0;
    //        hour++;
    //        if (hour >= 24)
    //        {
    //            hour = 0;
    //            day++;
    //        }
    //    }
    //    // Updating canvas
    //    day_text.text    = (night) ? "Night " + day.ToString() : "Day " + day.ToString();
    //    hour_text.text   = (hour < 10) ? "0" + hour.ToString() : hour.ToString();
    //    minute_text.text = (minute < 10) ? "0" + minute.ToString() : minute.ToString();
    //}
}
