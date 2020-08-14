using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

    private Button btn_Pause;
    private Button btn_Play;
    private Button btn_Luck;
    private Text txt_Score;
    private Text txt_DiamondCount;
    private Text txt_count;
    private Text txt_Time;
    private Text txt_Time1;
    private static bool flag_time;
    private static bool flag_time1;
  

    private void Awake()
    {
       
        EventCenter.AddListener(EventDefine.ShowGamePanel,Show);
        EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.AddListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
        EventCenter.AddListener<bool>(EventDefine.Mode, ShowTime);
        EventCenter.AddListener<bool>(EventDefine.Mode1, ShowTime1);
        EventCenter.AddListener(EventDefine.UpdateLuckText, UpdateLuckText);
       
        Init();
        
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
        EventCenter.RemoveListener(EventDefine.UpdateLuckText, UpdateLuckText);
        EventCenter.RemoveListener<bool>(EventDefine.Mode, ShowTime);
        EventCenter.RemoveListener<bool>(EventDefine.Mode1, ShowTime1);

    }

   

   

    //初始化
    private void Init() {
        btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
        btn_Pause.onClick.AddListener(OnPauseButtonClick);
        btn_Play = transform.Find("btn_Play").GetComponent<Button>();
        btn_Play.onClick.AddListener(OnPlayButtonClick);
        txt_Score = transform.Find("score").GetComponent<Text>();
        txt_Time= transform.Find("txt_time").GetComponent<Text>();
        txt_Time1 = transform.Find("txt_time1").GetComponent<Text>();

        txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();
        txt_count = transform.Find("txt_count").GetComponent<Text>();
        btn_Luck = transform.Find("btn_Luck").GetComponent<Button>();
        btn_Luck.onClick.AddListener(OnLuckButtonClick);
        btn_Play.gameObject.SetActive(false);
        gameObject.SetActive(false);
      

    }

    private void Show()
    {
       
            gameObject.SetActive(true);
        
    }

    private void ShowTime(bool flag)
    {
        flag_time = flag;
        
    }
    private void ShowTime1(bool flag)
    {
        flag_time1 = flag;

    }

    private void Start() {

      
        txt_count.text = "X" + GameManager.Instance.GetLuck();
        txt_Time.gameObject.SetActive(flag_time);
        txt_Time1.gameObject.SetActive(flag_time1);

    }
   
    /// <summary>
    /// 更新成绩显示
    /// </summary>
    /// <param name="score"></param>
    private void UpdateScoreText(int score)
    {
        txt_Score.text = score.ToString();
       
      
    }
    /// <summary>
    /// 更新钻石分数
    /// </summary>
    /// <param name="score"></param>
    private void UpdateDiamondText(int diamond)
    {
        txt_DiamondCount.text = diamond.ToString();
    }
    private void UpdateLuckText()
    {
        txt_count.text = "X" + GameManager.Instance.GetLuck();
    }
    private void OnPauseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);
        Time.timeScale = 0;
        GameManager.Instance.IsPause = true;
    }
   
    private void OnPlayButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        btn_Play.gameObject.SetActive(false);
        btn_Pause.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.IsPause = false;
    }
    private void OnLuckButtonClick()
    {
        if (GameManager.Instance.GetLuck() != 0)
        {
            int rand = Random.Range(0, 20);
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            EventCenter.Broadcast(EventDefine.ShowLuck, rand);
        }
        else
        {
            EventCenter.Broadcast(EventDefine.Hint, "幸运器不足");
            
        }
       
       
    }
}
