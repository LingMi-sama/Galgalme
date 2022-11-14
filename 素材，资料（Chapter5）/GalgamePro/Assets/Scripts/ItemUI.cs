using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    public ItemInfo itemInfo;
    private Transform imgBagBackground;
    private CanvasGroup canvasGroup;
    private GameManager gameManager;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameManager = GameManager.Instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        imgBagBackground = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(UIManager.Instance.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject pointObj= eventData.pointerEnter;
        if (pointObj==null|| pointObj.tag == "Bag")//鼠标下方是空白
        {
            transform.SetParent(imgBagBackground);
        }
        else if (pointObj.tag=="Item")
        {
            if (pointObj.GetComponent<ItemUI>().itemInfo.id==itemInfo.id)
            {
                Destroy(gameObject);
                //数量合并
            }
            else
            {
                transform.SetParent(pointObj.transform.parent);
                transform.localPosition = Vector3.zero;
                transform.SetSiblingIndex(pointObj.transform.GetSiblingIndex());
                pointObj.transform.SetSiblingIndex(transform.parent.childCount+1);
            }
        }
        else
        {
            transform.SetParent(imgBagBackground);
        }
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 使用物品
    /// </summary>
    public void UseItem()
    {
        if (gameManager.itemEffectDict.ContainsKey(itemInfo.id))
        {
            gameManager.itemEffectDict[itemInfo.id](itemInfo.src);
            gameManager.HideTipInfo();
            Destroy(gameObject);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.ShowTipInfo(itemInfo,transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.HideTipInfo();
    }
}
