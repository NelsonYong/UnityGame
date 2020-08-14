using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,
    Winter
}
public class platfromSpawner : MonoBehaviour {

    public  int milestoneCount = 10;
    public float fallTime;
    public float minFallTime;
    public float multiple;

    private Vector3 startSpawnPos=new Vector3(0,-2.4f,0);//平台初始位置
    private int spawnPlatformCount;//生成平台的数量
    private ManagerVars vars;
    private bool isLeftSpawn = false;//判断向左还是向右生成平台
    private bool spikeSpawnLeft=false;//钉子平台的方向
    private Sprite selectPlatformSprite;//选择平台主题
    private PlatformGroupType groupType;//组合平台类型
    private int afterSpawnSpikeSpawnCount;//钉子平台方向的普通平台生成数量
    private Vector3 spikeDirPlatformPos;
    private bool isSpawnSpike;
  


    private Vector3 platformSpawnPosition;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }

    private void OnDestroy() {

        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);

    }
    private void Start() {
        RandomPlatformTheme();
      
        platformSpawnPosition = startSpawnPos;//初始化平台位置

        for (int i = 0; i < 5; i++) {
            spawnPlatformCount = 5;
            DecidePath();
        }

        //初始化人物位置
        GameObject go1 = Instantiate(vars.charactersPre);
        go1.transform.position = new Vector3(0, -1.8f, 0);
    }

    private void Update()
    {
        Updatadifficult();

        if (GameManager.Instance.IsGameStarted && GameManager.Instance.IsGameOver == false)
        {
          
            UpdateFallTime();
        }
    }
    private void Updatadifficult() 
    {
        if (GameManager.Instance.GetGameScore() > 6) 
        {

            if (GameManager.Instance.GetLv() != 5) 
            {
                multiple = 0.9f - (GameManager.Instance.GetLv() / 25.0f);
                minFallTime = 0.5f - (GameManager.Instance.GetLv() / 28.0f);
                return;

            }
            multiple = 0.47f;
            minFallTime = 0.12f;
            return;

        }

         



    }
    /// <summary>
    /// 更新平台掉落时间
    /// </summary>
    private void UpdateFallTime()
    {
        if (GameManager.Instance.GetGameScore() > milestoneCount)
        {
            milestoneCount *= 2;
            fallTime= fallTime * multiple;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformList.Count);//随机获取平台
        selectPlatformSprite = vars.platformList[ran];
        //冬季的时候选择冬季主题平台
        if (ran == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        //其他的时候都选择草地主题的平台
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }
    private void DecidePath() {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }

        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();

        }

    }
    private void SpawnPlatform()
    {
        int ranObstacleDir = Random.Range(0, 2);//传递路径的随机值
        //生成单个平台
        if (spawnPlatformCount >= 1)
        {
            SpawnNormalPlatform(ranObstacleDir);
        }
        //生成组合平台，在最后的一个平台的时候，分岔口的时候会出现组合平台
        else if (spawnPlatformCount == 0)
        {
            int ran = Random.Range(0, 3);
            //生成通用组合平台
            if (ran == 0)
            {
                SpawnCommonPlatformGroup(ranObstacleDir);
            }
            //SpawnCommonPlatformGroup(ranObstacleDir);
            else if (ran == 1)
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranObstacleDir);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranObstacleDir);
                        break;
                    default:
                        break;
                }
            }
            else 
            {
                int value = -1;
                if (isLeftSpawn)
                {
                    value = 0;//生成右边方向得钉子
                }
                else
                {
                    value = 1;//生成左边方向得钉子
                }
                SpawnSpikePlatform(value);

                isSpawnSpike = true;
                afterSpawnSpikeSpawnCount = 4;
                if (spikeSpawnLeft)//钉子在左边
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x - 1.65f,
                        platformSpawnPosition.y + vars.nextYPos, 0);
                }
                else
                {
                    spikeDirPlatformPos = new Vector3(platformSpawnPosition.x + 1.65f,
                        platformSpawnPosition.y + vars.nextYPos, 0);
                }
              

            }

            
        }

        //随机生成钻石
        int ranSpawnDiamond = Random.Range(0, 8);
        int ranSpawnDiamond1= Random.Range(0, 15);
        if (ranSpawnDiamond >= 6 && GameManager.Instance.PlayerIsMove)
        {
            GameObject go = ObjectPool.Instance.GetDiamond();
            go.transform.position = new Vector3(platformSpawnPosition.x,
                platformSpawnPosition.y + 0.5f, 0);
            go.SetActive(true);
        }
        if (ranSpawnDiamond1 >13 && GameManager.Instance.PlayerIsMove)
        {
            GameObject go = ObjectPool.Instance.GetDouble();
            go.transform.position = new Vector3(platformSpawnPosition.x,
                platformSpawnPosition.y + 0.5f, 0);
            go.SetActive(true);
        }


        if (isLeftSpawn)//向左生成
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,platformSpawnPosition.y + vars.nextYPos, 0);
        }
        else//向右生成
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,platformSpawnPosition.y + vars.nextYPos, 0);
        }

    }

    private void SpawnNormalPlatform(int ranObstacleDir)//生成正常平台
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite,fallTime, ranObstacleDir);//将平台主题传进去
        go.SetActive(true);
    }
    private void SpawnCommonPlatformGroup(int ranObstacleDir)//生成同用平台
    {
       
        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);//将平台主题传进去
        go.SetActive(true);
    }
    private void SpawnGrassPlatformGroup(int ranObstacleDir)//生成草地平台
    {
        GameObject go = ObjectPool.Instance.GetGrassPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);//将平台主题传进去
        go.SetActive(true);
    }
    private void SpawnWinterPlatformGroup(int ranObstacleDir)//生成冬季平台
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacleDir);//将平台主题传进去
        go.SetActive(true);

    }
    private void SpawnSpikePlatform(int dir)
    {

        GameObject temp = null;
        if (dir == 0)
        {
            spikeSpawnLeft = false;
            temp = ObjectPool.Instance.GetRightSpikePlatform();
        }
        else
        {
            spikeSpawnLeft = true;
            temp = ObjectPool.Instance.GetLeftSpikePlatform();

        }
        temp.transform.position = platformSpawnPosition;
        temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, dir);//将平台主题传进去
        temp.SetActive(true);



    }

    private void AfterSpawnSpike()
    {
        if (afterSpawnSpikeSpawnCount > 0)
        {
            afterSpawnSpikeSpawnCount--;
            for (int i = 0; i < 2; i++)
            {
                GameObject temp = ObjectPool.Instance.GetNormalPlatform();
                if (i == 0)//生成原来方向的平台
                {
                    temp.transform.position = platformSpawnPosition;
                    //如果钉子在左边，原先路径就是右边
                    if (spikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos,
                            platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                }
                else//生成钉子方向的平台
                {
                    temp.transform.position = spikeDirPlatformPos;
                    if (spikeSpawnLeft)
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - vars.nextXPos,
                            spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos,
                            spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                }

                temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, 1);
                temp.SetActive(true);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
}
