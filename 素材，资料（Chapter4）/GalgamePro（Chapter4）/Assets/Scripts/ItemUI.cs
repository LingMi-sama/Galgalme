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

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
            //transform.localPosition = Vector3.zero;
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
            //transform.localPosition = Vector3.zero;
        }
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 使用物品
    /// </summary>
    public void UseItem()
    {
        if (GameManager.Instance.itemEffectDict.ContainsKey(itemInfo.id))
        {
            GameManager.Instance.itemEffectDict[itemInfo.id](itemInfo.src);
            UIManager.Instance.HideTipInfo();
            Destroy(gameObject);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTipInfo(itemInfo,transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTipInfo();
    }
}
