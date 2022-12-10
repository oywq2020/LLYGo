using Script.Event;
using UnityEngine;

namespace DialogueQuests.Custom
{
    [CreateAssetMenu(fileName = "CustomCondition",menuName = "")]
    public class CustomCondition:DialogueQuests.CustomCondition
    {
        private bool _isComplete = false;
        public override void Start()
        {
            GameFacade.Instance.RegisterEvent<OnCompleteQuest>(e =>
            {
                _isComplete = true;
               
            });
            base.Start();
          
        }

        public override bool IsMet(Actor player)
        {
            return _isComplete;
        }
    }
}