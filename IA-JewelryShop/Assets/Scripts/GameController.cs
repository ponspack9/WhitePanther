using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeCanvas.Framework;
using System;


public class GameController : MonoBehaviour
{
    public bool is_day = true;

    public float chance_to_buy = 0.25f;
    public float chance_to_leave = 0.25f;
    public float chance_to_keep = 0.50f;
    
    [Header("Shop Keeper --------------------------------------------------------")]
    public GameObject SK_object;
    public Dropdown SK_dropdown0;
    public Dropdown SK_dropdown1;
    public Dropdown SK_dropdown2;
    public Dropdown SK_dropdown3;
    public Button SK_add;
    public List<GameObject> SK;
    private int SK_count = 0;

    private void Start()
    {
        SK = new List<GameObject>();
        SK_add.onClick.AddListener(SKAdd);
    }

    private void Update()
    {
    }

    public void SKAdd()
    {
        if (SK_count <= 3)
        {
            SK.Add(Instantiate(SK_object));
            switch (SK_count)
            {
                case 0:
                    SK_dropdown0.enabled = true;
                    break;
                case 1:
                    SK_dropdown1.enabled = true;
                    break;
                case 2:
                    SK_dropdown2.enabled = true;
                    break;
                case 3:
                    SK_dropdown3.enabled = true;
                    break;
                default:
                    break;
            }
            SK_count++;
        }

    }
    public void SKChangeState0()
    {
        SK[0].GetComponent<ShopKeeper>().is_reclamed = SK_dropdown0.value == 0;
    }
    public void SKChangeState1()
    {
        SK[1].GetComponent<ShopKeeper>().is_reclamed = SK_dropdown1.value == 0;
    }
    public void SKChangeState2()
    {
        SK[2].GetComponent<ShopKeeper>().is_reclamed = SK_dropdown2.value == 0;
    }
    public void SKChangeState3()
    {
        SK[3].GetComponent<ShopKeeper>().is_reclamed = SK_dropdown3.value == 0;
    }










    //[Header("Time --------------------------------------------------------")]
    //private int day = 1;
    //public int hour = 0; 
    //public int minute = 0;
    //private float time_rate = 6.5f;
    //private float time = 0.0f;

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
