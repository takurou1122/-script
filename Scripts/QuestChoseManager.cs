using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YuRinChiLibrary.PlayFab;

using UnityEngine.iOS;
using System.Runtime.InteropServices;
using PlayFab.EconomyModels;


public class QuestChoseManager : MonoBehaviour
{





    private string appStoreUrl = "https://apps.apple.com/jp/app/carryball1/id6745418615";

    public static int hardMode = 0;
    private static int ReviewNum = -5;
    private bool SettingFlg = false;
    private bool ShoppingFlg = false;
    public GameObject ShoppingImage;

    public GameObject SettingPanel; // 設定パネルのGameObject
    public Slider BGMSlider;
    public Slider SESlider;

    public CoinManager CoinManager; // コインマネージャーの参照

    public Button NomalItemButton;

    public GameObject NomalItemImage; // ノーマルアイテムの画像
    public TMPro.TMP_Text NomalItemText; // ノーマルアイテムのテキスト
    private Vector2 endPos = new Vector2(-211, 540); // 目的地 (Canvas座標)
    private int NomalItemNum ; // ノーマルアイテムの数

    



    // Start is called before the first frame update
    void Start()
    {
        NomalItemNum = PlayerPrefs.GetInt("NomalItemNum", 0); // PlayerPrefsからノーマルアイテムの数を取得
        ReviewNum = PlayerPrefs.GetInt("ReviewNum", 0); // PlayerPrefsからレビューリクエストの回数を取得
        ReviewNum++;
        if (ReviewNum == 5)
        {
            RequestReview();

        }
        SoundBGMManager.Instance.PlayTitleAndMenuBGM(); // タイトル画面のBGMを再生
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.25f);
        SESlider.value = PlayerPrefs.GetFloat("SEVolume", 0.4f);
        BGMSlider.onValueChanged.AddListener(ChangeBGMVolume);
        SESlider.onValueChanged.AddListener(ChangeSEVolume);


        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RequestReview()
    {
        if (Device.RequestStoreReview())
        {
            Debug.Log("レビューリクエストを送信しました");
            ReviewNum = 30;
            PlayerPrefs.SetInt("ReviewNum", ReviewNum); // レビューリクエストの回数を保存
        }
        else
        {
            Debug.Log("レビューリクエストができません");
        }
    }

    public void NomalQuest()
    {

        SceneManager.LoadScene("Stage0");
    }


    public void Ranking()
    {
        PlayFabManager.Instance.LoadRankingScene("HighScore");
    }

    public void HardQuest()
    {
        PlayerPrefs.SetInt("hardMode", 1); // スコアを保存
        SceneManager.LoadScene("Stage0");
    }


    public void AppStore()
    {
        Application.OpenURL(appStoreUrl);
    }

    public void ToX()
    {
        Application.OpenURL("https://x.com/oootoko1129");
    }

    public void Setting()
    {
        SettingFlg = !SettingFlg;
        SettingPanel.SetActive(SettingFlg);
    }

    private void ChangeBGMVolume(float volume)
    {
        SoundBGMManager.Instance.audioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume); // 設定を保存
        PlayerPrefs.Save();
    }

    private void ChangeSEVolume(float volume)
    {
        SoundSEManager.Instance.audioSource.volume = volume;
        PlayerPrefs.SetFloat("SEVolume", volume); // 設定を保存
        PlayerPrefs.Save();
    }


    public void ToMoneyScene()
    {
        SceneManager.LoadScene("MoneyScene");
    }

    public void Shopping()
    {

        ShoppingFlg = !ShoppingFlg;
        ShoppingImage.SetActive(ShoppingFlg);
        Debug.Log("CoinNUm: " + CoinManager.CoinNum);
        if (CoinManager.CoinNum >= 7)
        {
            NomalItemButton.interactable = true;
        }
        else
        {
            NomalItemButton.interactable = false;
        }
        

    }

    public void NomalItem()
    {

        CoinManager.CoinNum -= 7;

        CoinManager.proText.text = "×" + CoinManager.CoinNum.ToString();
        StartCoroutine(MoveItem()); // アイテムを移動させる
        
        ShoppingImage.SetActive(false); // ショッピング画面を非表示にする


    }


    

    IEnumerator MoveItem()
    {
        float elapsedTime = 0;
        NomalItemImage.SetActive(true);
        Vector2 startPositions = NomalItemImage.GetComponent<RectTransform>().anchoredPosition;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 1.0f;
            t = EaseOutQuad(t); // **加速補間に変換**


            NomalItemImage.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPositions, endPos, t); // **UI移動**

            yield return null;
        }

        NomalItemImage.GetComponent<RectTransform>().anchoredPosition = endPos;
        yield return new WaitForSeconds(0.2f); // 少し待機してから非表示にする
        NomalItemImage.SetActive(false); // コインを非表示にする
        NomalItemImage.GetComponent<RectTransform>().anchoredPosition = startPositions;
        NomalItemNum++;
        NomalItemText.text = "×" + NomalItemNum.ToString();
        PlayerPrefs.SetInt("NomalItemNum", NomalItemNum); // ノーマルアイテムの数を保存
        PlayerPrefs.SetInt("CoinNum", CoinManager.CoinNum); // ノーマルアイテムの数を保存
    }
    float EaseOutQuad(float t)
    {
        return 1 - (1 - t) * (1 - t); // **加速**
    }
    





    
    
}
