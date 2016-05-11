using UnityEngine;
using System.Collections;
namespace UnityLib.LCDScreen.Demo
{
    public class FocalPoint : MonoBehaviour
    {

        private Camera _cam;
        public Camera cam { get { if (!_cam) _cam = GetComponent<Camera>(); return _cam; } }
        public Transform focal;
        public LayerMask mask;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!focal)
                return;
            RaycastHit hit;
            if( Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 99999f, mask))
            {
                focal.position = hit.point;
            }

        }
    }
}