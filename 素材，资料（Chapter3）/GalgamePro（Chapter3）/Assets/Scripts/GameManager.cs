using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private List<ScriptData> scriptDatas;
    private int scriptIndex;
    public int energyValue;//目前角色的精力值状态
    public Dictionary<string, int> favorabilityDict;//每个角色对玩家的好感度
    public Dictionary<int, Action<object>> eventDict;
    public GameObject hitPointGo;
    private void Awake()
    {
        Instance = this;
        scriptDatas = new List<ScriptData>()
        {
            new ScriptData()
            { 
                loadType=1,spriteName="Title",soundType=3,soundPath="Daily"
            },
            new ScriptData()
            {
                loadType=3,eventID=3
            },
            new ScriptData()//Test进场
            {
                loadType=3,eventID=5,name="Test",characterPos=2,eventData=1
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="你好，我叫Test",characterPos=2,soundType=1,soundPath="0",energyValue=10
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="你真厉害!这么快就学会了galgame实现思路的核心内容",characterPos=2,soundType=1,soundPath="1"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="Test真的好崇拜你哦！",characterPos=2,soundType=1,soundPath="2",favorability=5
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="不过我们的挑战才刚刚开始！",characterPos=1,ifRotate=true,soundType=1,soundPath="3"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="接下来我们要增加难度了哦",characterPos=1,ifRotate=true,soundType=1,soundPath="4"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="加油~",characterPos=3,soundType=1,soundPath="5",energyValue=-10
            },
            new ScriptData()
            {
                soundType=3,soundPath="Normal"
            },
            new ScriptData()//Debug进场
            {
                loadType=3,eventID=5,name="Debug",characterPos=1,eventData=1,characterID=1,ifRotate=true
            },
            new ScriptData()
            {
                loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="0",ifRotate=true,characterID=1
                ,dialogueContent="你好，在下名为Debug，看来阁下已经学会了很多拓展内容了"
            },
            new ScriptData()
            {
                loadType=2,name="Test",characterPos=2,soundType=1,soundPath="6"
                ,dialogueContent="呦，Debug来了呀，你也想试试他的能力吗？但是他很厉害的哦，我感觉Debug你不一定可以赢过他，那么你们接下来比什么？"
            },
            new ScriptData()
            {
                loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="1",ifRotate=true,characterID=1
                ,dialogueContent="在下想与阁下击剑，不知阁下意下如何？",energyValue=-50
            },
            new ScriptData()
            { 
                loadType=3,eventID=1,eventData=3,scriptID=1
            },
            new ScriptData()
            {
                loadType=3,eventID=2,eventData=2,dialogueContent="击。。。击剑？不太合适吧。。。"
            },
            new ScriptData()
            {
                loadType=3,eventID=2,eventData=3,dialogueContent="好的，来吧！但是我没有剑"
            },
            new ScriptData()
            {
                loadType=3,eventID=2,eventData=4,dialogueContent="我准备好了！"
            },
            new ScriptData()
            {
                loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="2",ifRotate=true,characterID=1
                ,dialogueContent="为什么阁下露出了娇羞的表情，似乎在期待什么",scriptID=2
            },
            new ScriptData()
            {
                loadType=3,eventID=2,eventData=1
            },
            new ScriptData()
            {
                loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="3",ifRotate=true,characterID=1
                ,dialogueContent="阁下放心，剑阁里有的，请随在下来",scriptID=3
            },
            new ScriptData()
            {
                loadType=3,eventID=2,eventData=1
            },
            new ScriptData()//Test退场
            {
                loadType=3,eventID=5,name="Test",characterPos=2,scriptID=4
            },
            new ScriptData()
            {
                loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="4",ifRotate=true,characterID=1
                ,dialogueContent="那么我们要开始了，我身上会随机出现破绽，阁下朝破绽上攻击就可以了，不需要考虑是否会伤到我，我能防好"
            },
            new ScriptData()
            {
                loadType=3,eventID=4,eventData=1
            },
            new ScriptData()//失败时需要跳转的剧情位置
            {
                loadType=3,eventID=2,eventData=5
            },
            new ScriptData()//胜利时需要跳转的剧情位置
            {
                loadType=3,eventID=2,eventData=6
            },
            new ScriptData()
            {
                 loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="5",ifRotate=true,characterID=1
                ,dialogueContent="阁下的剑术不是很精进，还需要多加努力",scriptID=5
            },
            new ScriptData()
            {
                loadType=3,eventID=2,eventData=7
            },
            new ScriptData()
            {
                 loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="6",ifRotate=true,characterID=1
                ,dialogueContent="阁下的剑术果然厉害！",scriptID=6
            },
            new ScriptData()
            {
                loadType=3,eventID=2,eventData=7
            },
            new ScriptData()
            {
                 loadType=2,name="Debug",characterPos=1,soundType=1,soundPath="7",ifRotate=true,characterID=1
                ,dialogueContent="那么在下告退，期待下次与阁下见面",scriptID=7
            },
            new ScriptData()//Debug退场
            {
                loadType=3,eventID=5,name="Debug",characterPos=1,ifRotate=true,characterID=1
            },
            new ScriptData()
            {
                 loadType=3,eventID=3,eventData=1
            }
        };
        scriptIndex = 0;
        HandleData();
        energyValue = 100;
        ChangeEnergyValue();
        favorabilityDict = new Dictionary<string, int>()
        {
            {"Player",0},
            { "Test",80},
            { "Debug",60}
        };
        eventDict = new Dictionary<int, Action<object>>()
        {
            { 1,StartHitPointEvent}
        };
        for (int i = 0; i < scriptDatas.Count; i++)
        {
            scriptDatas[i].scriptIndex = i;
        }
    }
    /// <summary>
    /// 处理每一条剧情数据
    /// </summary>
    private void HandleData()
    {
        if (scriptIndex>=scriptDatas.Count)
        {
            Debug.Log("游戏结束");
            return;
        }
        PlaySound(scriptDatas[scriptIndex].soundType);
        if (scriptDatas[scriptIndex].loadType==1)
        {
            //背景
            //设置一下背景图片
            SetBGImageSprite(scriptDatas[scriptIndex].spriteName);
            //加载下一条剧情数据
            LoadNextScript();
        }
        else if(scriptDatas[scriptIndex].loadType == 2)
        {
            //人物
            HandleCharacter();
        }
        else if (scriptDatas[scriptIndex].loadType == 3)
        {
            //事件
            switch (scriptDatas[scriptIndex].eventID)
            {
                //显示选择项
                case 1:
                    ShowChoiceUI(scriptDatas[scriptIndex].eventData,GetChoiceContent
                        (scriptDatas[scriptIndex].eventData));
                    break;
                //跳转到标记位置剧本
                case 2:
                    SetScriptIndex();
                    break;
                case 3:
                    ShowOrHideMask(System.Convert.ToBoolean(scriptDatas[scriptIndex].eventData));
                    break;
                case 4:
                    eventDict[scriptDatas[scriptIndex].eventData](null);
                    break;
                case 5:
                    ShowOrHideCharacter(System.Convert.ToBoolean(scriptDatas[scriptIndex].eventData)
                        ,scriptDatas[scriptIndex].characterID);
                    HandleCharacter(true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            LoadNextScript();
        }
    }
    //设置一下背景图片
    private void SetBGImageSprite(string spriteName)
    {
        UIManager.Instance.SetBGImageSprite(spriteName);
    }
    //加载下一条剧情数据
    public void LoadNextScript(int addNum=1)
    {
        //Debug.Log("加载下一条剧情");
        scriptIndex+=addNum;
        HandleData();
    }
    //显示人物
    private void ShowCharacter(string name,int characterID=0)
    {
        UIManager.Instance.ShowCharacter(name, characterID);
    }
    //更新对话框文本
    private void UpdateTalkLineText(string dialogueContent)
    {
        UIManager.Instance.UpdateTalkLineText(dialogueContent);
    }
    //设置人物方法
    public void SetCharacterPos(int posID, bool ifRotate = false,int characterID=0)
    {
        UIManager.Instance.SetCharacterPos(posID,ifRotate,characterID);
    }
    //播放游戏音效
    public void PlaySound(int soundType)
    {
        switch (soundType)
        {
            case 1:
                AudioSourceManager.Instance.PlayDialogue(
                    scriptDatas[scriptIndex].name+"/"+scriptDatas[scriptIndex].soundPath);
                break;
            case 2:
                AudioSourceManager.Instance.PlaySound(
                    scriptDatas[scriptIndex].soundPath);
                break;
            case 3:
                AudioSourceManager.Instance.PlayMusic(
                    scriptDatas[scriptIndex].soundPath);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 改变精力值
    /// </summary>
    /// <param name="value">需要具体改变的值（不是改变后的值）</param>
    public void ChangeEnergyValue(int value=0)
    {     
        if (value==0)
        {
            UpdateEnergyValue(energyValue);
            return;
        }
        if (value>0)
        {
            AudioSourceManager.Instance.PlaySound("Energy");
        }
        energyValue += value;
        if (energyValue>=100)
        {
            energyValue = 100;
        }
        else if (energyValue<=0)
        {
            energyValue = 0;
        }
        UpdateEnergyValue(energyValue);
    }
    /// <summary>
    /// 更新精力值UI
    /// </summary>
    public void UpdateEnergyValue(int value = 0)
    {
        UIManager.Instance.UpdateEnergyValue(value);
    }
    /// <summary>
    /// 改变好感度
    /// </summary>
    /// <param name="value"></param>
    public void ChangeFavorValue(int value = 0,string name=null)
    {
        if (value == 0)
        {
            return;
        }
        if (value > 0)
        {
            AudioSourceManager.Instance.PlaySound("Favor");
        }
        favorabilityDict[name] += value;
        if (favorabilityDict[name] >= 100)
        {
            favorabilityDict[name] = 100;
        }
        else if (favorabilityDict[name] <= 0)
        {
            favorabilityDict[name] = 0;
        }
        UpdateFavorValue(favorabilityDict[name],name);
    }
    /// <summary>
    /// 改变好感度UI
    /// </summary>
    public void UpdateFavorValue(int value = 0,string name=null)
    {
        UIManager.Instance.UpdateFavorValue(value,name);
    }
    /// <summary>
    /// 处理跟人物角色相关的内容
    /// </summary>
    public void HandleCharacter(bool showCharacterOnly=false)
    {
        //显示人物
        ShowCharacter(scriptDatas[scriptIndex].name,scriptDatas[scriptIndex].characterID);
        //设置人物位置
        SetCharacterPos(scriptDatas[scriptIndex].characterPos, scriptDatas[scriptIndex].ifRotate, scriptDatas[scriptIndex].characterID);
        if (!showCharacterOnly)
        {
            //更新对话框文本
            UpdateTalkLineText(scriptDatas[scriptIndex].dialogueContent);
            //更新好感度和精力值
            ChangeEnergyValue(scriptDatas[scriptIndex].energyValue);
            ChangeFavorValue(scriptDatas[scriptIndex].favorability, scriptDatas[scriptIndex].name);
        }        
    }
    /// <summary>
    /// 显示多项选择对话框
    /// </summary>
    /// <param name="choiceNum"></param>
    /// <param name="choiceContent"></param>
    public void ShowChoiceUI(int choiceNum,string[] choiceContent)
    {
        UIManager.Instance.ShowChoiceUI(choiceNum,choiceContent);
    }
    /// <summary>
    /// 获取当前选择项上的文本
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public string[] GetChoiceContent(int num)
    {
        string[] choiceContent = new string[num];
        for (int i = 0; i < num; i++)
        {
            choiceContent[i] = scriptDatas[scriptIndex+1+i].dialogueContent;
        }
        return choiceContent;
    }
    /// <summary>
    /// 设置剧本索引
    /// </summary>
    /// <param name="index"></param>
    public void SetScriptIndex(int index=0)
    {
        for (int i = 0; i < scriptDatas.Count; i++)
        {
            if (scriptDatas[scriptIndex+ index].eventData==scriptDatas[i].scriptID)
            {             
                scriptIndex = scriptDatas[i].scriptIndex;
                break;
            }
        }
        HandleData();
    }
    /// <summary>
    /// 击剑
    /// </summary>
    /// <param name="src"></param>
    public void StartHitPointEvent(object src)
    {
        UIManager.Instance.ShowOrHideTalkLine(false);
        hitPointGo.SetActive(true);
    }
    /// <summary>
    /// 显示或隐藏遮罩
    /// </summary>
    /// <param name="show">true表示显示</param>
    public void ShowOrHideMask(bool show)
    {
        UIManager.Instance.ShowOrHideMask(show);
    }
    /// <summary>
    /// 显示或隐藏人物
    /// </summary>
    /// <param name="show"></param>
    /// <param name="characterID"></param>
    public void ShowOrHideCharacter(bool show, int characterID)
    {
        UIManager.Instance.ShowOrHideCharacter(show,characterID);
    }
    /// <summary>
    /// 执行UI动画的渐隐渐现
    /// </summary>
    /// <param name="show"></param>
    /// <param name="image"></param>
    public void DoShowOrHideUITween(bool show,bool ifLoadNext,float interval,params UnityEngine.UI.Image[] images)
    {
        UIManager.Instance.DoShowOrHideUITween(show,ifLoadNext,interval,images);
    }
}


