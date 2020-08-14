using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class gameTime : MonoBehaviour
{

    public GameObject text;
    public int TotalTime;
    private int time;
    private int time1;
   
    void Start()
    {
        TotalTime = (60 - (GameManager.Instance.GetLv() - 1) * 12);
         time = TotalTime;
        StartCoroutine(CountDown());

    }
    
    IEnumerator CountDown()
    {
        while (TotalTime >= 0)
        {
            if (TotalTime == 10) 
            {

                EventCenter.Broadcast(EventDefine.Hint, "时间快到了");
            }

            text.GetComponent<Text>().text = TotalTime.ToString() + "秒";

            if (TotalTime == 0)
            {
               
                GameManager.Instance.IsGameOver = true;
                EventCenter.Broadcast(EventDefine.Hint, "时间到！");
                yield return new WaitForSeconds(2);
                //EventCenter.Broadcast(EventDefine.ShowGameOverPanel,"限时"+ time .ToString()+ "成功");
                EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
              
            }
            if (TotalTime!= 0) 
            {
                if (GameManager.Instance.IsGameOver == true) {
                    time1 = TotalTime;
                    //EventCenter.Broadcast(EventDefine.ShowGameOverPanel, "限时" + time.ToString() + "失败");


                }
            }
            yield return new WaitForSeconds(1);
            if(GameManager.Instance.IsGameOver != true)
            TotalTime--;
            
           
        }
    }
}
