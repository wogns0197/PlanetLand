using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum EContentsType
{
    Usable,
    NotUsable,
    Etc,
    // 교불, 업그레이드 등 옵션 추가
}
public class UIInventoryContent : MonoBehaviour
{
    private EContentsType ContentsType;
    private Image BackImage, ContentsImage;
    
    private Color32 BackUsableColor, BackNotUsableColor, ContUsableColor, ContNotUsableColor;

    // 리소스 로드가 안되어 Sprite 그대로 박아둠
    public Sprite SPNotUsable;
    public TextMeshProUGUI NumText;

    EContentsType GetContentsType () { return ContentsType; }
    void SetType (EContentsType T) { ContentsType = T; }
    void SetType (EContentsType T, string Path) 
    { 
        ContentsType = T;
        
        // !! Not Usable -> Usable 일땐 무조건 경로 지정하여 프리팹 프리뷰 넣어주도록 하기
    }

    void Awake()
    {
        NumText.text = "";
        BackUsableColor = new Color32(255, 255, 255, 132);
        BackNotUsableColor = new Color32(232, 232, 232, 100);
        // ContUsableColor =;
        // ContNotUsableColor =;
        BackImage = this.GetComponent<Image>();
        ContentsImage = this.transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        
    }

    public void LoadData(Sprite sprite = null, int num = 0)
    {
        if(ContentsImage == null) { Debug.Log("!!!!!!");}
        ContentsImage.sprite = null;

        if(sprite != null)
        {
            BackImage.color = BackUsableColor;
            ContentsImage.sprite = sprite;
            SetType(EContentsType.Usable);
        }

        NumText.text = num == 0 ? "" : num.ToString();
    }

    public void SetNotUsable()
    {
        SetType(EContentsType.NotUsable);
        BackImage.color = BackNotUsableColor;
        //ContentsImage.sprite = SPNotUsable;
    }
}
