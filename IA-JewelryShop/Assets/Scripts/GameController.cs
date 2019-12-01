using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    [Header("Time --------------------------------------------------------")]
    public int day = 0;
    public int hour = 0;
    public int minute = 0;
    private float time_rate = 6.5f;
    private float time = 0.0f;

    public float clients_time = 0.0f;

    public bool night = false;

    [Header("Fame --------------------------------------------------------")]
    private float time_between_client = 3.0f;
    private float clients_rate = 0.2f;

    [Header("Shop --------------------------------------------------------")]
    public List<GameObject> shop_keepers;
    public List<GameObject> guards;
    public List<GameObject> costumers;
    //public int number_shopkeepers = 1;
    //public int number_guards = 1;
    //public int number_costumers = 0;

    [Header("Prefabs --------------------------------------------------------")]
    public GameObject costumer_prefab;
    public GameObject guard_prefab;
    public GameObject shopkeeper_prefab;

    [Header("Canvas --------------------------------------------------------")]
    public Text day_text;
    public Text hour_text;
    public Text minute_text;
    public Slider timer_rate_slider;

    public Text shop_keepers_text;
    public Text guards_text;
    public Text costumers_text;

    public Button reclaim_shop_keeper;

    public Vector3 costumer_start_pos = new Vector3(14, 0, 44);
    public Vector3 guard_start_pos = new Vector3(14, 0, 44);
    public Vector3 shop_keeper_start_pos = new Vector3(8, 0, 10);

    // Start is called before the first frame update
    void Start()
    {
        shop_keepers = new List<GameObject>();
        guards       = new List<GameObject>();
        costumers    = new List<GameObject>();

        AddGuard();
        AddShopKeeper();
        AddCostumer();

        minute = System.DateTime.Now.Minute;
        hour = System.DateTime.Now.Hour;

        reclaim_shop_keeper.onClick.AddListener(ReclaimShopKeeper);

        timer_rate_slider.value = time_rate;
    }

    // Update is called once per frame
    void Update()
    {
        clients_time += clients_rate * Time.deltaTime;

        if (clients_time >= time_between_client)
        {
            clients_time = 0.0f;
            AddCostumer();
        }


        AdvanceTime();

        night = hour >= 21 || hour <= 7;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddCostumer();
        }

        for (int i=0;i<costumers.Count;i++)
        {
            if (costumers[i].GetComponent<CostumerBehaviour>().leave &&
                costumers[i].GetComponent<CostumerBehaviour>().arrived)
            {
                Destroy(costumers[i]);
                costumers.RemoveAt(i);
            }
        }
        time_rate = timer_rate_slider.value;

        costumers_text.text = "Costumers: " + costumers.Count.ToString();
        shop_keepers_text.text = "Shop keepers: " + shop_keepers.Count.ToString();
        guards_text.text = "Guards: " + guards.Count.ToString();

    }
    private void ReclaimShopKeeper()
    {
        shop_keepers[0].GetComponent<ShopKeeperBehaviour>().go_cashier = true;
    }

    private void AddCostumer()
    {
        costumers.Add(Instantiate(costumer_prefab, costumer_start_pos, Quaternion.identity));
    }
    private void AddShopKeeper()
    {
        shop_keepers.Add(Instantiate(shopkeeper_prefab, shop_keeper_start_pos, Quaternion.identity));
    }
    private void AddGuard()
    {
        guards.Add(Instantiate(guard_prefab, guard_start_pos, Quaternion.identity));
    }
    private void AdvanceTime()
    {
        time += Time.deltaTime * time_rate;

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
        day_text.text    = (day < 10) ? "Day " + day.ToString() : day.ToString();
        hour_text.text   = (hour < 10) ? "0" + hour.ToString() : hour.ToString();
        minute_text.text = (minute < 10) ? "0" + minute.ToString() : minute.ToString();
    }
}
