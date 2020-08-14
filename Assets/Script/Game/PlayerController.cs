using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour {

    public GameObject chart;
    public LayerMask platformLayer, obstacleLayer;
    public Transform rayDown, rayLeft, rayRight;
    private bool isMoveLeft = false;//判断向左还是向右跳
    private bool isJumping = false;//判断是否正在跳跃
    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVars vars;
    private Rigidbody2D my_Body;
    private SpriteRenderer spriteRenderer;

    private AudioSource m_AudioSource;
    private bool isMove = false;//是否真在跳
    private Vector3 liftPosition;
    private float t2=1.0f;

    private static int skin = 0;
    private static bool isTool;
  


    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener<int>(EventDefine.ChangeSkin,ChangeSkin);
        spriteRenderer = GetComponent<SpriteRenderer>();
        my_Body = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.AddListener<int>(EventDefine.ShowLuck, LuckType);
        EventCenter.AddListener<bool>(EventDefine.Mode1, JS);
    

    }
    private void IsMusicOn(bool value)
    {
        print(!value);
        m_AudioSource.mute = !value;
    }
    private void OnDestroy()
    {

        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.RemoveListener<int>(EventDefine.ShowLuck, LuckType);
        EventCenter.RemoveListener<bool>(EventDefine.Mode1, JS);
       

    }
    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击的UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

   
    private void JS(bool flag) 
    {
        isTool = flag;
    
    }

    private void Start() 
    {

        spriteRenderer.sprite = vars.characterSkinSpriteList[skin];
    }

    private void Update () 
    {

        t2 = t2 + Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.A))
        {
           
            if (t2>1.2f)
              {

                int rand = Random.Range(0, 25);
             
                if (GameManager.Instance.GetLuck() != 0)
                {
                    LuckType(rand);
                    t2 = 0f;
                }
                else
                {
                    EventCenter.Broadcast(EventDefine.Hint, "幸运器不足");
                    t2 = 0f;
                }
              }
              else
              {

                EventCenter.Broadcast(EventDefine.Hint, "操作频繁");
              }
        }

       

            if (IsPointerOverGameObject(Input.mousePosition)) return;

        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true|| GameManager.Instance.IsPause==true)//游戏开始或者游戏未结束玩家可移动
            {
           
            return;
            }
            if (Input.GetMouseButtonDown(0) && isJumping == false)
            {
            if (isMove == false)
            {

               
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }
            m_AudioSource.PlayOneShot(vars.jumpClip);
            EventCenter.Broadcast(EventDefine.DecidePath);
                Vector3 mousePos = Input.mousePosition;
                isJumping = true;
                //点击的是左边屏幕
                if (mousePos.x <= Screen.width / 2)
                {
                Debug.Log("左");
                     isMoveLeft = true;
                }
                //点击的右边屏幕
                else if (mousePos.x > Screen.width / 2)
                {
                Debug.Log("右");
                isMoveLeft = false;
                }
                Jump();
            }

        if (my_Body.velocity.y < 0&&IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            
            StartCoroutine(DealyShowGameOverPanel());

        }

        if (isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
           
                m_AudioSource.PlayOneShot(vars.hitClip);
                GameObject go = ObjectPool.Instance.GetDeathEffect();
                go.SetActive(true);
                go.transform.position = transform.position;
                spriteRenderer.enabled = false;
                GameManager.Instance.IsGameOver = true;//游戏结束
               
                StartCoroutine(DealyShowGameOverPanel());
             
              
        }
        if (transform.position.y-Camera.main.transform.position.y< -6&&GameManager.Instance.IsGameOver==false) 
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            GameManager.Instance.IsGameOver = true;//游戏结束
           
            StartCoroutine(DealyShowGameOverPanel());


        }

    }


    private void LuckType(int rand) 
    {
        int rand1 = Random.Range(0, 10);
        EventCenter.Broadcast(EventDefine.ReduceLuck);
        EventCenter.Broadcast(EventDefine.UpdateLuckText);
        if (rand <= 2) 
        {
            if (isTool == false)
            {
                EventCenter.Broadcast(EventDefine.DoubleScore, 50);
                EventCenter.Broadcast(EventDefine.Hint, "本局+50分");
            }
            else EventCenter.Broadcast(EventDefine.Hint, "该模式不支持得分");
        }
      
        if (rand>2&&rand <= 6)
        {
            if (isTool == false)
            {
                EventCenter.Broadcast(EventDefine.DoubleScore, 12);
                EventCenter.Broadcast(EventDefine.Hint, "本局+12分");
            }
            else EventCenter.Broadcast(EventDefine.Hint, "该模式不支持得分");
        }
       
        if (rand== 7)
        {
           
            EventCenter.Broadcast(EventDefine.Hint, "+40钻总数");
            GameManager.Instance.UpdateAllDiamond(40);
        }
        if (rand > 7 && rand <= 12)
        {
           
            EventCenter.Broadcast(EventDefine.Hint, "+20钻总数");
            GameManager.Instance.UpdateAllDiamond(20);

        }
        if (rand > 12 && rand <= 20)
        {
           
            EventCenter.Broadcast(EventDefine.Hint, "幸运将在下次降临");
        }
        if (rand > 20 && rand <= 95)
        {
            if (rand1 == 6)
            {
                if (GameManager.Instance.GetSkinUnlocked(3) == false)
                {
                    EventCenter.Broadcast(EventDefine.Hint, "僵尸解锁");
                    GameManager.Instance.SetSkinUnloacked(3);
                }
                else
                {
                    EventCenter.Broadcast(EventDefine.Hint, "已兑换成100钻石");
                    GameManager.Instance.UpdateAllDiamond(100);

                }
            }
            else 
            {
                EventCenter.Broadcast(EventDefine.Hint, "幸运将在下次降临");

            }

            
        }

    }
    private void StarShowGameOverPanel() {

        StartCoroutine(DealyShowGameOverPanel());

    }
    IEnumerator DealyShowGameOverPanel()
    {
       
            yield return new WaitForSeconds(1f);
            //调用结束面板
            EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
     
        
    }

    //换皮肤
    private void ChangeSkin(int skinIndex)
    {
        skin = skinIndex;
        spriteRenderer.sprite = vars.characterSkinSpriteList[skin];
    }
  
    private GameObject lastHitGo = null;


    //射线检测是否落到了平台上
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null)
        {
            if (lastHitGo != hit.collider.gameObject)
            {
                if (lastHitGo == null)
                {
                    lastHitGo = hit.collider.gameObject;
                    return true;
                }
                EventCenter.Broadcast(EventDefine.AddScore);
                lastHitGo = hit.collider.gameObject;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 是否检测到障碍物
    /// </summary>
    /// <returns></returns>
    /// 
  
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

        if (leftHit.collider != null)
        {
          

            if (leftHit.collider.tag == "Obstacle")
            {
                return true;

            }
        }

        if (rightHit.collider != null)
        {
            return true;

        }
        return false;
    }

    private void Jump()
    {
        if (isJumping)
        {
            if (isMoveLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.DOMoveX(nextPlatformLeft.x, 0.2f);
                transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
                

            }
            else
            {

                transform.DOMoveX(nextPlatformRight.x, 0.2f);
                transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
                transform.localScale = Vector3.one;
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            isJumping = false;//碰到平台跳跃结束
            Vector3 currentPlatformPos = collision.gameObject.transform.position;
          
            nextPlatformLeft = new Vector3(currentPlatformPos.x -vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x +vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "Diamond")
        {
            Debug.Log("吃到了钻石");
            m_AudioSource.PlayOneShot(vars.diamondClip);
            EventCenter.Broadcast(EventDefine.AddDiamond);
            //吃到钻石了
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "DoubleScore")
        {
            m_AudioSource.PlayOneShot(vars.diamondClip);
            EventCenter.Broadcast(EventDefine.DoubleScore,2);
            //吃到钻石了
            collision.gameObject.SetActive(false);
        }
    }
   
}
