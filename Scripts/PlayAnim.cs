
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.PosterUI
{
    public class PlayAnim : UdonSharpBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] string boolName = "Show";
        public void Show() => Show(boolName);
        public void Show(string _boolName) => animator.SetBool(_boolName, true);
        public void Hide() => Hide(boolName);
        public void Hide(string _boolName) => animator.SetBool(_boolName, false);
    }
}
