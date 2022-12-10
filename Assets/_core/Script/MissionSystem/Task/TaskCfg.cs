using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.AddressableAssets;

namespace _core.Script.MissionSystem.Task
{
    
    public class TaskCfgItem
    {
        public int task_chain_id;
        public int task_sub_id;
        public string icon;
        public string desc;
        public string task_target;
        public int target_amount;
        public string award;
        public string open_chain;
    }

    
    
    public class TaskCfg
    {
        //to store 
        private Dictionary<int, Dictionary<int, TaskCfgItem>> m_cfg;
        
        //load task from json data 
        public void LoadCfg()
        {
            m_cfg = new Dictionary<int, Dictionary<int, TaskCfgItem>>();
            var txt = Addressables.LoadAssetAsync<TextAsset>("task_cfg").WaitForCompletion().text;
            var jd = JsonMapper.ToObject<JsonData>(txt);


            for (int i = 0, cnt = jd.Count; i < cnt; ++i)
            {
                var itemJd = jd[i] as JsonData;
                TaskCfgItem cfgItem = JsonMapper.ToObject<TaskCfgItem>(itemJd.ToJson());

                if (!m_cfg.ContainsKey(cfgItem.task_chain_id))
                {
                    m_cfg[cfgItem.task_chain_id] = new Dictionary<int, TaskCfgItem>();
                }
                m_cfg[cfgItem.task_chain_id].Add(cfgItem.task_sub_id, cfgItem);
            }

        }
        
        
        //get a Task Cfg Item from 
        public TaskCfgItem GetCfgItem(int chainId, int taskSubId)
        {
            if (m_cfg.ContainsKey(chainId) && m_cfg[chainId].ContainsKey(taskSubId))
                return m_cfg[chainId][taskSubId];
            return null;
        }
      
       
    }

    
}