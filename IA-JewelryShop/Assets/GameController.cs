using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int day = 0;
    public int hour = 0;
    public int minute = 0;

    public float clients_rate = 0.5f;

    [Header("-------- Read Only --------")]
    public int number_shopkeepers = 1;
    public int number_guards = 1;
    public int number_clients = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
