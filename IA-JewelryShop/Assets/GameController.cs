using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Time --------------------------------------------------------")]
    public int day = 0;
    public int hour = 0;
    public int minute = 0;
    private float time = 0.0f;

    public float time_rate = 0.5f;

    [Header("Fame --------------------------------------------------------")]
    public float fame = 1.0f;
    public float clients_rate = 0.5f;

    [Header("Shop --------------------------------------------------------")]
    public int number_shopkeepers = 1;
    public int number_guards = 1;
    public int number_clients = 0;

    [Header("Canvas --------------------------------------------------------")]
    public Canvas canvas;
    public Text day_text;
    public Text hour_text;
    public Text minute_text;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
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
            if (hour >= 60)
            {
                hour = 0;
                day++;
            }
        }
        UpdateCanvasTime();
    }

    void UpdateCanvasTime()
    {
        day_text.text = (day < 10 ) ? "0" + day.ToString() : day.ToString();
        hour_text.text = (hour < 10) ? "0" + hour.ToString() : hour.ToString();
        minute_text.text = (minute < 10) ? "0" + minute.ToString() : minute.ToString() ;
    }
}
