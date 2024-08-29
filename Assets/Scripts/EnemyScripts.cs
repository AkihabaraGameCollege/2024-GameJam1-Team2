using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(EnemyScripts), 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.1f, 0, 0);//1フレームごとに等速で直進

        //指定の位置に来たら壊す
        if(transform.position.x >60f)
        {
            Destroy(gameObject);
        }

    }


}
