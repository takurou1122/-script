using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // UIコンポーネントを使用するために必要
using TMPro;


public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static bool isGameOver = false; //ゲームオーバーのフラグ
    public static bool isGameClear = false;
    public float timeRemaining = 0f; // 残り時間（秒）
    public static int AroundNum ;
    public static float AliveTime; // プレイヤーの生存時間（秒）
    public TMP_Text timerText;

    public Image fadeGameOverPanel;             // フェード用のUIパネル（Image）
    public Image fadeGameClearPanel;             // フェード用のUIパネル（Image）

    public float fadeDuration = 1.5f;   // フェードの完了にかかる時間
    public TextAnimation TextAnimation; // テキストアニメーションのスクリプト
    private float maxFontSize = 180f;
    private float minFontSize = 140f;

    public Slider TimerSlider;

    public int NomalItemNum; // ノーマルアイテムの数
    public static bool TorchEnemy = true; // ノータッチモードのフラグ

    public GameObject GardPanel; // ガードパネルのGameObject
    public Button NonalItemButton; // ノーマルアイテムボタン
    public Text NomalItemNumText;

    public Text ItemTimeCountText; // アイテム使用時間のカウントダウンテキスト


    void Start()
    {
        NomalItemNum = PlayerPrefs.GetInt("NomalItemNum", 0); // NomalItemNumをPlayerPrefsから取得
        Debug.Log("NomalItemNum: " + NomalItemNum); // デバッグ用ログ
        isGameClear = false;
        isGameOver = false; // ゲームオーバーのフラグを初期化
        SoundBGMManager.Instance.PlayGameBGM(SceneManager.GetActiveScene().name); 
        
        
        
    }
    // Update is called once per frame
    void Update()
    {
        if (timeRemaining <= 20f && !isGameOver) // ゲームオーバーでない場合のみタイマーを更新
        {
            timeRemaining += Time.deltaTime;
            float sizeFactor = (timeRemaining / 20f); // 0～1 の値を計算
            timerText.fontSize = Mathf.Lerp(minFontSize, maxFontSize, sizeFactor);
            timerText.text = timeRemaining.ToString("F1") + " sec";
            TimerSlider.value = timeRemaining; // スライダーの値を更新（0～1の範囲）

        }
        else if (timeRemaining >= 20f && !isGameOver && !isGameClear)
        {
            GameClear();
            timerText.text = "";//現在の累積タイムを表示
        }
        else
        {
            timerText.text = "";
        }

        if (NomalItemNum <= 0)
        {
            NonalItemButton.interactable = false;
        }
        else
        {
            NonalItemButton.interactable = true; // ノーマルアイテムボタンを有効にする
        }
        NomalItemNumText.text = NomalItemNum.ToString(); // ノーマルアイテムの数を表示するテキストを更新


    }

    public void GameOver()
    {
        isGameOver = true; // ゲームオーバーのフラグを立てる
        StartCoroutine(FadeOutAndLoadScene(false));
        SoundBGMManager.Instance.StopBGM(); // BGMを停止
        SoundSEManager.Instance.PlayGameOverSound(); // ゲームオーバーのSEを再生
    }

    public void GameClear()
    {
        isGameClear = true; // ゲームクリアのフラグを立てる
        Debug.Log("Game Clear!");
        StartCoroutine(FadeOutAndLoadScene(true));
        SoundBGMManager.Instance.StopBGM(); // BGMを停止

    }

    public void SaveTime(float time)
    {
        if (AliveTime < time)
        {
            AliveTime = time; // 新しい生存時間が現在の生存時間より長い場合のみ更新
        }
            
        PlayerPrefs.SetFloat("AliveTime", AliveTime); // PlayerPrefsに生存時間を保存
        
    }

    
    public void LoadTime()
    {
        AliveTime = PlayerPrefs.GetFloat("AliveTime", 0f); // PlayerPrefsから生存時間を読み込む
        
    }

    public void UseNomalItem()
    {
        StartCoroutine(UseItem()); // アイテム使用のコルーチンを開始
        
    }

    IEnumerator UseItem()
    {
        SoundBGMManager.Instance.PauseBGM(); // BGMを一時停止
        NomalItemNum--;
        float elapsedTime = 3.0f;
        TorchEnemy = false; // ノータッチモードを有効にする
        GardPanel.SetActive(!TorchEnemy); // ガードパネルを表示 
        ItemTimeCountText.gameObject.SetActive(!TorchEnemy); // アイテム使用時間のカウントダウンテキストを表示
        int lastAnnouncedSecond = -1;
        while (elapsedTime >= 0f)
        {

            int currentSecond = Mathf.FloorToInt(elapsedTime);
            if (currentSecond != lastAnnouncedSecond)
            {
                // 1回だけ再生する処理
                lastAnnouncedSecond = currentSecond;
                SoundSEManager.Instance.PlayUseItemSound(); // アイテム使用のSEを再生
            }


            elapsedTime -= Time.deltaTime;
            ItemTimeCountText.text = elapsedTime.ToString("F1");

            // 経過時間を増やす

            yield return null;                                     // 1フレーム待機
        }


        TorchEnemy = true; // ノータッチモードを無効にする
        GardPanel.SetActive(!TorchEnemy); // ガードパネルを非表示
        ItemTimeCountText.gameObject.SetActive(!TorchEnemy); // アイテム使用時間のカウントダウンテキストを非表示
        ItemTimeCountText.text = 3.ToString("F1");
        SoundBGMManager.Instance.ResumeBGM(); // BGMを再開
        
    }

    


    




    public IEnumerator FadeOutAndLoadScene(bool isgameClear)
    {
        ItemTimeCountText.gameObject.SetActive(false);
        GardPanel.SetActive(false); // ガードパネルを表示
        NonalItemButton.gameObject.SetActive(false); // ノーマルアイテムボタンを非表示
        NomalItemNumText.gameObject.SetActive(false); // ノーマルアイテムの数を表示するテキストを非表示
        float elapsedTime = 0.0f;
        Image fadePanel = isGameClear ? fadeGameClearPanel : fadeGameOverPanel; // ゲームクリアかゲームオーバーかでパネルを選択
        fadePanel.enabled = true;
        Color startColor = fadePanel.color;       // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定            


        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        PlayerPrefs.SetInt("NomalItemNum",NomalItemNum);
        fadePanel.color = endColor;  // フェードが完了したら最終色に設定
        StartCoroutine(TextAnimation.Simple(isgameClear)); // テキストアニメーションを開始
        // SceneManager.LoadScene(""); // シーンをロードしてメニューシーンに遷移
    }

    
}

    

