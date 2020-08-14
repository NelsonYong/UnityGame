using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool Instance;

    public int initSpawnCount = 5;
    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> winterPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();
    private List<GameObject> deathEffectList = new List<GameObject>();
    private List<GameObject> diamondList= new List<GameObject>();
    private List<GameObject> doubleList = new List<GameObject>();
    //private List<GameObject> diamondList = new List<GameObject>();
    private ManagerVars vars;

    public int lift;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        AddLift();
        Init();
       
    }
    public void AddLift() {

        lift = lift + 1;
    
    }
    private void Init()
    {
       
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.commonPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGroup[j], ref commonPlatformList);

            }

        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.grassPlatformGroup[j], ref grassPlatformList);

            }

        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.winterPlatformGroup[j], ref winterPlatformList);

            }

        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
        }

        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.deathEffect, ref deathEffectList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.diamondPre, ref diamondList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.diamondPre1, ref doubleList);
        }
    }


    private GameObject InstantiateObject(GameObject prefab, ref List<GameObject> addList)
    {
        GameObject go = Instantiate(prefab, transform);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }

    public GameObject GetNormalPlatform()
    {
        for (int i = 0; i < normalPlatformList.Count; i++)
        {
            //没有在场景中显示，就返回出去
            if (normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }
        //若没有相应的物体的话，重新调用生成函数，生成出来的物体重新加入到对象池中
        return InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
    }

    public GameObject GetCommonPlatformGroup()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.commonPlatformGroup.Count);
        return InstantiateObject(vars.commonPlatformGroup[ran], ref commonPlatformList);
    }

    public GameObject GetGrassPlatformGroup()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.grassPlatformGroup.Count);
        return InstantiateObject(vars.grassPlatformGroup[ran], ref grassPlatformList);
    }

    public GameObject GetWinterPlatformGroup()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.winterPlatformGroup.Count);
        return InstantiateObject(vars.winterPlatformGroup[ran], ref winterPlatformList);
    }
    public GameObject GetLeftSpikePlatform()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
    }
    /// <summary>
    /// 获取右边钉子组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetRightSpikePlatform()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
    }

    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }
        return InstantiateObject(vars.deathEffect, ref deathEffectList);
    }
    /// <summary>
    /// 获取钻石
    /// </summary>
    /// <returns></returns>
    public GameObject GetDiamond()
    {
        for (int i = 0; i < diamondList.Count; i++)
        {
            if (diamondList[i].activeInHierarchy == false)
            {
                return diamondList[i];
            }
        }
        return InstantiateObject(vars.diamondPre, ref diamondList);
    }
    public GameObject GetDouble()
    {
        for (int i = 0; i < doubleList.Count; i++)
        {
            if (doubleList[i].activeInHierarchy == false)
            {
                return doubleList[i];
            }
        }
        return InstantiateObject(vars.diamondPre1, ref doubleList);
    }
}
