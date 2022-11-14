using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPointEvent : MonoBehaviour
{
    private bool gameStart;
    private float startTime;
    private float timeVal;
    public GameObject[] points;
    private Image[] imgPoints;
    public int hitNum;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = true;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if (Time.time-startTime>=10)
            {
                //游戏结束
                gameStart = false;
                if (hitNum>=3)
                {
                    //游戏胜利
                    GameManager.Instance.LoadNextScript(2);
                }
                else
                {
                    //游戏失败
                    GameManager.Instance.LoadNextScript();
                }
                gameObject.SetActive(false);
            }
            else
            {
                //游戏进行中的内容
                timeVal -= Time.deltaTime;
                if (timeVal<=0)
                {
                    ShowPoints();
                    timeVal = 2;
                }
            }
        }
    }

    private void ShowPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].SetActive(false);
        }
        GameObject point= points[Random.Range(0, points.Length)];
        point.SetActive(true);
        Image[] images = point.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(images[i].color.r, images[i].color.g
                , images[i].color.b,0);
        }
        GameManager.Instance.DoShowOrHideUITween(true,false,2,images);
    }

    public void HitPoint(GameObject obj)
    {
        AudioSourceManager.Instance.PlaySound("Hit");
        hitNum++;
        obj.SetActive(false);
    }
}
