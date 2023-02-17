using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor; // for AssetPreview

// parsing string to enum : Color c = (Color)Enum.Parse(typeof(Color), "Blue")
public enum EItemType // max int 64
{
    Fashion = 1 << 14,

    Consume = 1 << 15,
    Fish    = 1 << 15 | 1,


    Equip   = 1 << 16,

    Etc     = 1 << 17,
}

[Serializable]
public struct ItemSlotInfo
{
    public int Count;
    public string Name;
    public EItemType Type;
    public string ThumbnailPath;
}

public struct InventoryInfo
{
    public ItemSlotInfo[] FashionContents;
    public ItemSlotInfo[] ConsumeContents;
    public ItemSlotInfo[] EquipContents;
    public ItemSlotInfo[] EtcContents;
}

public class PUtility : MonoBehaviour
{
    static Dictionary<int, Texture2D> mStaticTexture = new Dictionary<int, Texture2D>();
    public static InventoryInfo GetInventoryData()
    {
        TextAsset ta = Resources.Load<TextAsset>("Data/Dummy/CharacterInventoryData");

        InventoryInfo II = JsonUtility.FromJson<InventoryInfo>(ta.text);

        return II;
    }

    public static Sprite GetPreview(string path)
    {
        if(path.Length == 0) { return null; }
        Texture2D tx = null;
        Sprite sp = null;
        
        GameObject target = Resources.Load(path) as GameObject;
        if (target != null)
        {
            if (mStaticTexture.ContainsKey(target.GetInstanceID()))
            {
                tx = mStaticTexture[target.GetInstanceID()];
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

                mStaticTexture[target.GetInstanceID()] = tx;
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
