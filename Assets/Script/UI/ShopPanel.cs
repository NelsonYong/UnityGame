using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ShopPanel : MonoBehaviour {

    private ManagerVars vars;
    private Transform parent;
    private Text txt_Name;
    private Text txt_Diamond;
    private Text txt_Price;
    private Button btn_Back;
    private Button btn_Select;
    private Button btn_Buy;
    private Button btn_add;
    private Button btn_reduce;
    private Text LuckCount;
    private GameObject chang_dia;
    private int selectIndex;
    private int Luck_price = 3;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowShopPanel, Show);
        parent = transform.Find("ScroolRect/Parent");
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_Diamond = transform.Find("Diamond/Text").GetComponent<Text>();
        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        btn_Select = transform.Find("btn_Select").GetComponent<Button>();
        btn_Select.onClick.AddListener(OnSelectButtonClick);
        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        btn_Buy.onClick.AddListener(OnBuyButtonClick);
        txt_Price = transform.Find("btn_Buy/txt_Price").GetComponent<Text>();
        chang_dia = transform.Find("btn_Buy/change_dia").GetComponent<GameObject>();

        btn_add = transform.Find("btn_add").GetComponent<Button>();
        btn_add.onClick.AddListener(OnAddLiftButtonClick);
        btn_reduce = transform.Find("btn_reduce").GetComponent<Button>();
        btn_reduce.onClick.AddListener(OnReduceLiftButtonClick);
        LuckCount = transform.Find("LuckCount").GetComponent<Text>();
        vars = ManagerVars.GetManagerVars();
    }
    private void OnDestroy()
    {

        EventCenter.RemoveListener(EventDefine.ShowShopPanel, Show);

    }

    private void Start()
    {
        LuckCount.text = GameManager.Instance.GetLuck().ToString();
        Init();
        gameObject.SetActive(false);
    }
    
  
    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }
    private void OnBuyButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        int price = int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
       
        
        if (price > GameManager.Instance.GetAllDiamond())
        {


            EventCenter.Broadcast(EventDefine.Hint, "钻石不足");
            return;
        }
       
            GameManager.Instance.UpdateAllDiamond(-price);
            GameManager.Instance.SetSkinUnloacked(selectIndex);
            parent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
        


    }

    private void OnSelectButtonClick()
    {
        Debug.Log(selectIndex);
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ChangeSkin, selectIndex);
        GameManager.Instance.SetSelectedSkin(selectIndex);
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);

    }
    private void OnAddLiftButtonClick()
    {
       
        if (GameManager.Instance.GetLuck() <5)
        {
            if (Luck_price > GameManager.Instance.GetAllDiamond())
            {

                EventCenter.Broadcast(EventDefine.Hint, "钻石不足");
                return;
            }
            Debug.Log("yes");
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            EventCenter.Broadcast(EventDefine.AddLuck);
            EventCenter.Broadcast(EventDefine.UpdateLuckText);
            LuckCount.text = GameManager.Instance.GetLuck().ToString();
            GameManager.Instance.UpdateAllDiamond(-Luck_price);
          
        }


    }
    private void OnReduceLiftButtonClick()
    {
        if (GameManager.Instance.GetLuck() > 0)
        {
            if (Luck_price > GameManager.Instance.GetAllDiamond())
            {

                EventCenter.Broadcast(EventDefine.Hint, "钻石不足");
                return;
            }
            Debug.Log("no");
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            EventCenter.Broadcast(EventDefine.ReduceLuck);
            EventCenter.Broadcast(EventDefine.UpdateLuckText);
            LuckCount.text = GameManager.Instance.GetLuck().ToString();
            GameManager.Instance.UpdateAllDiamond(Luck_price);
        }


    }
    private void Show()
    {


        gameObject.SetActive(true);

    }

    private void Init()
    {

        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 2.0f, 3);
        txt_Diamond.text = GameManager.Instance.GetAllDiamond().ToString();
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go = Instantiate(vars.skinChooseItemPre, parent);
            //未解锁
          
            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else//解锁了
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * 2, 0, 0);

        }
        parent.transform.localPosition =
           new Vector3(GameManager.Instance.GetCurrentSelectedSkin() * -2, 0);
    }

    private void Update()
    {
        selectIndex = (int)Mathf.Round(parent.transform.localPosition.x / -2.0f);
        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(selectIndex * -2, 0.5f);
        }
        SetItemSize(selectIndex);
        RefreshUI(selectIndex);
    }
    private void SetItemSize(int selectIndex)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (selectIndex == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1.6f, 1.6f);
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0.8f, 0.8f);
            }
        }
    }
    private void RefreshUI(int selectIndex)
    {
        txt_Name.text = vars.skinNameList[selectIndex];
        txt_Diamond.text = GameManager.Instance.GetAllDiamond().ToString();
        //未解锁
        if (GameManager.Instance.GetSkinUnlocked(selectIndex) == false)
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            // btn_Buy.GetComponentInChildren<Text>().text = vars.skinPrice[selectIndex].ToString();
            //if (selectIndex == 3) 
            //{
                
            //    txt_Price.text = "抽奖解锁";
            //    return;

            //}
         
                txt_Price.text = vars.skinPrice[selectIndex].ToString();
                
               
           
              
        }
        else
        {
            btn_Buy.gameObject.SetActive(false);
            btn_Select.gameObject.SetActive(true);
            
        }
    }
}
