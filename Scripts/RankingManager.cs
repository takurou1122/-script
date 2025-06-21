using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YuRinChiLibrary.PlayFab;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static int inputname = 0;
    public TMPro.TMP_InputField inputField;
    public GameObject CreatNameImage;
    public GameObject LoginButtonObject;
    public GameObject CreatNameButton;

    public string userName;
    // Start is called before the first frame update
    void Start()
    {
        inputname = PlayerPrefs.GetInt("inputname", 0); // PlayerPrefsから名前入力のフラグを取得
        userName = PlayerPrefs.GetString("UserName", "Player"); // PlayerPrefsからユーザー名を取得
        if (inputname == 0)
        {
            inputField.onEndEdit.AddListener(CreatName);
            CreatNameImage.SetActive(true);
            inputname = 1;
            CreatNameButton.SetActive(true);
        }
        else
        {
            LoginButtonObject.SetActive(true);

        }
        PlayerPrefs.SetInt("GetCoin",0);
        SoundBGMManager.Instance.PlayTitleAndMenuBGM(); // タイトル画面のBGMを再生


    }

    // Update is called once per frame
    void Update()
    {

    }




    async public void LoginButton()
    {
        await PlayFabManager.Instance.StartLogin();

        SceneManager.LoadScene("StageChoseScene");
    }

    async public void CreatName(string userName)
    {
        await PlayFabManager.Instance.UpdateUserName(userName);
        Debug.Log($"入力された名前: {userName}");
        CreatNameImage.SetActive(false);
        CreatNameButton.SetActive(false);
        LoginButtonObject.SetActive(true);
        SaveUserDitails(userName,1);


    }
    
    public void SaveUserDitails(string userName, int inputname)
    {
        PlayerPrefs.SetString("UserName", userName); // ユーザー名を保存
        PlayerPrefs.SetInt("inputname", inputname); // 名前入力のフラグを保存
        PlayerPrefs.Save(); // PlayerPrefsの変更を保存
    }



    
}
