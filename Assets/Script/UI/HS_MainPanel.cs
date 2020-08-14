using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HS_MainPanel : MonoBehaviour {

    private Button btn_StartWJ;
    private Button btn_StartXS;
    private Button btn_StartJS;
    private Button btn_Shop;
    private Button btn_Rank;
    private Button btn_Sound;
    private Button btn_Reset;
    private Button btn_Difficult;
    private Button btn_Exit;
    private Button btn_Bg;
    private ManagerVars vars;
    private float t2 = 0f;
    private static bool Mainflag;
    private static int skin1=0;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.ShowMainPanel, Show);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
       
    }

    void Update()
    {

        Mainflag = GameManager.Instance.isMusicOn;
        Debug.Log("播放音乐");
        t2 = t2 + Time.deltaTime;
        if (t2 > 30.0f)
        {
            if (Mainflag)
            {
                EventCenter.Broadcast(EventDefine.PlayClickBgMusic);
                t2 = 0f;
            }
        }

    }


    private void OnDestroy() {

        Debug.Log("开始移除");
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        Debug.Log("移除结束");
     

    }

    private void Show()
    {


        gameObject.SetActive(true);
    }
    private void Start()
    {
        t2 = 30.0f;

        if (GameData.IsAgainGame)
        {
            Debug.Log(GameData.IsAgainGame);
            //GameManager.Instance.IsGameStarted = false;
            EventCenter.Broadcast(EventDefine.ShowGamePanel);
            EventCenter.Broadcast(EventDefine.UpdateLuckText);
            gameObject.SetActive(false);
        }
        Init();
        Sound();
    }

   
   
        private void Init()
    {
        btn_StartWJ = transform.Find("btn_StartWJ").GetComponent<Button>();
        btn_StartWJ.onClick.AddListener(OnStartButtonClick);
        btn_StartXS = transform.Find("btn_StartXS").GetComponent<Button>();
        btn_StartXS.onClick.AddListener(OnStart1ButtonClick);
        btn_StartJS = transform.Find("btn_StartJS").GetComponent<Button>();
        btn_StartJS.onClick.AddListener(OnStart2ButtonClick);

        btn_Exit = transform.Find("btn_Exit").GetComponent<Button>();
        btn_Exit.onClick.AddListener(OnExitButtonClick);

       


        btn_Rank = transform.Find("btns/btn_Rank").GetComponent<Button>();
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Sound = transform.Find("btns/btn_Sound").GetComponent<Button>();
        btn_Sound.onClick.AddListener(OnSoundButtonClick);
        btn_Reset = transform.Find("btns/btn_Reset").GetComponent<Button>();
        btn_Reset.onClick.AddListener(OnResetButtonClick);
        btn_Difficult= transform.Find("btns/btn_Difficult").GetComponent<Button>();
        btn_Difficult.onClick.AddListener(OnDifficultButtonClick);

        btn_Shop = transform.Find("btns/btn_Shop").GetComponent<Button>();
        btn_Shop.onClick.AddListener(OnShopButtonClick);
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite = vars.skinSpriteList[skin1];


    }
    //更换图标
    private void ChangeSkin(int skinIndex)
    {
        skin1 = skinIndex;
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite =vars.skinSpriteList[skin1];
    }
    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.Mode, false);
        EventCenter.Broadcast(EventDefine.Mode1, false);
       
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }

    private void OnStart1ButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.Mode, true);
        EventCenter.Broadcast(EventDefine.Mode1, false);
       
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }

    private void OnStart2ButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.Mode, false);
        EventCenter.Broadcast(EventDefine.Mode1, true);
      
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }
    private void OnExitButtonClick()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
 
  
    private void OnShopButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        gameObject.SetActive(false);
    }
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        Debug.Log("点击了一下排行榜按钮");
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    private void OnSoundButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());
        Sound();
    }
    private void Sound()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOn;
        }
        else
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOff;
        }
        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager.Instance.GetIsMusicOn());
    }
    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        Debug.Log("点击了一下重置按钮");
        EventCenter.Broadcast(EventDefine.ShowResetPanel);
        
    }
    private void OnDifficultButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        Debug.Log("点击了一下困难修改按钮");
        EventCenter.Broadcast(EventDefine.ShowDifficultPanel);

    }
}
