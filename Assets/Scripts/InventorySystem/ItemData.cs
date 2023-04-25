using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        public string displayName;
        public Sprite icon;
        public string itemDetails;
    }
}