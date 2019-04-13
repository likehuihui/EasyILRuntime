using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

public class Loom : MonoBehaviour
{
    public static Thread unityThread;

    private static Loom _ins;
    
    public static Loom Current
    {
        get
        {        
            //if(null == _ins)
            //{
            //    _ins = new GameObject("Loom").AddComponent<Loom>();
            //}
            return _ins;
        }
    }

    private List<Action> _actions = new List<Action>();

    void Awake()
    {        
        if(null != _ins)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        _ins = this;
        DontDestroyOnLoad(gameObject);
        unityThread = Thread.CurrentThread;
    }

    public static void QueueOnMainThread(Action action)
    {
        lock (Current._actions)
        {
            Current._actions.Add(action);
        }
    }

    private void OnDestroy()
    {
        if (_ins == this)
        {
            _ins = null;
        }
    }

    private void FixedUpdate()
    {
        if (_actions.Count > 0)
        {
            List<Action> runActions = new List<Action>();
            lock (_actions)
            {
                runActions.Clear();
                runActions.AddRange(_actions);
                _actions.Clear();
            }

            foreach (var action in runActions)
            {
                action();
            }
        }
    }
}

