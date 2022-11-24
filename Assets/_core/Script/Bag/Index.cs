using UnityEngine;

namespace _core.Script.Bag
{
    public class Index : MonoBehaviour
    {
        [SerializeField]
        private int index = 0;

        public int GetIndex()
        {
            return index;
        }
    }
}