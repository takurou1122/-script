using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcheryManager : MonoBehaviour
{
    private PlayerManager PlayerManager;
    public FloatingJoystick inputMove; //左画面JoyStick


    float moveSpeed = 5.0f; //移動する速度

    private bool HasEffect = false;

    public GameObject BreakEffect;
    private Rigidbody ArcheryRigidbody;

    private float Positionx;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        ArcheryRigidbody = GetComponent<Rigidbody>();
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Stage1" || sceneName == "Stage4")
        {
            Positionx = 6.8f;
        }
        else if (sceneName == "Stage2" || sceneName == "Stage6")
        {
            Positionx = 7.5f; //ステージ2以降のX座標の制限
        }
        else//(sceneName == "Stage3" || sceneName == "Stage5")
        {
            Positionx = 7.25f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameOver && !PlayerManager.isGameClear) //ゲームオーバーでない、または残り時間が20秒以下の場合
        {
            MoveArchery();
        }
        else if (PlayerManager.isGameOver || PlayerManager.isGameClear) //ゲームオーバーまたはクリアの場合
        {

            Rigidbody Headrb = GetComponent<Rigidbody>();
            Headrb.velocity = Vector3.zero; //ヘッドの速度をゼロにする
            if (!HasEffect && PlayerManager.isGameOver)
            {
                HasEffect = true;
                GameObject childObject = Instantiate(BreakEffect); // Prefabを生成
                childObject.transform.SetParent(transform);
                childObject.SetActive(true); // このオブジェクトの子に設定
                childObject.transform.localPosition = Vector3.zero; // 子オブジェクトの位置を調整 //ヘッドのブレイクエフェクトを表示
            }

        }
        float x = transform.position.x;
        if (x > Positionx)
        {
            x = -Positionx;
        }
        else if (x < -Positionx)
        {
            x = Positionx;
        }
        transform.position = new Vector3(x, transform.position.y, transform.position.z); //位置に合わせてX座標を調整

        
    }

    void MoveArchery()
    {
        ArcheryRigidbody.velocity = new Vector3(0, 0, 5.0f);
        //スティックでの横移動
        this.transform.position += this.transform.right * inputMove.Horizontal * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && PlayerManager.TorchEnemy)
        {
            PlayerManager.GameOver(); // ゲームオーバー処理を呼び出す
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && PlayerManager.TorchEnemy)
        {
            PlayerManager.GameOver(); // ゲームオーバー処理を呼び出す
        }
    }
}
