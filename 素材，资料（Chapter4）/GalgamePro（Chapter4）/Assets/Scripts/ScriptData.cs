using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剧本数据
/// </summary>
public class ScriptData
{
    public int loadType;//载入资源类型 1.背景 2.人物 3.事件
    public string name;//角色名称
    public string spriteName;//图片资源路径
    public string dialogueContent;//对话内容
    public int characterPos;//1.左 2.右 3.中
    public bool ifRotate;
    public int soundType;//音频类型 1.对话 2.音效 3.音乐
    public string soundPath;//音频路径
    public int favorability;//好感度(是改变值不是当前值)
    public int energyValue;//精力值(是改变值不是当前值)
    public int characterID;//三人对话时的人物ID
    //处理事件的ID 
    //1.显示选择项 2.跳转到指定剧本位置 3.显示隐藏遮罩 4.特殊事情 5.显示隐藏人物
    public int eventID;
    //事件数据
    //1.几个选项 2.具体要跳转到的标记位 3.0隐藏1显示 4.事件ID 5.0退场1进场
    public int eventData;
    //剧本标记位，用于跳转
    public int scriptID;
    //剧本索引
    public int scriptIndex;
    //当前人物需要播放的动作编号
    public int animationNum;
    //当前人物需要播放的表情标号
    public int expressionIndex;
}
