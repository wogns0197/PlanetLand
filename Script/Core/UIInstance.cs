using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUIType
{
    Inventory = 0,
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
    private void Awake() 
    {
        DontDestroyOnLoad(this);

        UIMap = new Dictionary<EUIType, UIBase>();
        for (int i = 0; i < UIList.Count; i++)
        {
            UIBase BaseComp = UIList[i].GetComponent<UIBase>();
            if ( BaseComp == null ) 
            {
                Debug.LogFormat("{0} have no UIBase Component!", UIList[i].gameObject.name);
                return;
            }
            UIMap.Add((EUIType)i , BaseComp);
        }
    }

    private void Start() {
    }

    // 제네릭이나 템플릿으로 캐스팅 해서 뱉고싶은데...
    public UIBase GetUI(EUIType type) 
    {
        return UIMap[type];
    }

    
}
