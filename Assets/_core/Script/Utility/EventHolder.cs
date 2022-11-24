using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Script.Facade
{



    public interface IEventHolder
    {
        void Register<T>(Action<T> action) where T : new();
        void UnRegister<T>(Action<T> action) where T : new();

        void Send<T>() where T : new();
        void Send<T>(T t) where T : new();

    }
    public interface IEvent
    {
        
    }
    public class MyEvent<T>:IEvent
    {
        private Action<T> _action = e => { };

        public void Register(Action<T> action)
        {
            _action += action;
        }

        public void UnRegister(Action<T> action)
        {
            _action -= action;
        }

       
        public void Trigger(T t)
        {
            _action?.Invoke(t);
        }
    }


    public class EventHolder:IEventHolder
    {
        //store registered Event
        private Dictionary<Type, IEvent> _dictionary = new Dictionary<Type, IEvent>();

        public void Register<T>(Action<T> action) where T : new()
        {
            if (_dictionary.TryGetValue(typeof(T),out IEvent e))
            {
                //register event
                 ((MyEvent<T>)e).Register(action);
            }
            else
            {
                var myEvent = new MyEvent<T>();
                myEvent.Register(action);
                _dictionary.Add(typeof(T),myEvent);
            }
        }

        public void UnRegister<T>(Action<T> action) where T : new()
        {
            if (_dictionary.TryGetValue(typeof(T),out IEvent e))
            {
                ((MyEvent<T>)e).UnRegister(action);
            }
        }
        public void Send<T>()where T:new()
        {
            
            if (_dictionary.TryGetValue(typeof(T),out IEvent e))
            {
                ((MyEvent<T>)e).Trigger(new T());
            }
        }
        public void Send<T>(T t)where T:new()
        {
            if (_dictionary.TryGetValue(typeof(T),out IEvent e))
            {
                ((MyEvent<T>)e).Trigger(t);
            }
        }
    }
}