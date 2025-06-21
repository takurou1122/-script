using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingManager : MonoBehaviour
{
    private Rigidbody rb;
    private MakeEnemyManager MakeEnemyManager;

    public float ChasingSpeed = 7f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MakeEnemyManager = GameObject.Find("MakeEnemyManager").GetComponent<MakeEnemyManager>();
        this.gameObject.transform.rotation= Quaternion.Euler(90, 180, 0); // 初期回転を設定
    }

    // Update is called once per frame
    void Update()
    {
        GameObject ChasingTarget = MakeEnemyManager.spawnPoint.gameObject;
        if (PlayerManager.isGameOver || PlayerManager.isGameClear) //ゲームオーバーまたはクリアの場合
        {
            rb.velocity = Vector3.zero; // 追跡の速度をゼロにする
            this.gameObject.SetActive(false); // このオブジェクトを非アクティブにする
        }
        else
        {
            float direction = ChasingTarget.transform.position.x - transform.position.x; // 追跡対象への方向を計算
            rb.velocity = new Vector3(direction/2,0,-ChasingSpeed); // 追跡速度を設定
        }
    }
}
