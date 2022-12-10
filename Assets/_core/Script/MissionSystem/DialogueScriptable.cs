using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _core.Script.MissionSystem
{
    public class DialogueScriptable:ScriptableObject
    {
        //dialogue id 
        public int Id;

        public AssetReferenceSprite headSprite;
        
        //dialogue content
        [TextArea] public string Dialouge;
    }
}