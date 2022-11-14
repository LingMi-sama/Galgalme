using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private List<ScriptData> scriptDatas;
    private int scriptIndex;

    private void Awake()
    {
        Instance = this;
        scriptDatas = new List<ScriptData>()
        {
            new ScriptData()
            { 
                loadType=1,spriteName="Title"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="你好，我叫Test"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="你真厉害!这么快就学会了galgame实现思路的核心内容"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="Test真的好崇拜你哦！"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="不过我们的挑战才刚刚开始！"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="接下来我们要增加难度了哦"
            },
            new ScriptData()
            {
                loadType=2,name="Test",dialogueContent="加油~"
            }
        };
        scriptIndex = 0;
        HandleData();
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
        if (scriptDatas[scriptIndex].loadType==1)
        {
            //背景
            //设置一下背景图片
            SetBGImageSprite(scriptDatas[scriptIndex].spriteName);
            //加载下一条剧情数据
            LoadNextScript();
        }
        else
        {
            //人物
            //显示人物
            ShowCharacter(scriptDatas[scriptIndex].name);
            //更新对话框文本
            UpdateTalkLineText(scriptDatas[scriptIndex].dialogueContent);
        }
    }
    //设置一下背景图片
    private void SetBGImageSprite(string spriteName)
    {
        UIManager.Instance.SetBGImageSprite(spriteName);
    }
    //加载下一条剧情数据
    public void LoadNextScript()
    {
        Debug.Log("加载下一条剧情");
        scriptIndex++;
        HandleData();
    }
    //显示人物
    private void ShowCharacter(string name)
    {
        UIManager.Instance.ShowCharacter(name);
    }
    //更新对话框文本
    private void UpdateTalkLineText(string dialogueContent)
    {
        UIManager.Instance.UpdateTalkLineText(dialogueContent);
    }
}

/// <summary>
/// 剧本数据
/// </summary>
public class ScriptData
{
    public int loadType;//载入资源类型 1.背景 2.人物
    public string name;//角色名称
    public string spriteName;//图片资源路径
    public string dialogueContent;//对话内容
}
