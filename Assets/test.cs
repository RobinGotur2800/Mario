using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public KeyCode iskey = KeyCode.LeftShift;
    private float timer = 0f;
    private float reachedtimer = 5f;
    private bool disable= false;    
    void Update()
    {
        if (disable)
        {
            if(Input.GetKey(iskey))
            {
                timer += Time.deltaTime;
                Debug.Log(timer);
                if(timer >= reachedtimer) 
                {
                    disable = false;
                    timer = 0f;
                }
            }else
            {
                if(Input.GetKey(iskey))
                {
                    disable = true;
                }
            }
        }
    }
}
