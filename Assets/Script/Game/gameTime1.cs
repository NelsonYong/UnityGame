using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameTime1 : MonoBehaviour {

    public GameObject text;
    public int jieCoun=30;
    private int TotalTime;
    private int score;
    private bool flag;
   


    void Start()
    {
      
        TotalTime =0;
        jieCoun = (100 - (GameManager.Instance.GetLv() - 1) * 12);
        score = 0;
        StartCoroutine(CountDown());

    }

    void Updata() {

        score = GameManager.Instance.GetGameScore();
        Debug.Log(score);


    }

    IEnumerator CountDown()
    {
        while (TotalTime >= 0)
        {
            score = GameManager.Instance.GetGameScore();
            if (score >= jieCoun) 
            {
                GameManager.Instance.IsGameOver = true;
                text.GetComponent<Text>().text = TotalTime.ToString() + "秒";
                EventCenter.Broadcast(EventDefine.Hint, jieCoun.ToString()+"阶用时"+ (TotalTime-1).ToString()+"秒");
                // EventCenter.Broadcast(EventDefine.ShowGameOverPanel, jieCoun.ToString() + "阶竞速成功:"+ TotalTime.ToString()+"S");
                EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
                yield return new WaitForSeconds(3);
                text.SetActive(false);
                EventCenter.Broadcast(EventDefine.ShowGameOverPanel);

            }
            if (score <= jieCoun) {

                if (GameManager.Instance.IsGameOver == true) {

                    //EventCenter.Broadcast(EventDefine.ShowGameOverPanel, jieCoun.ToString()+"阶竞速失败");

                }
            
            }
            yield return new WaitForSeconds(1);
            if (GameManager.Instance.IsGameOver != true)
            {
                TotalTime++;
            }
            text.GetComponent<Text>().text = TotalTime.ToString() + "秒";
           
        }
    }
}
