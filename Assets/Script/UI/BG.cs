using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour {

    private SpriteRenderer m_SpriteRenderer;//定义2d平面变量
    private ManagerVars vars;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        int ranValue = Random.Range(0, vars.bglist.Count);
        m_SpriteRenderer.sprite = vars.bglist[ranValue];
    }
}
