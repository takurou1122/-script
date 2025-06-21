using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using YuRinChiLibrary.PlayFab;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text GameOverText; // ゲームオーバーのテキスト
    [SerializeField] private TMP_Text GameClearText; // ゲームクリアのテキスト

    [SerializeField] private TMP_Text TotalTimeText; // ゲームクリアのテキスト
    private PlayerManager PlayerManager; // PlayerManagerの参照


    public GameObject ReturnButtonObject;

   


    public int stagenum;

    private int MaxStageNum = 7;


    void Start()
    {
        ReturnButtonObject.SetActive(false);
        PlayerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    public IEnumerator Simple(bool isgameclear)
    {
        TMP_Text tmpText = isgameclear ? GameClearText : GameOverText; // ゲームオーバーのテキストを使用
        GameObject textObject = isgameclear ? GameClearText.gameObject : GameOverText.gameObject; // ゲームオーバーのテキストオブジェクトを取得
        string sceneName = SceneManager.GetActiveScene().name; // 例: Sceneの名前
        string number = Regex.Match(sceneName, @"\d+").Value; // 数字のみ抽出
        if (isgameclear)
        {
            if ((int.Parse(number) + 1) % MaxStageNum == 0)
            {
                PlayerManager.AroundNum++;
                number = "-1";
            }
        }
        stagenum = int.Parse(number);
        if (isgameclear)
        {
            tmpText.text = $"Stage {stagenum + 2} Round {PlayerManager.AroundNum}";
        }
        

        textObject.SetActive(true);
        tmpText.maxVisibleCharacters = 0;

        // テキストの文字数分ループ
        for (var i = 0; i < tmpText.text.Length; i++)
        {
            // 一文字ごとに0.2秒待機
            
            yield return new WaitForSeconds(0.2f);
            SoundSEManager.Instance.PlayNextStageSound();

            // 文字の表示数を増やしていく
            tmpText.maxVisibleCharacters = i + 1;
        }
        ReturnButtonObject.SetActive(isgameclear);
        if (isgameclear)
        {
            yield return new WaitForSeconds(1f); // 1秒待機してから次のステージへ
            int NextStageNum = stagenum + 1;
            SceneManager.LoadScene("Stage" + NextStageNum.ToString());
        }
        else
        {
            StartCoroutine(ShowTotalTime());
        }

    }

    public IEnumerator ShowTotalTime()
    {


        TotalTimeText.gameObject.SetActive(true);
        TotalTimeText.maxVisibleCharacters = 0;
        TotalTimeText.text = ((PlayerManager.AroundNum * MaxStageNum * 20f) + (20f * stagenum) + PlayerManager.timeRemaining).ToString("F1") + " sec";
        PlayerManager.AliveTime = PlayerManager.timeRemaining + (PlayerManager.AroundNum * MaxStageNum * 20f) + (20f * stagenum);
        
        Debug.Log("TotalTime;" + PlayerManager.AliveTime + "TimeRemaining:" + PlayerManager.timeRemaining + "AroundNum:" + PlayerManager.AroundNum + "stagenum:" + stagenum);

        yield return SubmitScoreAsync().ToCoroutine();


        for (var i = 0; i < TotalTimeText.text.Length; i++)
        {
            // 一文字ごとに0.2秒待機
            yield return new WaitForSeconds(0.2f + (i * 0.05f));
            SoundSEManager.Instance.PlayNextStageSound();

            // 文字の表示数を増やしていく
            TotalTimeText.maxVisibleCharacters = i + 1;
        }
        ReturnButtonObject.SetActive(true);
        PlayerManager.AroundNum = 0;

    }



    public void ReturnStage()
    {
        SceneManager.LoadScene("StageChoseScene");
        PlayerPrefs.SetInt("hardMode",0); // スコアを保存

    }
    
    private async UniTask SubmitScoreAsync()
    {
        int score = Mathf.FloorToInt(PlayerManager.AliveTime);
        PlayerPrefs.SetInt("Score", score);
        int GetCoin = score / 20;
        PlayerPrefs.SetInt("GetCoin",GetCoin);
        
        
        // 通信開始前の処理をココに書く

        try
        {
            bool success = await PlayFabManager.Instance.SubmitMyScore(score, "HighScore");

            // 通信後の処理をココに書く
            if (success)
                Debug.Log("スコア送信成功");
            else
                Debug.Log("スコア送信失敗");
        }
        catch (Exception ex)
        {
            // 例外処理をココに書く
            Debug.LogError($"スコア送信中の例外エラー: {ex.Message}");
        }
    }
}