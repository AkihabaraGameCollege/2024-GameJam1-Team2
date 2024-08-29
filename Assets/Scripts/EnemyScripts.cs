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
        this.player_ = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.04f, 0, 0);//1フレームごとに等速で直進

        //指定の位置に来たら壊す
        if (transform.position.x > 60f)
        {
            Destroy(gameObject);
        }



        //当たり判定
        Vector2 p1 = transform.position;
        Vector2 p2 = this.player_.transform.position;
        Vector2 dir = p1 - p2;
        float d = dir.magnitude;
        float r1 = 0.5f;
        float r2 = 1.0f;

        if(d < r1 + r2)
        {
            Destroy (gameObject);
        }

    }
}
