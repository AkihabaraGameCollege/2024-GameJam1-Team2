using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScripts : MonoBehaviour
{
    GameObject player_;
    // Start is called before the first frame update
    void Start()
    {
        this.player_ = GameObject.Find("Player 1");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(3*Time.deltaTime, 0, 0);//1フレームごとに等速で直進

        //指定の位置に来たら壊す
        if (transform.position.x > 120f)
        {
            Destroy(gameObject);
        }




    }
}
