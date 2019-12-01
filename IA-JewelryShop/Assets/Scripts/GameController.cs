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

    public bool night = false;

    [Header("Fame --------------------------------------------------------")]
    public float fame = 1.0f;
    public float clients_rate = 0.25f;

    [Header("Shop --------------------------------------------------------")]
    public int number_shopkeepers = 1;
    public int number_guards = 1;
    public int number_costumers = 0;

    [Header("Prefabs --------------------------------------------------------")]
    public GameObject costumer_prefab;
    public GameObject guard_prefab;
    public GameObject shopkeeper_prefab;

    [Header("Canvas --------------------------------------------------------")]
    public Text day_text;
    public Text hour_text;
    public Text minute_text;
    public Slider timer_rate_slider;

    private Vector3 costumer_start_pos = new Vector3(14, 0, 44);

    // Start is called before the first frame update
    void Start()
    {
        minute = System.DateTime.Now.Minute;
        hour = System.DateTime.Now.Hour;

        timer_rate_slider.value = time_rate;
    }

    // Update is called once per frame
    void Update()
    {
        AdvanceTime();

        night = hour >= 21 || hour <= 7;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddCostumer();
        }

        time_rate = timer_rate_slider.value;
    }
    private void AddCostumer()
    {
        Instantiate(costumer_prefab, costumer_start_pos, Quaternion.identity);
        number_costumers++;
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
