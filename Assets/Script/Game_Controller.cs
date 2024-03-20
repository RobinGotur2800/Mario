using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    public GameObject PlayerPos;
    public GameObject Bg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i < 10; i++)
        {
            if (PlayerPos.transform.position.x >= 5)
            {
                Instantiate(Bg, new Vector2(48f * i, 8.2f), Quaternion.identity);
            }
        }       
    }
}
