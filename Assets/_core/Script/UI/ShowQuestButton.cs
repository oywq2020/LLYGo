using System.Collections;
using System.Collections.Generic;
using DialogueQuests;
using UnityEngine;
using UnityEngine.UI;

public class ShowQuestButton : MonoBehaviour
{
    // Start is called before the first frame update
    private bool swift = true;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            QuestPanel.Get().TogglePanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
