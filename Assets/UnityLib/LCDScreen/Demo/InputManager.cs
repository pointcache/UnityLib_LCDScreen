using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
namespace UnityLib.LCDScreen.Demo
{
    public class InputManager : MonoBehaviour
    {

        public static event Action<Vector2> OnMouseLeftDown;
        public static event Action<Vector2> OnMouseLeftUp;
        public static event Action<Vector2> OnMouseLeft;

        public static event Action OnForward;
        public static event Action OnStrafeleft;
        public static event Action OnStraferight;
        public static event Action OnBackward;
        public static event Action OnJump;
        public static event Action OnJumpUp;
        public static event Action OnJumpDown;
        public static event Action OnAnyKey;
        public static event Action OnRun;
        public static event Action OnRunDown;
        public static event Action OnRunUp;

        public static event Action<bool> OnMovement;

        public static event Action<float> HorizontalAxis, VerticalAxis;

        private Vector2 lastClick;
        private int PressCount, MovementCount;
        public Options options;

        [System.Serializable]
        public class Options
        {
            public float AxisDeadzone;
            public bool UseMainCameraForRaycast;
            public Camera raycastCam;
            public KeyCode forward;
            public KeyCode strafeleft;
            public KeyCode straferight;
            public KeyCode backward;
            public KeyCode jump;
            public KeyCode run;
            public string s_Horizontal;
            public string s_Vertical;
        }

        public static InputManager New(Options opts)
        {
            GameObject go = new GameObject("InputManager");
            InputManager component = go.AddComponent<InputManager>();
            component.options = opts;
            return component;
        }

        // Update is called once per frame
        void Update()
        {
            //Mouse Input

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (OnMouseLeft != null)
                    OnMouseLeft(CastRayFromCamera());
                PressCount++;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (OnMouseLeftDown != null)
                    OnMouseLeftDown(CastRayFromCamera());
                PressCount++;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (OnMouseLeftUp != null)
                    OnMouseLeftUp(CastRayFromCamera());
                PressCount++;
            }
            //Axis Inputs

            //Horizontal Axis
            if (HorizontalAxis != null)
            {
                float HorAx = Input.GetAxis(options.s_Horizontal);
                if (HorAx > options.AxisDeadzone)
                {
                    HorizontalAxis(HorAx);
                    MovementCount++;
                }
                else
                {
                    HorizontalAxis(0f);
                }
            }

            if (VerticalAxis != null)
            {
                //Vertical Axis
                float VerAx = Input.GetAxis(options.s_Vertical);
                if (VerAx > options.AxisDeadzone)
                {
                    VerticalAxis(VerAx);
                    MovementCount++;
                }
                else
                {
                    VerticalAxis(0f);
                }
            }

            //Movement Input

            if (Input.GetKey(options.forward))
            {
                if (OnForward != null)
                    OnForward.Invoke();

                if (VerticalAxis != null)
                    VerticalAxis(1);


                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.backward))
            {
                if (OnBackward != null)
                    OnBackward.Invoke();


                if (VerticalAxis != null)
                    VerticalAxis(-1);

                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.straferight))
            {
                if (OnStraferight != null)
                    OnStraferight.Invoke();

                if (HorizontalAxis != null)
                    HorizontalAxis(1);

                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.strafeleft))
            {
                if (OnStrafeleft != null)
                    OnStrafeleft.Invoke();

                if (HorizontalAxis != null)
                    HorizontalAxis(-1);

                PressCount++; MovementCount++;
            }

            if (Input.GetKey(options.jump))
            {
                OnJump.NullCheckInvoke();
                PressCount++;
            }
            if (Input.GetKeyDown(options.jump))
            {
                OnJumpDown.NullCheckInvoke();
                PressCount++;
            }

            if (Input.GetKeyUp(options.jump))
            {
                OnJumpUp.NullCheckInvoke();
            }

            if (Input.GetKey(options.run))
            {
                OnRun.NullCheckInvoke();
                PressCount++;
            }

            if (Input.GetKeyDown(options.run))
            {
                OnRunDown.NullCheckInvoke();
                PressCount++;
            }

            if (Input.GetKeyUp(options.run))
            {
                OnRunUp.NullCheckInvoke();
            }


            if (OnMovement != null)
            {
                if (MovementCount > 0)
                {
                    OnMovement(true);
                }
                else
                {
                    OnMovement(false);
                }
            }

            if (PressCount > 0)
            {
                OnAnyKey.NullCheckInvoke();
                PressCount = 0;
            }
        }


        Vector2 CastRayFromCamera()
        {
            Vector3 point3;
            if (options.UseMainCameraForRaycast)
            {
                point3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                point3 = options.raycastCam.ScreenToWorldPoint(Input.mousePosition);
            }

            Vector2 point = new Vector2(point3.x, point3.y);
            lastClick = point;
            return point;
        }
    }
    public static class Extensions
    {

        public static void NullCheckInvoke(this Action del)
        {
            if (del != null)
            {
                del();
            }
        }
    }

}