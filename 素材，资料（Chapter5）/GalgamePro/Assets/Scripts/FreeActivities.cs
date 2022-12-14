using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeActivities : MonoBehaviour
{
    private Dictionary<int, System.Action> chooseActions;
    private Dictionary<int, System.Action> actActions;
    private Dictionary<string, string[]> characterDialogue;
    private string currentCharacterName;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        ShowChoiceUI();
        chooseActions = new Dictionary<int, System.Action>()
        {
            { 1,ChooseTest},
            { 2,ChooseDebug},
            { 3,FinishActivity}
        };
        actActions = new Dictionary<int, System.Action>()
        {
            { 1,Talk},
            { 2,GiveGifts},
            { 3,Date},
            { 4,ReturnToChoice}
        };
        //string[] testStrs = new string[]
        //        {
        //            "你来找我有什么事吗？",
        //            "你真的是一个很聪明的人，期待你的成功！",
        //            "哇！谢谢你的礼物，我很开心！",
        //            "真。。。真的吗？",
        //            "你根本没有礼物嘛"
        //        };
        //string[] debugStrs = new string[]
        //        {
        //            "阁下来找我有什么事吗",
        //            "阁下的击剑技术真的很棒！",
        //            "谢谢阁下的礼物，在下会好好珍惜",
        //            "那咱们去击剑吧",
        //            "阁下事在开玩笑吗"
        //        };
        characterDialogue = new Dictionary<string, string[]>()
        {
            {"Test",GameManager.Instance.LoadByJson<string[]>("/Json/SingleDialogue/Test.json")},
            { "Debug",GameManager.Instance.LoadByJson<string[]>("/Json/SingleDialogue/Debug.json")}
        };
        //GameManager.Instance.SaveByJson<string[]>(testStrs, "/Json/SingleDialogue/Test.json");
        //GameManager.Instance.SaveByJson<string[]>(debugStrs, "/Json/SingleDialogue/Debug.json");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowChoiceUI()
    {
        gameManager.ShowChoiceUI(3,new string[] {"Test","Debug","结束今天的活动" },ChooseCharacter);
    }

    private void ChooseCharacterAndHandle()
    {
        gameManager.UpdateCharacterName(currentCharacterName);
        gameManager.CloseChoiceUI();
        gameManager.DoShowOrHideCharacterTween(true, false, 1, currentCharacterName, 3);
        Invoke("PlaySoundAndShowChoiceLineUI", 1.1f);
    }

    /// <summary>
    /// 选择Test
    /// </summary>
    private void ChooseTest()
    {
        currentCharacterName = "Test";
        ChooseCharacterAndHandle();
    }
    private void PlaySoundAndShowChoiceLineUI()
    {
        gameManager.PlayGameSound("Single/0", 1, currentCharacterName);
        ShowChoiceLineUI();
        gameManager.AddClickButtonListener(ShowChoiceLineUI);
    }
    /// <summary>
    /// 显示与人物之前的交流行为
    /// </summary>
    private void ShowChoiceLineUI(object src=null)
    {
        gameManager.ShowChioceLineUI(4, new string[] { "交谈", "送礼物", "约会", "没事了" }, Act);
    }

    /// <summary>
    /// 选择Debug
    /// </summary>
    public void ChooseDebug()
    {
        currentCharacterName = "Debug";
        ChooseCharacterAndHandle();
    }
    /// <summary>
    /// 结束当天的活动
    /// </summary>
    public void FinishActivity()
    {
        gameManager.CloseChoiceUI();
        gameManager.LoadNextScript();
    }

    public void ChooseCharacter(object chooseID)
    {
        chooseActions[(int)chooseID]();
    }

    public void Act(object actID)
    {
        actActions[(int)actID]();
    }

    private void Talk()
    {
        gameManager.PlayGameSound("Single/1", 1, currentCharacterName);
        gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][1],false);
    }

    private void GiveGifts()
    {
        if (gameManager.IfOwnItem(2)&&currentCharacterName=="Test")
        {
            gameManager.PlayGameSound("Single/2", 1, currentCharacterName);
            gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][2], false);
            gameManager.RemoveItem(2);
            gameManager.ChangeFavorValue(20,currentCharacterName);
        }
        else
        {
            gameManager.PlayGameSound("Single/4",1,currentCharacterName);
            gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][4],false);
        }
    }

    private void Date()
    {
        gameManager.PlayGameSound("Single/3", 1, currentCharacterName);
        gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][3], false);
    }

    private void ReturnToChoice()
    {
        gameManager.DoShowOrHideCharacterTween(false,false,1,currentCharacterName,3);
        Invoke("ShowChoiceUI", 1.1f);
    }
}
