using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour {
    private AudioSource BgAudioSource;
    private ManagerVars vars;
    private int bgtime;
    private bool flag;
    // Use this for initialization
    private void Awake()
    {
        BgAudioSource = GetComponent<AudioSource>();
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.PlayClickBgMusic, PlayBgMusic);
     
    }
    void Start()
    {
        bgtime = 30;
        flag = true;
    }

    private void PlayBgMusic()
    {
        StartCoroutine(BgMusic());

    }
    // Update is called once per frame
    IEnumerator BgMusic()
    {
        while (flag)
        {
            bgtime = 30;
            while (bgtime != 0)
            {
                if (bgtime == 30)
                {
                    BgAudioSource.PlayOneShot(vars.bgMusic);
                }
                yield return new WaitForSeconds(1);
                bgtime--;
            }

        }
    }

}
