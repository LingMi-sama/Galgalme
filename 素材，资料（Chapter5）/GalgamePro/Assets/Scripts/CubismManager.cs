using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubismManager : MonoBehaviour
{
    public static CubismManager Instance { get; private set; }
    private Dictionary<string, CubismObject> characterDict;
    public Transform[] characterPos;
    private List<CharacterInfo> characterTweenList;
    private bool startShowCharacterTween;
    private Dictionary<string, CubismObject> currentCharacterDict;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        characterDict = new Dictionary<string, CubismObject>();
        characterTweenList = new List<CharacterInfo>();
        currentCharacterDict = new Dictionary<string, CubismObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startShowCharacterTween)
        {
            for (int i = 0; i < characterTweenList.Count; i++)
            {
                if (characterTweenList[i].show)
                {
                    ShowCharacter(characterTweenList[i]);
                }
                else
                {
                    HideCharacter(characterTweenList[i]);
                }
            }
        }
    }
    /// <summary>
    /// 显示人物（加载人物模型）
    /// </summary>
    public void ShowCharacter(string name)
    {
        if (name==null||name=="")
        {
            return;
        }
        if (characterDict.ContainsKey(name))
        {
            characterDict[name].gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(name);
            characterDict.Add(name, Instantiate(Resources.Load<GameObject>
                ("Prefabs/Characters/" + name)).transform.GetComponent<CubismObject>());            
        }
    }

    /// <summary>
    /// 设置人物位置
    /// </summary>
    /// <param name="posID">位置ID</param>
    /// <param name="name"></param>
    public void SetCharacterPos(int posID,string name)
    {
        if (name == null || name == "")
        {
            return;
        }
        if (characterDict.ContainsKey(name))
        {
            characterDict[name].transform.SetParent(characterPos[posID-1]);
            characterDict[name].transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// 人物进场退场动画(外部调用方法)
    /// </summary>
    public void DoShowOrHideCharacterTween(bool show,bool ifLoadNext,float interval,string name, int posID)
    {
        StartCoroutine(DoShowOrHideCharacter(show,ifLoadNext,interval,name,posID));
    }
    private IEnumerator DoShowOrHideCharacter(bool show, bool ifLoadNext, float interval, string name,int posID)
    {
        if (name == null || name == "")
        {
            yield break;
        }
        CubismObject cubismObject;
        if (characterDict.ContainsKey(name))
        {
            cubismObject = characterDict[name];
        }
        else
        {
            cubismObject = Instantiate(Resources.Load<GameObject>("Prefabs/Characters/" + name)).GetComponent<CubismObject>();
            characterDict.Add(name, cubismObject);
        }
        if (show)
        {
            //设置到屏幕外
            SetCharacterPos(4, name);
        }        
        yield return new WaitForSeconds(0.1f);
        //当前剧本需要人物显示到的位置
        SetCharacterPos(posID, name);
        float percent;
        cubismObject.EnableAnimator(false);
        if (show)
        {
            percent = 0;
            cubismObject.SetOpacityValue(percent);   
            currentCharacterDict.Add(name, cubismObject);
        }
        else
        {            
            percent = 1;
            currentCharacterDict.Remove(name);
        }
        characterTweenList.Add(new CharacterInfo()
        {
            show = show,
            ifLoadNext = ifLoadNext,
            percent = percent
        ,
            lerpSpeed = 1 / interval,
            cubismObject = cubismObject
        });
        startShowCharacterTween = true;
    }

    /// <summary>
    /// 人物进场
    /// </summary>
    /// <param name="characterInfo"></param>
    private void ShowCharacter(CharacterInfo characterInfo)
    {
        characterInfo.percent += characterInfo.lerpSpeed * Time.deltaTime;
        characterInfo.cubismObject.SetOpacityValue(characterInfo.percent);
        if (characterInfo.cubismObject.GetOpacityValue()>=0.995f)
        {
            characterInfo.cubismObject.SetOpacityValue(1);
            characterInfo.cubismObject.EnableAnimator(true);
            if (characterInfo.ifLoadNext)
            {
                GameManager.Instance.LoadNextScript();
            }
            characterTweenList.Remove(characterInfo);
            if (characterTweenList.Count<=0)
            {
                startShowCharacterTween = false;
            }
        }
    }
    /// <summary>
    /// 人物退场
    /// </summary>
    /// <param name="characterInfo"></param>
    private void HideCharacter(CharacterInfo characterInfo)
    {
        characterInfo.percent -= characterInfo.lerpSpeed * Time.deltaTime;
        characterInfo.cubismObject.SetOpacityValue(characterInfo.percent);
        if (characterInfo.cubismObject.GetOpacityValue() <=0.005f)
        {
            characterInfo.cubismObject.SetOpacityValue(0);            
            if (characterInfo.ifLoadNext)
            {
                GameManager.Instance.LoadNextScript();
            }
            characterTweenList.Remove(characterInfo);
            if (characterTweenList.Count <= 0)
            {
                startShowCharacterTween = false;
            }
        }
    }
    /// <summary>
    /// 说话（唇形同步）
    /// </summary>
    /// <param name="name"></param>
    public void Talk(string name)
    {
        if (!currentCharacterDict.ContainsKey(name))
        {
            return;
        }
        foreach (var item in currentCharacterDict)
        {
            item.Value.Talk(false);
        }
        currentCharacterDict[name].Talk(true);
    }
    /// <summary>
    /// 播放动作
    /// </summary>
    /// <param name="name"></param>
    /// <param name="animationNum"></param>
    public void PlayMotion(string name,int animationNum)
    {
        foreach (var item in currentCharacterDict)
        {
            if (item.Key!=name)
            {
                item.Value.PlayMotion(0);
            }
            else
            {
                currentCharacterDict[name].PlayMotion(animationNum);
            }
        }       
    }
    /// <summary>
    /// 播放表情
    /// </summary>
    /// <param name="name"></param>
    /// <param name="index"></param>
    public void PlayExpression(string name,int index)
    {
        currentCharacterDict[name].PlayExpression(index);
    }
}

public class CharacterInfo
{
    public bool show;
    public CubismObject cubismObject;
    public bool ifLoadNext;
    public float percent;
    public float lerpSpeed;
}
