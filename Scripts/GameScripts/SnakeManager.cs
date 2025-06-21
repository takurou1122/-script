using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    private PlayerManager PlayerManager;
    public FloatingJoystick inputMove; //左画面JoyStick


    float moveSpeed = 5.0f; //移動する速度

    public List<Transform> bodyParts = new List<Transform>();
    public float followSpeed = 10f;
    public float distanceBetweenParts = 0.5f;

    private bool HasEffect = false;

    public GameObject BreakEffect;

    void Start()
    {
        PlayerManager  = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (!PlayerManager.isGameOver && !PlayerManager.isGameClear) //ゲームオーバーでない、または残り時間が20秒以下の場合
        {
            MoveSnake();
        }
        else if (PlayerManager.isGameOver || PlayerManager.isGameClear) //ゲームオーバーまたはクリアの場合
        {

            Rigidbody Headrb = bodyParts[0].GetComponent<Rigidbody>();
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
        float Parentx = transform.parent.position.x;
        if (Parentx > 7.8f)
        {
            Parentx = -7.8f;
        }
        else if (Parentx < -7.8f)
        {
            Parentx = 7.8f;
        }
        transform.parent.position = new Vector3(Parentx, transform.position.y, transform.position.z); //親の位置に合わせてX座標を調整
        

    }

    void MoveSnake()
    {
        Rigidbody Headrb = bodyParts[0].GetComponent<Rigidbody>();
        Headrb.velocity = new Vector3(0, 0, 5.0f);
        //左スティックでの横移動
        bodyParts[0].transform.position += bodyParts[0].transform.right * inputMove.Horizontal * moveSpeed * Time.deltaTime;



        for (int i = 1; i < bodyParts.Count; i++)
        {
            Vector3 targetPosition = bodyParts[i - 1].position - bodyParts[i - 1].forward * distanceBetweenParts;
            bodyParts[i].position = Vector3.Lerp(bodyParts[i].position, targetPosition, Time.deltaTime * followSpeed);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && PlayerManager.TorchEnemy)
        {
            //敵に触れた時の処理
            PlayerManager.GameOver();//ゲームオーバーのフラグを立てる
            VibrationMng.ShortVibration(); //振動を発生させる
        }
        
        
    }
    
}
