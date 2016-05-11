using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
namespace UnityLib.LCDScreen.Demo
{
    public class FadeOut : MonoBehaviour
    {
        private Text _text;
        public Text text { get { if (!_text) _text = GetComponent<Text>(); return _text; } }
        // Use this for initialization
        void Start()
        {
            text.DOFade(0f, 5f);
        }
    }
}