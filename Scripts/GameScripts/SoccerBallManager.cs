using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBallManager : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5.0f; // ボールの初期速度
    private PlayerManager PlayerManager;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        
        rb = GetComponent<Rigidbody>();
        
        rb.velocity = new Vector3(0, 0, -speed); // 初期速度を設定
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isGameOver || PlayerManager.isGameClear) //ゲームオーバーでない、または残り時間が20秒以下の場合
        {
            rb.velocity = Vector3.zero; // ボールの速度をゼロにする
            this.gameObject.SetActive(false); // ボールを非アクティブにする
        }
    }
}
