using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace UnityLib.LCDScreen.Demo
{
    public class ShowTooltip : MonoBehaviour
    {
        public float Delay, ActivationTime;
        public Ease easing;
        public GameObject textRoot;
        Vector3 initScale;
        bool isShowing;
        void Start()
        {
            initScale = textRoot.transform.localScale;
            textRoot.transform.localScale = new Vector3(0f, initScale.y, initScale.z);
            textRoot.SetActive(false);
        }
        public void Show()
        {
            if (isShowing)
                return;
            textRoot.SetActive(true);
            isShowing = true;
            textRoot.transform.DOScaleX(initScale.x, ActivationTime).SetEase(easing).OnComplete(() =>
            {
                Invoke("StopShowing", Delay);
            });
        }

        void StopShowing()
        {
            textRoot.transform.DOScaleX(0f, .2f).SetEase(Ease.InQuad).OnComplete(() => {
                textRoot.SetActive(false);
                isShowing = false;
            });
           
        }

        void OnTriggerEnter()
        {
            Show();
        }
    }
}