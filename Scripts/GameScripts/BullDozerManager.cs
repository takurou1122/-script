using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullDozerManager : MonoBehaviour
{

    private float targetAngle = 90f;  // 目標角度
    private float startAngle = -60f; // 初期角度
    private float speed = 2f;        // 回転速度
    private float t = 0f;            // 補間係数
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(startAngle, 0, -90f);
        StartCoroutine(RotateBullDozer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator RotateBullDozer()
    {
        yield return new WaitForSeconds(2.5f); // 1秒待機

        while (t < 1f|| !PlayerManager.isGameClear || !PlayerManager.isGameOver) // ゲームオーバーまたはクリアでない場合
        {
            t += Time.deltaTime * speed;
            float angle = Mathf.Lerp(startAngle, targetAngle, t);
            transform.rotation = Quaternion.Euler(angle, 0, -90f);
            yield return null; // 1フレーム待機
        }
    }
}
