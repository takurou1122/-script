using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    // proTextをprivateのprefabに設定したい
    public TMP_Text proText;
    public static int CoinNum;

    public static int GetCoinNum;

    public List<GameObject> coins = new List<GameObject>(); // コインのリスト
    public Transform coinContainer; // Canvas 内の親オブジェクト (Panelなど)

    public float moveDuration = 0.1f; // 移動時間

    public Vector2 endPos = new Vector2(-211, 740); // 目的地 (Canvas座標)

    public static int RemainTime = 300;
    public TMP_Text RemainTimeText; // 残り時間のテキスト

    public GameObject Get3CoinsImage; // 3コインの画像


    // Start is called before the first frame update
    void Start()
    {
        CoinNum = PlayerPrefs.GetInt("CoinNum", 0); // PlayerPrefsからコインの数を取得
        GetCoinNum = PlayerPrefs.GetInt("GetCoin", 0); // PlayerPrefsからコインの数を取得
        // Find the TMP_Text component in the scene
        proText = GameObject.Find("CoinNumText").GetComponent<TMP_Text>();
        if (CoinNum < 0)
        {
            CoinNum = 0;
            PlayerPrefs.SetInt("CoinNum", CoinNum); // コインの数をリセット
        }

        // Check if proText is found


            // Initialize the text
            proText.text = "×" + CoinNum.ToString();
        StartCoroutine(ShowCoinsThenMove(GetCoinNum));

        RemainTime -= PlayerPrefs.GetInt("Score", 0);

        
        

    }

    // Update is called once per frame
    void Update()
    {
        RemainTimeText.text = RemainTime.ToString() + "Sec";
    }
    private IEnumerator ShowCoinsThenMove(int coin)
    {
        if (coin >= 7) coin = 7; // **最大5枚まで表示**
                                 // **順番にコインを表示**
        yield return new WaitForSeconds(0.5f); // 少し待機してから表示開始
        for (int i = 0; i < coin; i++)
        {
            coins[i].SetActive(true); // コインを表示
            yield return new WaitForSeconds(0.02f);


        }

        yield return new WaitForSeconds(0.2f); // 少し間をあけて移動開始

        // **すべてのコインを目的地へ移動**
        float elapsedTime = 0;
        Vector2[] startPositions = new Vector2[coin]; // 初期座標を保存

        for (int i = 0; i < coin; i++)
        {
            startPositions[i] = coins[i].GetComponent<RectTransform>().anchoredPosition; // UI座標取得
        }


        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            t = EaseOutQuad(t); // **加速補間に変換**

            for (int i = 0; i < coin; i++)
            {
                coins[i].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPositions[i], endPos, t); // **UI移動**
            }

            yield return null;
        }

        // **最終位置を確定**
        for (int i = 0; i < coin; i++)
        {
            SoundSEManager.Instance.PlayGetMoneySound(); // コイン取得のSEを再生
            coins[i].GetComponent<RectTransform>().anchoredPosition = endPos;
            coins[i].SetActive(false); // コインを非表示にする
            coins[i].GetComponent<RectTransform>().anchoredPosition = startPositions[i];
            yield return new WaitForSeconds(0.1f); // 少し待機してから次のコインへ
        }

        for (int i = 0; i < GetCoinNum; i++)
        {
            

            CoinNum++;
            proText.text = "×" + CoinNum.ToString(); // コインの数を更新
            yield return new WaitForSeconds(0.01f); // コインの数を更新する間隔
        }

        if (RemainTime <= 0)
        {
            Get3CoinsImage.SetActive(true); // 3コインの画像を表示
            RemainTime = 300;
            
        }

        PlayerPrefs.SetInt("CoinNum", CoinNum); // コインの数をリセット
    }

    public void GetItemButton()
    {
        GetCoinNum = 3; 
        StartCoroutine(ShowCoinsThenMove(3));
        Get3CoinsImage.SetActive(false); // 3コインの画像を非表示にする
    }

   float EaseOutQuad(float t)
    {
        return 1 - (1 - t) * (1 - t); // **加速**
    }

}
