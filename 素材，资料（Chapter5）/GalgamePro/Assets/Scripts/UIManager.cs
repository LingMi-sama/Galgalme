using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Image imgBG;
    public Image imgCharacter;
    public Image imgCharacter2;
    public Text textName;
    public Text choiceTalkLineTextName;
    public Text textTalkLine;
    public GameObject talkLineGo;//对话框父对象游戏物体
    public Transform[] characterPosTrans;
    public Text textEnergyValue;
    public Text textFavorValue;
    public GameObject empChoiceUIGo;//多项选择框父对象
    public GameObject[] choiceUIGos;
    public Text[] textChoiceUIs;
    private bool toNextScene;//是否开始遮罩动画
    public Image mask;//全屏遮罩
    private List<UIInfo> imageTweenList;//目前需要做UI动画渐隐渐现的图片对象
    public GameObject bagPanelGo;
    public bool openBag;
    public GameObject imgFavorability;
    private List<ItemInfo> itemInfos;
    public Transform imgBagBackground;
    private Dictionary<int, GameObject> itemBtnGosDict;
    public GameObject tipInfoGo;
    public Text textTipInfo;
    public GameObject choiceLineGo;//对话框中的多项选择内容
    public GameObject[] choiceLineUIGos;
    public Text[] textChoiceLineUI;
    public Button clickEventButton;
    public Button loadNextButton;

    private void Awake()
    {
        Instance = this;
        imageTweenList = new List<UIInfo>();
        //itemInfos = new List<ItemInfo>()
        //{
        //    new ItemInfo(){name="老冰棍",id=1,instruction="吃了它，可以恢复体力哦！",src=10 },
        //    new ItemInfo(){name="小熊玩偶",id=2,instruction="一只卡哇伊的小熊布偶，可以赠送给喜欢的人呦",src=10 }
        //};
       
    }

    private void Start()
    {
        //GameManager.Instance.SaveByJson<List<ItemInfo>>(itemInfos, "/Json/ItemInfo.json");
        itemInfos = GameManager.Instance.LoadByJson<List<ItemInfo>>("/Json/ItemInfo.json");
        itemBtnGosDict = new Dictionary<int, GameObject>();
        for (int i = 0; i < itemInfos.Count; i++)
        {
            AddItem(itemInfos[i]);
        }
    }

    /// <summary>
    /// 加载并添加物品到背包（包括物品数据添加到物品数据存贮字典）
    /// </summary>
    /// <param name="itemInfo"></param>
    public void AddItem(ItemInfo itemInfo)
    {
        Button btnItem= Instantiate(Resources.Load<GameObject>("Prefabs/Items/Btn_Item")).GetComponent<Button>();
        btnItem.transform.SetParent(imgBagBackground);
        btnItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Items/"+itemInfo.id);
        btnItem.GetComponent<ItemUI>().itemInfo = itemInfo;
        if (!itemBtnGosDict.ContainsKey(itemInfo.id))
        {
            itemBtnGosDict.Add(itemInfo.id, btnItem.gameObject);
        }        
    }
    /// <summary>
    /// 从物品数据存贮字典中移除物品(包括数据)
    /// </summary>
    /// <param name="id"></param>
    public void RemoveItem(int id)
    {
        if (itemBtnGosDict.ContainsKey(id))
        {
            Destroy(itemBtnGosDict[id]);
            itemBtnGosDict.Remove(id);
        }
    }

    /// <summary>
    /// 设置背景图片
    /// </summary>
    /// <param name="spriteName"></param>
    public void SetBGImageSprite(string spriteName)
    {
        imgBG.sprite = Resources.Load<Sprite>("Sprites/BG/"+spriteName);
    }
    /// <summary>
    /// 更新对话内容
    /// </summary>
    /// <param name="dialogueContent"></param>
    public void UpdateTalkLineText(string dialogueContent,bool loadNext=true)
    {
        CloseChoiceLineUI();
        CloseChoiceUI();
        ShowOrHideTalkLine();
        textTalkLine.text = dialogueContent;
        ShowEventButton(loadNext); 
    }

    public void SetCharacterPos(int posID,bool ifRotate=false,int characterID=0)
    {
        if (characterID==0)
        {
            SetPos(posID,imgCharacter,ifRotate);
        }
        else
        {
            SetPos(posID, imgCharacter2, ifRotate);
        }
    }
    public void SetPos(int posID, Image imgTargetCharacter, bool ifRotate = false)
    {
        imgTargetCharacter.transform.localPosition = characterPosTrans[posID - 1].localPosition;
        if (ifRotate)
        {
            imgTargetCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            imgTargetCharacter.transform.eulerAngles = Vector3.zero;
        }
    }

    /// <summary>
    /// 更新精力值UI
    /// </summary>
    public void UpdateEnergyValue(int value = 0)
    {
        textEnergyValue.text = value.ToString();
    }
    /// <summary>
    /// 改变好感度UI
    /// </summary>
    public void UpdateFavorValue(int value = 0, string name = null)
    {
        textFavorValue.text = value.ToString();
    }
    /// <summary>
    /// 显示多项选择对话框
    /// </summary>
    /// <param name="choiceNum">选择数量</param>
    /// <param name="choiceContent">选项框的文本</param>
    public void ShowChoiceUI(int choiceNum,string[] choiceContent,UnityEngine.Events.UnityAction<object> unityAction)
    {
        SetChoiceUI(empChoiceUIGo,choiceUIGos,textChoiceUIs,choiceNum,choiceContent,unityAction);
    }
    /// <summary>
    /// 关闭多选选择框
    /// </summary>
    public void CloseChoiceUI()
    {
        empChoiceUIGo.SetActive(false);
    }
    /// <summary>
    /// 显示或隐藏对话框
    /// </summary>
    /// <param name="show">默认是显示(true)</param>
    public void ShowOrHideTalkLine(bool show=true)
    {
        if (show)
        {
            CloseChoiceUI();
        }
        talkLineGo.SetActive(show);
    }
    /// <summary>
    /// 显示或隐藏遮罩
    /// </summary>
    /// <param name="show">true表示显示</param>
    public void ShowOrHideMask(bool show)
    {
        DoShowOrHideUITween(show,true,2,mask);
    }
    /// <summary>
    /// 显示或隐藏人物
    /// </summary>
    /// <param name="show"></param>
    /// <param name="characterID"></param>
    public void ShowOrHideCharacter(bool show,int characterID)
    {       
        if (characterID==0)
        {
            DoShowOrHideUITween(show,true, 1.5f,imgCharacter);
        }
        else
        {
            DoShowOrHideUITween(show, true, 1.5f,imgCharacter2);
        }
    }

    /// <summary>
    /// 执行UI渐隐渐现的动画
    /// </summary>
    /// <param name="show"></param>
    /// <param name="image"></param>
    public void DoShowOrHideUITween(bool show,bool ifLoadNext,float interval,params Image[] images)
    {
        toNextScene = true;
        float percent;
        if (show)
        {
            percent = 0;
        }
        else
        {
            percent = 1;
        }
        for (int i = 0; i < images.Length; i++)
        {
            imageTweenList.Add(new UIInfo() { show = show, imageTween = images[i], ifLoadNext = ifLoadNext,lerpSpeed=1/interval,percent=percent });
        }      
    }

    private void ShowUI(UIInfo info)
    {
        info.percent += info.lerpSpeed * Time.deltaTime;
        info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b
            , info.percent);
        if (info.imageTween.color.a>=0.995f)
        {
            info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b
            , 1);
            if (info.ifLoadNext)
            {
                GameManager.Instance.LoadNextScript();
            }
            imageTweenList.Remove(info);
            if (imageTweenList.Count <= 0)
            {
                toNextScene = false;
            }
        }
    }
    private void HideUI(UIInfo info)
    {
        info.percent -= info.lerpSpeed * Time.deltaTime;
        info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b
            , info.percent);
        if (info.imageTween.color.a <= 0.005f)
        {
            info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b
            , 0);
            if (info.ifLoadNext)
            {
                GameManager.Instance.LoadNextScript();
            }
            imageTweenList.Remove(info);
            if (imageTweenList.Count<=0)
            {
                toNextScene = false;
            }
        }
    }
    private void Update()
    {
        //开始遮罩动画
        if (toNextScene)
        {
            for (int i = 0; i < imageTweenList.Count; i++)
            {
                if (imageTweenList[i].show)
                {
                    ShowUI(imageTweenList[i]);
                }
                else
                {
                    HideUI(imageTweenList[i]);
                }
            }
        }
    }
    /// <summary>
    /// 打开或关闭背包
    /// </summary>
    public void OpenOrCloseBag()
    {
        openBag = !openBag;
        bagPanelGo.SetActive(openBag);
    }
    /// <summary>
    /// 更新角色名称
    /// </summary>
    /// <param name="name"></param>
    public void UpdateCharacterName(string name)
    {
        textName.text = name;
        choiceTalkLineTextName.text = name;
        ShowOrHideFavorUI(true);
    }
    /// <summary>
    /// 是否显示好感度
    /// </summary>
    /// <param name="show"></param>
    private void ShowOrHideFavorUI(bool show)
    {
        imgFavorability.SetActive(show);
    }
    /// <summary>
    /// 显示物品信息说明框
    /// </summary>
    /// <param name="itemInfo">物品信息</param>
    /// <param name="itemPos">显示位置</param>
    public void ShowTipInfo(ItemInfo itemInfo,Vector3 itemPos)
    {
        tipInfoGo.SetActive(true);
        tipInfoGo.transform.position = itemPos;
        textTipInfo.text = "<color=#FFE7D6>" + itemInfo.name+"</color>" + "\n"+ "<color=#F7F8D6>" + itemInfo.instruction+"</color>";
    }
    public void HideTipInfo()
    {
        tipInfoGo.SetActive(false);
    }
    /// <summary>
    /// 显示对话框中的多项选择
    /// </summary>
    public void ShowChioceLineUI(int choiceNum, string[] choiceContent, UnityEngine.Events.UnityAction<object> unityAction)
    {
        SetChoiceUI(choiceLineGo,choiceLineUIGos,textChoiceLineUI,choiceNum,choiceContent,unityAction);
    }
    private void CloseChoiceLineUI()
    {
        choiceLineGo.SetActive(false);
    }
    /// <summary>
    /// 设置选择类型UI的方法
    /// </summary>
    /// <param name="choiceUIGo">选择UI的父对象游戏物体</param>
    /// <param name="choiceUIGos">选择UI的所有子游戏物体</param>
    /// <param name="choiceUITexts">选择UI文本引用</param>
    /// <param name="choiceNum">有几个选项</param>
    /// <param name="choiceContent">选择按钮上的文本内容</param>
    /// <param name="unityAction">点击按钮后触发的回调事件</param>
    public void SetChoiceUI(GameObject choiceUIGo,GameObject[] choiceUIGos,Text[] choiceUITexts,int choiceNum,string[] choiceContent,UnityEngine.Events.UnityAction<object> unityAction)
    {
        ShowOrHideTalkLine(false);
        CloseChoiceUI();
        CloseChoiceLineUI();
        choiceUIGo.SetActive(true);       
        for (int i = 0; i < choiceUIGos.Length; i++)
        {
            choiceUIGos[i].SetActive(false);
            choiceUIGos[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i = 0; i < choiceNum; i++)
        {
            choiceUIGos[i].SetActive(true);
            choiceUITexts[i].text = choiceContent[i];
            int src = i;
            choiceUIGos[i].GetComponent<Button>().onClick.AddListener(() => { unityAction(src + 1); });
        }
    }
    /// <summary>
    /// 对话框选择按钮触发事件注册
    /// </summary>
    /// <param name="unityAction"></param>
    public void AddClickButtonListener(UnityEngine.Events.UnityAction<object> unityAction)
    {
        clickEventButton.onClick.RemoveAllListeners();
        clickEventButton.onClick.AddListener(()=> { unityAction(null); });
    }
    /// <summary>
    /// 显示加载下一个剧本的按钮还是其他回调事件按钮
    /// </summary>
    public void ShowEventButton(bool loadNext=true)
    {
        if (loadNext)
        {
            loadNextButton.gameObject.SetActive(true);
            clickEventButton.gameObject.SetActive(false);
        }
        else
        {
            loadNextButton.gameObject.SetActive(false);
            clickEventButton.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 是否拥有某样物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IfOwnItem(int id)
    {
        return itemBtnGosDict.ContainsKey(id);
    }
}

public class UIInfo
{
    public bool show;
    public Image imageTween;
    public bool ifLoadNext;
    public float percent;
    public float lerpSpeed;
}

public struct ItemInfo
{
    public string name;
    public int id;
    public string instruction;
    public object src;
}
