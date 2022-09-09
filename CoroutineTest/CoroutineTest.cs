using BepInEx;
using System.Collections;
using UnityEngine;

namespace CoroutineTest
{
    [BepInPlugin("test", "test", "0.1")]
    public class CoroutineTest : BaseUnityPlugin
    {
        private void Awake()
        {
            StartCoroutine(TestScreen());
        }
        private IEnumerator TestScreen()
        {
            while(true)
            {
                Logger.LogInfo("Before yield Return");
                yield return new WaitForSecondsRealtime(1f);
                Logger.LogInfo("After yield Return");
            }
        }
    }
}
