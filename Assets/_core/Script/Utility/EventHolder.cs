using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Script.Facade
{
    public interface IEventHolder
    {
        IUnRegister Register<T>(Action<T> action) where T : new();
        void UnRegister<T>(Action<T> action) where T : new();

        void Send<T>() where T : new();
        void Send<T>(T t) where T : new();
    }

    public interface IEvent
    {
    }

    public interface IUnRegister
    {
        void UnRegister();
    }


    /// <summary>
    /// define a UnRegister list interface
    /// </summary>
    public interface IUnRegisterList
    {
        List<IUnRegister> UnRegisterList { get; }
    }

    /// <summary>
    /// Custom the UnRegister Method
    /// </summary>
    public struct CustomUnRegister : IUnRegister
    {
        private Action _onUnRegister;

        //assign on constructing
        public CustomUnRegister(Action onUnRegister)
        {
            _onUnRegister = onUnRegister;
        }

        //UnRegister this function
        public void UnRegister()
        {
            _onUnRegister.Invoke();
            _onUnRegister = null;
        }
    }

    /// <summary>
    /// IUnRegister extension method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class IUnRegisterExtension
    {
        public static void AddToUnRegisterList(this IUnRegister self, IUnRegisterList unRegisterList)
        {
            //take self, IUnRegister, to the argument,unRegisterList
            unRegisterList.UnRegisterList.Add(self);
        }


        /// <summary>
        /// UnRegister those IUnRegister that added self UnRegisterList 
        /// </summary>
        /// <param name="self"></param>
        public static void UnRegisterAll(this IUnRegisterList self)
        {
            foreach (var unRegister in self.UnRegisterList)
            {
                unRegister.UnRegister();
            }

            //clear this list
            self.UnRegisterList.Clear();
        }
    }

    /// <summary>
    /// UnRegister Trigger attach the mono that configure auto register event
    /// on destroy
    /// </summary>
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnRegister> _unRegisters = new HashSet<IUnRegister>();

        //add unRegister to UnRegister collection
        public void AddUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Add(unRegister);
        }

        public void RemoveUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Remove(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in _unRegisters)
            {
                unRegister.UnRegister();
            }

            _unRegisters.Clear();
        }
    }

    public static class UnRegisterExtension
    {
        /// <summary>
        /// get or add a trigger component from this gameobject that call for this extension method
        /// and add the UnRegister to UnRegister List of this Trigger
        /// </summary>
        public static IUnRegister UnRegisterOnDestroy(this IUnRegister self, GameObject gameObject)
        {
            var unRegisterOnDestroyTrigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!unRegisterOnDestroyTrigger)
            {
                unRegisterOnDestroyTrigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            unRegisterOnDestroyTrigger.AddUnRegister(self);
            return self;
        }
    }


    public class MyEvent<T> : IEvent
    {
        private Action<T> _action = null;

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

        //this type event chain is null
        public bool IsNull()
        {
            return _action == null;
        }
    }


    public class EventHolder : IEventHolder
    {
        //store registered Event
        // private Dictionary<Type, IEvent> _dictionary = new Dictionary<Type, IEvent>();
        private Dictionary<Type, IEvent> _dictionary = new Dictionary<Type, IEvent>();

        // public void Register<T>(Action<T> action) where T : new()
        // {
        //     if (_dictionary.TryGetValue(typeof(T), out IEvent e))
        //     {
        //         //register event
        //         ((MyEvent<T>) e).Register(action);
        //     }
        //     else
        //     {
        //         var myEvent = new MyEvent<T>();
        //         myEvent.Register(action);
        //         _dictionary.Add(typeof(T), myEvent);
        //     }
        //     
        // }


        public IUnRegister Register<T>(Action<T> action) where T : new()
        {
            MyEvent<T> myEvent;

            if (_dictionary.TryGetValue(typeof(T), out IEvent e))
            {
                //register event
                myEvent = (MyEvent<T>) e;
                myEvent.Register(action);
            }
            else
            {
                myEvent = new MyEvent<T>();
                myEvent.Register(action);
                _dictionary.Add(typeof(T), myEvent);
            }

            //return UnRegister method
            return new CustomUnRegister(() =>
            {
                myEvent.UnRegister(action);
                
                //if this Event is null remove it from dictionary
                if (myEvent.IsNull())
                {
                    _dictionary.Remove(typeof(T));
                }
            });
        }

        public void UnRegister<T>(Action<T> action) where T : new()
        {
            if (_dictionary.TryGetValue(typeof(T), out IEvent e))
            {
                ((MyEvent<T>) e).UnRegister(action);
            }
        }

        public void Send<T>() where T : new()
        {
            if (_dictionary.TryGetValue(typeof(T), out IEvent e))
            {
                ((MyEvent<T>) e).Trigger(new T());
            }
        }

        public void Send<T>(T t) where T : new()
        {
            if (_dictionary.TryGetValue(typeof(T), out IEvent e))
            {
                ((MyEvent<T>) e).Trigger(t);
            }
        }
    }
}