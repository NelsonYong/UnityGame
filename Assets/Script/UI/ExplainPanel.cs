using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class ExplainPanel : MonoBehaviour {

   
    private Button btn_close;
    private Image img_Bg;
    private GameObject dialog;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowExplainPanel, Show);
        img_Bg = transform.Find("bg").GetComponent<Image>();
       
        btn_close = transform.Find("Dialog/btn_close").GetComponent<Button>();
        btn_close.onClick.AddListener(OnNoButtonClick);
        dialog = transform.Find("Dialog").gameObject;

        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowExplainPanel, Show);
    }
    private void Show()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f);
    }

   
    /// <summary>
    /// 否按钮点击
    /// </summary>
    private void OnNoButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
