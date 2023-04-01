using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utility;

enum EInventorytype
{
    All = 0,
    Fashion,
    Consume,
    Equip,
    Etc,
}
public class UIInventory : UIBase
{
    public Button TypeBut0, TypeBut1, TypeBut2, TypeBut3, TypeBut4, CloseBut;
    public Button ContentsLBut, ContentsRBut;
    public TextMeshProUGUI Tmpro0, Tmpro1, Tmpro2, Tmpro3, Tmpro4;
    public GameObject ContentBox;

    private string UnSelectColor = "#E9E9E9";
    private string SelectColor = "#4795FF";
    EInventorytype EBType;

    // ===================================================
    // test 
    public Button Button_Test;
    // ===================================================

    private List<ItemSlotInfo> ItemContentsArr = new List<ItemSlotInfo>();
    public static Dictionary<EItemType, Dictionary<int, ItemSlotInfo>> II;
    
    public override void OnOpen()
    {
        // !!!!!!!!!!!
        // 매번 열 때마다 말고 변경사항있으면 로드해야함
        // !!!!!!!!!!!!
        II = PUtility.GetInventoryData();
    }

    void Start()
    {
        //test
        Button_Test.onClick.AddListener(OnClickedTest1);

        TypeBut0.onClick.AddListener(OnClickedBut0);
        TypeBut1.onClick.AddListener(OnClickedBut1);
        TypeBut2.onClick.AddListener(OnClickedBut2);
        TypeBut3.onClick.AddListener(OnClickedBut3);
        TypeBut4.onClick.AddListener(OnClickedBut4);
        // CloseBut.onClick.AddListener(() => { this.gameObject.SetActive(false); });
        // Ship의 키로만 on off로 일단 변경 해둠

        Tmpro0 = TypeBut0.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Tmpro1 = TypeBut1.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Tmpro2 = TypeBut2.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Tmpro3 = TypeBut3.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Tmpro4 = TypeBut4.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        ResetInventory();

        DataMgr.OnMoneyUpdate += OnMoneyUpdated;

    }

    void OnClickedTest1()
    {
        DataMgr.OnMoneyUpdate(1000);
    }
    void Update()
    {
        
    }

    void OnMoneyUpdated(int outDelegateNum)
    {

    }

    void ResetInventory()
    {
        // 여기서 캐릭터 인벤토리 data기준으로 Usable, NotUsable 설정(태그포함) 해주고
        // Usable 설정 이후의 프리팹 프리뷰로 넣어주기
        // 일단 지금은 Usable 4개, Not Usable 12개로 set 해둠
        // Dummy
        SetContentsArr();

        for (int i = 0; i < ContentBox.transform.childCount; i++)
        {
            GameObject el = ContentBox.transform.GetChild(i).gameObject;
            if( el == null) { continue; }

            UIInventoryContent cont = el.GetComponent<UIInventoryContent>();
            if ( cont == null )  { continue; }

            if (i >= ItemContentsArr.Count ) 
            { 
                cont.LoadData(null, 0);
                cont.SetNotUsable();
                continue;
            }

            else
            {
                if(ItemContentsArr[i].Count > 0)
                {
                    Sprite texture = PUtility.GetPreview(ItemContentsArr[i].Path, ItemContentsArr[i].ItemCode);
                    cont.LoadData(texture, ItemContentsArr[i].Count);
                }
            }
        }
    }

    void SetContentsArr()
    {
        // Data 많아지면 무거워질 수 있으니 추후 텀 두고 Load 하도록 수정하자
        II = PUtility.GetInventoryData();
        ItemContentsArr.Clear();

        if ( EBType == EInventorytype.All )
        {
            foreach (var el in II)
            {
                foreach (var val in el.Value)
                {
                    ItemContentsArr.Add(val.Value);
                }
            }
        }
        else
        {
            // ㅋㅋ 진짜 C#.....
            foreach (var val in II[(EItemType)((int)EBType * 10^6)])
            {
                ItemContentsArr.Add(val.Value);
            }
        }
    }

    void ResetButtonType()
    {
        Color color;
        ColorUtility.TryParseHtmlString(UnSelectColor, out color);

        TypeBut0.image.color = color;
        TypeBut1.image.color = color;
        TypeBut2.image.color = color;
        TypeBut3.image.color = color;
        TypeBut4.image.color = color;

        Color colorT;
        ColorUtility.TryParseHtmlString("#323232", out colorT);


        Tmpro0.color = colorT;
        Tmpro1.color = colorT;
        Tmpro2.color = colorT;
        Tmpro3.color = colorT;
        Tmpro4.color = colorT;
    }

    void SetButtonType(int i)
    {
        Color color;
        ColorUtility.TryParseHtmlString(SelectColor, out color);

        Color colorT;
        ColorUtility.TryParseHtmlString("#ffffff", out colorT);

        EBType = (EInventorytype)i;

        switch (i)
        {
            case 0:
                TypeBut0.image.color = color;
                Tmpro0.color = colorT;
                break;
            case 1:
                TypeBut1.image.color = color;
                Tmpro1.color = colorT;
                break;
            case 2:
                TypeBut2.image.color = color;
                Tmpro2.color = colorT;
                break;
            case 3:
                TypeBut3.image.color = color;
                Tmpro3.color = colorT;
                break;
            case 4:
                TypeBut4.image.color = color;
                Tmpro4.color = colorT;
                break;
            default:
                break;
        }

        ResetInventory();
    }

    void OnClickedBut0()
    {
        ResetButtonType();
        SetButtonType(0);
    }
    void OnClickedBut1()
    {
        ResetButtonType();
        SetButtonType(1);
    }
    void OnClickedBut2()
    {
        ResetButtonType();
        SetButtonType(2);
    }
    void OnClickedBut3()
    {
        ResetButtonType();
        SetButtonType(3);
    }
    void OnClickedBut4()
    {
        ResetButtonType();
        SetButtonType(4);
    }
}
