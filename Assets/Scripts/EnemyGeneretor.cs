using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneretor : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject EnemyPrefab;
    float span = 2f;
    float delta = 0;
    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if (this.delta > this.span) 
        {
            this.delta = 0;
            GameObject go = Instantiate(EnemyPrefab);
            int py = Random.Range(0, 10);//y座標に位置を指定
            go.transform.position = new Vector3(-100, py, 0);//Enemyを生成する場所を指定
        
        }
    }
}
