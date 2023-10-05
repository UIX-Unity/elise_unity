using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalControl : MonoBehaviour
{
    [SerializeField] BMSPlayer total;
    [SerializeField] GameScene gameScen;
    [SerializeField] UnityEngine.GameObject exp;
    public bool expCheck = false;
    public bool fadeCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0) && expCheck == false && total.stopCheck == false)
       {
            total.StopTotalSocre();
            expCheck = true;
       }
       if (Input.GetMouseButtonDown(0) && total.expbutton == true && fadeCheck == false)
       {
           total.expEndFade();
           fadeCheck = true;
       }

    }
}
