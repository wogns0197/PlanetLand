using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUIType
{
    Inventory = 0,
}

struct UIProperty
{
    public bool bToggle;        // on이면 다음은 무조건 off 인 UI
    public bool bCheckClose;    // 종료확인 팝업 뜨는 UI
    public bool bExclusive;     // 다른 UI와 공존 가능한 UI

    // C# 구조체 생성자는 무조건 인자가 들어가야함
    public UIProperty(bool _bToggle, bool _bCheckClose, bool _bExclusive) 
    { 
        this.bToggle = true;
        this.bCheckClose = false;
        this.bExclusive = false;
    }
}

public class UIInstance : MonoBehaviour
{
    public static UIInstance _instance = null;

    public static UIInstance GetUIInstance() { return _instance; }

    // UI 등록시 해당 프리팹 꼭 추가해줘야 함 !!EUIType과 순서 유지해야함
    [SerializeField]
    private List<GameObject> UIList;

    // 언리얼 처럼 map 인스펙터에서 지원 안해주는거 오반데..
    private Dictionary<EUIType, UIBase> UIMap;
    private Dictionary<EUIType, UIProperty> UIState;
    private Dictionary<EUIType, bool> UIShowCheck;
    private void Awake() 
    {
        if ( _instance == null ) { _instance = this; }
        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        UIMap = new Dictionary<EUIType, UIBase>();
        UIState = new Dictionary<EUIType, UIProperty>();
        UIShowCheck = new Dictionary<EUIType, bool>();

        for (int i = 0; i < UIList.Count; i++)
        {
            GameObject obj = null;
            if ( !this.transform.Find(UIList[i].gameObject.name) )
            {
                UIBase BaseComp = UIList[i].GetComponent<UIBase>();
                if ( BaseComp == null ) 
                {
                    Debug.LogFormat("{0} have no UIBase Component!", UIList[i].gameObject.name);
                    return;
                }

                obj = Instantiate(UIList[i]);
                obj.transform.parent = this.gameObject.transform;
                obj.SetActive(false);
                
                InitUIProperty((EUIType)i);
                
                UIShowCheck.Add((EUIType)i, false);
                UIBase objComp = obj.GetComponent<UIBase>();
                UIMap.Add((EUIType)i , objComp);
            }
        }
    }

    // 제네릭이나 템플릿으로 캐스팅 해서 뱉고싶은데...
    public UIBase GetUI(EUIType type)
    {
        return UIMap[type].GetComponent<UIBase>();
    }

    public void OpenUI(EUIType type, bool bShow = true)
    {
        if ( !UIShowCheck.ContainsKey(type) )
        {
            if ( bShow == false ) 
            { 
                Debug.LogError("Not Shown UI can not be Show true");
                return;
            }
        }

        if ( bShow ) { GetUI(type).Open(); }
        else { GetUI(type).Close(); }

        UIShowCheck[type] = bShow;
    }

    public bool IsUIShow(EUIType type) { return UIShowCheck[type]; }
    private void InitUIProperty(EUIType type)
    {
        if ( !UIState.ContainsKey(type) ) 
        { 
            UIState[type] = new UIProperty();
            return; 
        }

        UIProperty Prop = UIState[type];
        switch (type)
        {
            // Toggle Property : on 다음 무조건 off여야 하는 UI들은 하위 케이스로 추가
            case EUIType.Inventory :
            //case EUIType.ADDSOMETHING :
            {
                // ㅋㅋ포인터 안돼서 복사된 구조체가 나오기때문에 수정이 안됨... 위에 선언해둔 Prop 구조체 수정해서 다시 넣어주자
                //UIState[type].bToggle = true;
                Prop.bToggle = true;
                break;
            }

            default:
                break;
        }

        UIState[type] = Prop;
    }
}
