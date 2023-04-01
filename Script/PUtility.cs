using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor; // for AssetPreview

// parsing string to enum : Color c = (Color)Enum.Parse(typeof(Color), "Blue")

public enum EItemType // max int 64
{
    Fashion = EInventorytype.Fashion * 10^6,

    Consume = EInventorytype.Consume * 10^6,
    Fish    = Consume | 1,


    Equip   = EInventorytype.Equip * 10^6,

    Etc     = EInventorytype.Etc * 10^6,
}

[Serializable]
public struct ItemSlotInfo
{
    public int ItemCode;
    public int Count;
    public string Name;
    public EItemType Type;
    public string Path;

    // public Vector3 Pos; 위치변경 추후 추가 : All 고려해야함. All을 빼버리던가 해야할 듯

    public ItemSlotInfo(int _ItemCode, int _Count, string _Name, EItemType _Type, string _Path)
    {
        this.ItemCode = _ItemCode;
        this.Count = _Count;
        this.Name = _Name;
        this.Type = _Type;
        this.Path = _Path;
    }
}
public struct JsonInventoryInfo 
{
    // Only Use For Read Json!
    // Json 파싱 이후, 탐색 속도와 접근용이를 위해 Dictionary InventoryInfo로 로드해줌
    public ItemSlotInfo[] FashionContents;
    public ItemSlotInfo[] ConsumeContents;
    public ItemSlotInfo[] EquipContents;
    public ItemSlotInfo[] EtcContents;
}

namespace Utility
{
    using InventoryInfo = Dictionary<EItemType, Dictionary<int, ItemSlotInfo>>;
    public class PUtility : MonoBehaviour
    {
        public static InventoryInfo _InventoryInfo;

        void Start()
        {
            _InventoryInfo = new InventoryInfo();
            LoadInventoryInfoFromJson();
        }
        static Dictionary<int, Texture2D> mStaticTexture = new Dictionary<int, Texture2D>();

        public void LoadInventoryInfoFromJson()
        {
            TextAsset ta = Resources.Load<TextAsset>("Data/Dummy/CharacterInventoryData");
            JsonInventoryInfo II = JsonUtility.FromJson<JsonInventoryInfo>(ta.text);

            // 구조 진짜 이상한데...
            Dictionary<int, ItemSlotInfo> Fashionslots = new Dictionary<int, ItemSlotInfo>();
            for (int i = 0; i < II.FashionContents.Length; i++)
            {
                Fashionslots.Add(II.FashionContents[i].ItemCode, II.FashionContents[i]);
            }
            _InventoryInfo.Add(EItemType.Fashion, Fashionslots);

            Dictionary<int, ItemSlotInfo> ConsumeSlots = new Dictionary<int, ItemSlotInfo>();
            for (int i = 0; i < II.ConsumeContents.Length; i++)
            {
                ConsumeSlots.Add(II.ConsumeContents[i].ItemCode, II.ConsumeContents[i]);
            }
            _InventoryInfo.Add(EItemType.Consume, ConsumeSlots);

            Dictionary<int, ItemSlotInfo> EquipSlots = new Dictionary<int, ItemSlotInfo>();
            for (int i = 0; i < II.EquipContents.Length; i++)
            {
                EquipSlots.Add(II.EquipContents[i].ItemCode, II.EquipContents[i]);
            }
            _InventoryInfo.Add(EItemType.Equip, EquipSlots);

            Dictionary<int, ItemSlotInfo> EtcSlots = new Dictionary<int, ItemSlotInfo>();
            for (int i = 0; i < II.EtcContents.Length; i++)
            {
                EtcSlots.Add(II.EtcContents[i].ItemCode, II.EtcContents[i]);
            }
            _InventoryInfo.Add(EItemType.Etc, EtcSlots);
        }

        public static Dictionary<EItemType, Dictionary<int, ItemSlotInfo>> GetInventoryData()
        {
            return _InventoryInfo;
        }

        public static Sprite GetPreview(string path, int SN /*SerialNumber*/)
        {
            if(path.Length == 0) { return null; }
            Texture2D tx = null;
            Sprite sp = null;
            
            GameObject target = Resources.Load(path) as GameObject;
            if (target != null)
            {
                if (mStaticTexture.ContainsKey(SN))
                {
                    tx = mStaticTexture[SN];
                }

                else 
                {
                    if (Application.isPlaying)
                        EditorUtility.SetDirty(target);

                    tx = AssetPreview.GetAssetPreview(target);
                    int tries = 1000;
                    while (AssetPreview.IsLoadingAssetPreview(target.GetInstanceID()) && tries > 0)
                    {
                        tries--;
                    }

                    mStaticTexture[SN] = tx;
                }
            }

            else{
                Debug.Log("NULL!!");
            }

            Rect rc = new Rect(0, 0, tx.width, tx.height);
            sp = Sprite.Create(tx, rc, new Vector2(0.5f, 0.5f));

            return sp;
        }
    }
}
