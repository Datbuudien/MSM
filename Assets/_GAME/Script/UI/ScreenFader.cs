// using System;
// using System.Collections;
// using UnityEngine;
// public class ScreenFader : Singleton<ScreenFader>
// {
//     [SerializeField] private CanvasGroup cg;
//     [SerializeField] private float fadeDuration =.2f;
//     [SerializeField] private float hold = 1f;
//     private bool isRunning;
//     protected override void Awake()
//     {
//         base.Awake();
//         if(IsDuplicate) return;
//         cg.alpha=1f;
//         cg.blocksRaycasts=true;
//     }
//     public void Fade(Action onHalfway,Action onComplete = null)
//     {
        
//     }
//     private IEnumerator CoFade(Action onHalfway,Action onComplete)
//     {
//         isRunning = true;
//         try
//         {
            
//         }
//     }
// }