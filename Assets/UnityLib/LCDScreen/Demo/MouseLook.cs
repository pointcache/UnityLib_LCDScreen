using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityLib.LCDScreen.Demo
{
    public class MouseLook : MonoBehaviour
    {
        public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        public RotationAxes axes = RotationAxes.MouseXAndY;
        public float sensitivity = 1F;

        public float minimumX = -360F;
        public float maximumX = 360F;
        public float minimumY = -70F;
        public float maximumY = 70F;
        float rotationX = 0F;
        float rotationY = 0F;
        public float SensitivityCompensation = 100f;
        Quaternion originalRotation;

        float lerpTime = 1f;
        float currentLerpTime;

        public CursorLockMode cursorLockMode;
        bool work;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)){
                work = !work;
				
            }
            if (work)
            {
                Cursor.lockState = cursorLockMode;
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
				Debug.Break();
                EditorApplication.isPlaying = false;
                Application.Quit();
            }
            if (!work)
            {
                return;
            }
            if (currentLerpTime > 1f)
            {
                currentLerpTime = 0f;
            }
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;


            if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * sensitivity;
                rotationY += Input.GetAxis("Mouse Y") * sensitivity;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                Quaternion newRotation = originalRotation * xQuaternion * yQuaternion;

                transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, perc * 1 / SensitivityCompensation);


            }
            else if (axes == RotationAxes.MouseX)
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivity;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                transform.localRotation = originalRotation * xQuaternion;
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivity;
                rotationY = ClampAngle(rotationY, minimumY, maximumY);
                Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
                transform.localRotation = originalRotation * yQuaternion;
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        void Start()
        {
            originalRotation = transform.localRotation;
        }
    }
}