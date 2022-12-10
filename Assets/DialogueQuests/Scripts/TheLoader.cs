using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DialogueQuests
{

    public class TheLoader : MonoBehaviour
    {
        [Header("Loader")]
        public AssetReferenceGameObject ui_canvas;
        public AssetReferenceGameObject audio_manager;
        public AssetReferenceGameObject event_system;
        public AssetReferenceGameObject chat_bubble;

        [Header("Resources")]
        public string actors_folder = "Actors";
        public string quests_folder = "Quests";

        private void Awake()
        {
            if (ui_canvas != null && !FindObjectOfType<TheUI>())
                Addressables.InstantiateAsync(ui_canvas).WaitForCompletion();
            if (audio_manager != null && !FindObjectOfType<TheAudio>())
                Addressables.InstantiateAsync(audio_manager).WaitForCompletion();
            if (event_system != null && !FindObjectOfType<UnityEngine.EventSystems.EventSystem>())
                Addressables.InstantiateAsync(event_system).WaitForCompletion();
            if (chat_bubble != null && !FindObjectOfType<ChatBubbleFX>())
                Addressables.InstantiateAsync(chat_bubble).WaitForCompletion();

            ActorData[] all_actors = Resources.LoadAll<ActorData>(actors_folder);
            foreach (ActorData quest in all_actors)
                ActorData.Load(quest);

            QuestData[] all_quests = Resources.LoadAll<QuestData>(quests_folder);
            foreach (QuestData quest in all_quests)
                QuestData.Load(quest);
        }
    }

}
