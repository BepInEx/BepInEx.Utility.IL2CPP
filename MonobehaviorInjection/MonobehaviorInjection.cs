using System;
//using System.Diagnostics;
using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;


namespace MonobehaviorInjection
{
    [BepInPlugin(nameof(MonobehaviorInjection), nameof(MonobehaviorInjection), Version)]
    public class MonobehaviorInjection : BasePlugin
    {
        public const string Version = "0.5";
        public override void Load()
        {
            Debug.Log("MonobehaviorInjection Load");
            //IL2CPP don't automatically inherits Monobehavior, so needs to add separatelly
            AddComponent<MyMonobehavior>();
        }
    }

    public class MyMonobehavior : MonoBehaviour
    {
        //Got this from BepInEx Discord pinned messages
        public MyMonobehavior(IntPtr handle) : base(handle) { }

        bool update = true;
        bool fixedUpdate = true;
        bool lateUpdate = true;

        private void Awake()
        {
            Debug.Log("MyMonobehavior Awake");
        }

        private void Start()
        {
            Debug.Log("MyMonobehavior Start");
        }

        private void Update()
        {
            if (update)
            {
                update = false;
                Debug.Log("MyMonobehavior Update");
            }
        }

        private void FixedUpdate()
        {
            if (fixedUpdate)
            {
                fixedUpdate = false;
                Debug.Log("MyMonobehavior FixedUpdate");
            }
        }

        private void LateUpdate()
        {
            if (lateUpdate)
            {
                lateUpdate = false;
                Debug.Log("MyMonobehavior LateUpdate");
            }
        }
    }
}
