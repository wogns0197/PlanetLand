using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum ButtonType
    {
        All,
        Fashion,
        Consume,
        Equip,
        Etc,
    }
public class UIInventory : UIBase
{
    public Button TypeBut0, TypeBut1, TypeBut2, TypeBut3, TypeBut4;
    public TextMeshPro Tmpro0, Tmpro1, Tmpro2, Tmpro3, Tmpro4;

    private string UnSelectColor = "#E9E9E9";
    private string SelectColor = "#4795FF";
    ButtonType EBType;
    
    void Start()
    {
        TypeBut0.onClick.AddListener(OnClickedBut0);
        TypeBut1.onClick.AddListener(OnClickedBut1);
        TypeBut2.onClick.AddListener(OnClickedBut2);
        TypeBut3.onClick.AddListener(OnClickedBut3);
        TypeBut4.onClick.AddListener(OnClickedBut4);
        Tmpro0 = TypeBut0.transform.GetChild(0).GetComponent<TextMeshPro>();
        Tmpro1 = TypeBut1.transform.GetChild(0).GetComponent<TextMeshPro>();
        Tmpro2 = TypeBut2.transform.GetChild(0).GetComponent<TextMeshPro>();
        Tmpro3 = TypeBut3.transform.GetChild(0).GetComponent<TextMeshPro>();
        Tmpro4 = TypeBut4.transform.GetChild(0).GetComponent<TextMeshPro>();

        if(Tmpro0.color == null)
            Debug.Log("12");

    }
    void Update()
    {
        
    }

    void ResetButtonFocus()
    {
        Color color;
        ColorUtility.TryParseHtmlString(UnSelectColor, out color);

        TypeBut0.image.color = color;
        TypeBut1.image.color = color;
        TypeBut2.image.color = color;
        TypeBut3.image.color = color;
        TypeBut4.image.color = color;

        Color colorT;
        ColorUtility.TryParseHtmlString("#ffffff", out colorT);


        Tmpro0.color = colorT;
        Tmpro1.color = colorT;
        Tmpro2.color = colorT;
        Tmpro3.color = colorT;
        Tmpro4.color = colorT;
    }

    void SetButtonFocus(int i)
    {
        Color color;
        ColorUtility.TryParseHtmlString(SelectColor, out color);

        Color colorT;
        ColorUtility.TryParseHtmlString("#000000", out colorT);

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
    }

    void OnClickedBut0()
    {
        ResetButtonFocus();
        SetButtonFocus(0);
    }
    void OnClickedBut1()
    {
        ResetButtonFocus();
        SetButtonFocus(1);
    }
    void OnClickedBut2()
    {
        ResetButtonFocus();
        SetButtonFocus(2);
    }
    void OnClickedBut3()
    {
        ResetButtonFocus();
        SetButtonFocus(3);
    }
    void OnClickedBut4()
    {
        ResetButtonFocus();
        SetButtonFocus(4);
    }
}
