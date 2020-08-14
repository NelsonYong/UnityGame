using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreatManagerVarsContainer")]//标签头文件
public class ManagerVars : ScriptableObject {
    public static ManagerVars GetManagerVars()
    {

        return Resources.Load<ManagerVars>("ManagerVarsContainer");

    }
    public List<Sprite> bglist = new List<Sprite>();//背景
    public List<Sprite>  platformList = new List<Sprite>();//台阶
    public List<Sprite> skinSpriteList = new List<Sprite>();//皮肤
    public List<string> skinNameList = new List<string>();//皮肤名字
    public List<int> skinPrice = new List<int>();//皮肤价格
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();//皮肤

    public GameObject normalPlatformPre;//获取普通平台预制体
    public GameObject charactersPre;//获取玩家预制体
    public GameObject deathEffect;//特效预制体
    public GameObject skinChooseItemPre;//获取皮肤预制体


    public List<GameObject> commonPlatformGroup = new List<GameObject>();//获取通用平台预制体
    public List<GameObject> winterPlatformGroup = new List<GameObject>();//获取冬季平台预制体
    public List<GameObject> grassPlatformGroup = new List<GameObject>();//获取草地平台预制体
    public GameObject spikePlatformLeft;//左边钉子平台
    public GameObject spikePlatformRight;//右边钉子平台
    public GameObject diamondPre;//钻石
    public GameObject diamondPre1;//双倍分数
    public float nextXPos = 0.554f, nextYPos = 0.645f;//x轴方向的差值，y方向的差值

    public AudioClip jumpClip, fallClip, hitClip, diamondClip, buttonClip,bgMusic;
    public Sprite musicOn, musicOff;
}
