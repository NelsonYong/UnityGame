using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DifficultPanel : MonoBehaviour {

    private Button btn_Close;
    private GameObject go_ScoreList;

    public Button btn_Add;
    public Button btn_Reduce;
    public Text txt_lv;
    public Text txt_time;
    public Text txt_platCount;
    public Button btn_explain;
    public Slider LvSlider;


    private void Awake()
    {
       
        EventCenter.AddListener(EventDefine.ShowDifficultPanel, Show);
        btn_Close = transform.Find("btn_back").GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);
        btn_Add.onClick.AddListener(OnAddButtonClick);
        btn_Reduce.onClick.AddListener(OnReduceButtonClick);
        btn_explain.onClick.AddListener(OnExplainButtonClick);

        go_ScoreList = transform.Find("DifficultList").gameObject;
        btn_Close.GetComponent<Image>().color = new Color(btn_Close.GetComponent<Image>().color.r, btn_Close.GetComponent<Image>().color.g, btn_Close.GetComponent<Image>().color.b, 0);
        go_ScoreList.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowDifficultPanel, Show);
    }

    private void Start() {
       
        LvSlider.value = GameManager.Instance.GetLv();
        txt_lv.text = "级别" + GameManager.Instance.GetLv();
        txt_time.text = "限时" + (60 - (GameManager.Instance.GetLv() - 1) * 12).ToString()+"S";
        txt_platCount.text = "竞速" + (100 - (GameManager.Instance.GetLv() - 1) * 12).ToString()+"阶";
    }

  
    private void Show()
    {
        gameObject.SetActive(true);
        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().
            color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0.3f), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.one, 0.3f);

       
    }
   
    private void OnCloseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().
            color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnAddButtonClick()
    {
        if (GameManager.Instance.GetLv() <5)
        {
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            EventCenter.Broadcast(EventDefine.AddLv);
            LvSlider.value = GameManager.Instance.GetLv();
            txt_time.text = "限时" + (60 - (GameManager.Instance.GetLv() - 1) * 12).ToString() + "S";
            txt_platCount.text = "竞速" + (100 - (GameManager.Instance.GetLv() - 1) * 12).ToString() + "阶";
            if (GameManager.Instance.GetLv() != 5)
                txt_lv.text = "级别" + GameManager.Instance.GetLv();
            else
                txt_lv.text = "非人级别";

        }
        return;
    }
    private void OnReduceButtonClick()
    {
        if (GameManager.Instance.GetLv()>= 2)
        {
           
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            EventCenter.Broadcast(EventDefine.ReduceLv);
            LvSlider.value = GameManager.Instance.GetLv();
            txt_time.text = "限时" + (60 - (GameManager.Instance.GetLv() - 1) * 12).ToString() + "S";
            txt_platCount.text = "竞速" + (100 - (GameManager.Instance.GetLv() - 1) * 12).ToString() + "阶";
            if (GameManager.Instance.GetLv() != 5)
                txt_lv.text = "级别" + GameManager.Instance.GetLv();
            else
                txt_lv.text = "非人级别";
        }
        return;
    }

    private void OnExplainButtonClick()
    {
       
        EventCenter.Broadcast(EventDefine.ShowExplainPanel);
    }
}
