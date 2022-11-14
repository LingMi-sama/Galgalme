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
    public Text textTalkLine;
    public GameObject talkLineGo;//对话框父对象游戏物体
    public Transform[] characterPosTrans;
    public Text textEnergyValue;
    public Text textFavorValue;
    public GameObject empChoiceUIGo;//多项选择框父对象
    public GameObject[] choiceUIGos;
    public Text[] textChoiceUIs;
    private bool toNextScene;//是否开始遮罩动画
    //private bool showMask;//显示或者隐藏遮罩
    public Image mask;//全屏遮罩
    //private Image imageTween;
    private List<UIInofo> imageTweenList;//目前需要做UI动画渐隐渐现的图片对象

    private void Awake()
    {
        Instance = this;
        imageTweenList = new List<UIInofo>();
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
    /// 显示人物
    /// </summary>
    /// <param name="name"></param>
    public void ShowCharacter(string name,int characterID=0)
    {
        CloseChoiceUI();
        textName.text = name;
        ShowOrHideTalkLine(false);
        if (characterID==0)
        {
            imgCharacter.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
            imgCharacter.SetNativeSize();
            imgCharacter.gameObject.SetActive(true);
        }
        else
        {
            imgCharacter2.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
            imgCharacter2.SetNativeSize();
            imgCharacter2.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 更新对话内容
    /// </summary>
    /// <param name="dialogueContent"></param>
    public void UpdateTalkLineText(string dialogueContent)
    {
        ShowOrHideTalkLine();
        textTalkLine.text = dialogueContent;
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
    public void ShowChoiceUI(int choiceNum,string[] choiceContent)
    {
        empChoiceUIGo.SetActive(true);
        ShowOrHideTalkLine(false);
        for (int i = 0; i < choiceUIGos.Length; i++)
        {
            choiceUIGos[i].SetActive(false);
        }
        for (int i = 0; i < choiceNum; i++)
        {
            choiceUIGos[i].SetActive(true);
            textChoiceUIs[i].text = choiceContent[i];
        }
    }
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
            imageTweenList.Add(new UIInofo() { show = show, imageTween = images[i], ifLoadNext = ifLoadNext,lerpSpeed=1/interval,percent=percent });
        }      
    }

    private void ShowUI(UIInofo info)
    {
        info.percent += info.lerpSpeed * Time.deltaTime;
        info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b
            , info.percent);
        if (info.imageTween.color.a>=0.95f)
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
    private void HideUI(UIInofo info)
    {
        info.percent -= info.lerpSpeed * Time.deltaTime;
        info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b
            , info.percent);
        if (info.imageTween.color.a <= 0.05f)
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
}

public class UIInofo
{
    public bool show;
    public Image imageTween;
    public bool ifLoadNext;
    public float percent;
    public float lerpSpeed;
}
