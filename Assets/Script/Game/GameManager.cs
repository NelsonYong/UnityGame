using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameData data;
    private ManagerVars vars;

   
    public bool IsGameStarted { get; set; }
   // public bool IsGameTime { get; set; }
    public bool IsGameOver { get; set; }
    public bool IsPause { get; set; }
    
    public bool PlayerIsMove { get; set; }

    public bool IsBg { get; set; }

    private int gameScore;
    private int gameDiamond;

    private int LuckCount;
    private int LvCount;
   


    private bool isFirstGame;
    public bool isMusicOn;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private void Awake()
    {
        IsBg = true;
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener<int>(EventDefine.DoubleScore, AddDoubleGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddGameDiamond);
        EventCenter.AddListener(EventDefine.AddLuck, AddLuck);
        EventCenter.AddListener(EventDefine.ReduceLuck, ReduceLuck);
        EventCenter.AddListener(EventDefine.AddLv, AddLv);
        EventCenter.AddListener(EventDefine.ReduceLv, ReduceLv);

        //IsGameTime = false;
        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
           
        }
        InitGameData();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiamond);
        EventCenter.RemoveListener<int>(EventDefine.DoubleScore, AddDoubleGameScore);
        EventCenter.RemoveListener(EventDefine.AddLv, AddLv);
        EventCenter.RemoveListener(EventDefine.ReduceLv, ReduceLv);
        EventCenter.RemoveListener(EventDefine.AddLuck, AddLuck);
        EventCenter.RemoveListener(EventDefine.ReduceLuck, ReduceLuck);



    }

    public void SaveScore(int score)
    {
        List<int> list = bestScoreArr.ToList();
        //从大到小排序list
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        //50 20 10
        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                index = i;
            }
        }
        if (index == -1) return;

        for (int i = bestScoreArr.Length - 1; i > index; i--)
        {
            bestScoreArr[i] = bestScoreArr[i - 1];
        }
        bestScoreArr[index] = score;

        Save();
    }

    //获取最高分
    public int GetBestScore()
    {
        return bestScoreArr.Max();
    }

    public int[] GetScoreArr()
    {
        List<int> list = bestScoreArr.ToList();
        //从大到小排序list
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        return bestScoreArr;
    }

    /// <summary>
    /// 玩家移动会调用到此方法
    /// </summary>
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }
    /// <summary>
    /// 增加游戏成绩
    /// </summary>
    private void AddGameScore()
    {
        if (IsGameStarted == false || IsGameOver || IsPause) return;
        gameScore++;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
    }
    private void AddDoubleGameScore(int flag)
    {
        if (IsGameStarted == false || IsGameOver || IsPause) return;
        gameScore += flag;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
    }

    private void AddLuck() 
    {
        LuckCount++;
        Save();

    }
    private void ReduceLuck()
    {
        LuckCount--;
        Save();

    }
    public int GetLuck()
    {
        return LuckCount;

    }
    /// <summary>
    /// 获取游戏成绩
    /// </summary>
    public int GetGameScore()
    {
        return gameScore;
    }
    /// <summary>
    /// 更新游戏钻石数量
    /// </summary>
    private void AddGameDiamond()
    {
        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText, GetGameDiamond());
    }
    
    /// <summary>
    /// 获得吃到的钻石数
    /// </summary>
    /// <returns></returns>
    public int GetGameDiamond()
    {
        return gameDiamond;
    }
    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }
    /// <summary>
    /// 设置当前皮肤解锁
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnloacked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }
    /// <summary>
    /// 获取所有得钻石数量
    /// </summary>
    /// <returns></returns>
    public int GetAllDiamond()
    {
        return diamondCount;
    }
    /// <summary>
    /// 更新总钻石数量
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAllDiamond(int value)
    {
        diamondCount += value;
        Save();
    }
    //选择皮肤
    public void SetSelectedSkin(int index)
    {
        selectSkin = index;
        Save();
    }
    public int GetCurrentSelectedSkin()
    {
        return selectSkin;
    }
    public void SetIsMusicOn(bool value)
    {
        isMusicOn = value;
        Save();
    }

    private void AddLv() 
    {
      
        LvCount++;
        Save();
    
    }

    private void ReduceLv() 
    {
       
        LvCount--;
        Save();
    }

    public int GetLv() 
    {
        return LvCount;
    
    }

  
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameData()
    {
        Read();
        isFirstGame = true;
        if (data != null)
        {
            isFirstGame = data.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
        }
        //如果第一次开始游戏
        if (isFirstGame)
        {
           
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            LvCount = 1;
            LuckCount = 0;

            data = new GameData();
            Save();
        }
        else
        {
          
            isMusicOn = data.GetIsMusicOn();
            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();
            LuckCount = data.GetLuckCount();
            LvCount = data.GetLvCount();
        }
    }
    /// <summary>
    /// 储存数据
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                data.SetIsFirstGame(isFirstGame);
                data.SetIsMusicOn(isMusicOn);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
                data.SetLuckCount(LuckCount);
                data.SetLVCount(LvCount);
                bf.Serialize(fs, data);
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// 读取
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// 重置数据
    /// </summary>
    public void ResetData()
    {
        isFirstGame = false;
        isMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSpriteList.Count];
        skinUnlocked[0] = true;
        diamondCount = 10;
        LuckCount = 0;
        LvCount = 1;
        Save();
    }
}
