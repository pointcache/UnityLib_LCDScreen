using UnityEngine;
using System.Collections;
using System;
namespace UnityLib.LCDScreen.Demo
{
    public class MovementController : MonoBehaviour
    {
        void OnEnable()
        {
            OnRun = () => { moveSetting.run = true; };
            OnRunStop = () => { moveSetting.run = false; };
            OnJump = () => { moveSetting.jumpPressed = true; };
            OnNotJump = () =>
            {
                moveSetting.jumpPressed = false;
            };
            InputManager.VerticalAxis += y => { forwardInput = y; };
            InputManager.HorizontalAxis += x => { strafeInput = x; };
            InputManager.OnJumpDown += OnJump;
            InputManager.OnJumpUp += OnNotJump;
            InputManager.OnRunDown += OnRun;
            InputManager.OnRunUp += OnRunStop;
            InputManager.OnMovement += BaseMovement;
        }

        void OnDisable()
        {
            InputManager.VerticalAxis -= y => { forwardInput = y; };
            InputManager.HorizontalAxis -= x => { strafeInput = x; };
            InputManager.OnJumpDown -= OnJump;
            InputManager.OnJumpUp -= OnNotJump;
            InputManager.OnRunDown -= OnRun;
            InputManager.OnRunUp -= OnRunStop;
            InputManager.OnMovement -= BaseMovement;
        }

        public float forwardInput, strafeInput;

        Action OnRun;
        Action OnRunStop;
        Action OnJump;
        Action OnNotJump;

        [System.Serializable]
        public class Technicalities
        {
            public float val_multiplier;
        }

        [System.Serializable]
        public class MoveSettings
        {
            public float DefaultSpeed;
            public float walkVel;
            public float runVel;
            public float jumpVel;
            public bool run;
            public bool jumpPressed;
            //How far from the layermask ground is the player? If it is 0.1 then grounded
            public float distToGround = 0.1f;
            public LayerMask ground;
        }

        [System.Serializable]
        public class PhysicsSettings
        {
            public float defaultDownAcel = 0.4951f;
            public float downAcel = 0.4951f;
            public bool grounded;
            public bool jumped;
            public float maxSlope = 60f;
            public float terminalVelocity = 2.65f;
            public float debugFallVel;
            public float timeForTerminalVelocity;
        }

        public MoveSettings moveSetting = new MoveSettings();
        public PhysicsSettings physicsSetting = new PhysicsSettings();
        public Technicalities technicalities = new Technicalities();

        #region Camera
        //Camera
        public float RotationToHeadSpeed = 2f;
        private Transform cam_tr;
        public Camera _camera;
        #endregion

        #region Body    
        public GameObject body;
        private Vector3 velocity = Vector3.zero;
        private Rigidbody rBody;
        #endregion

        //ONLY METHODS BELOW HERE (Only dreams)
        void Awake()
        {
            physicsSetting.grounded = false;
            cam_tr = _camera.transform;
            if (GetComponent<Rigidbody>())
                rBody = GetComponent<Rigidbody>();
            else
                Debug.LogError("No rigidbody assigned to the character");
            //  Game.InternalConfig.playerCameraHeight.AddCallback(SetCameraHeight);
            //  Game.InternalConfig.playerCameraHeight.Refresh();
        }

        void FixedUpdate()
        {
            if (physicsSetting.jumped)
            {
                TestJump();
            }

            Jump();
            rBody.AddForce(body.transform.TransformDirection(velocity) * Time.deltaTime, ForceMode.Acceleration);
        }

        void LateUpdate()
        {
            TrackCamera();
        }
        void BaseMovement(bool move)
        {
            if (physicsSetting.grounded && !physicsSetting.jumped)
            {
                if (move)
                {
                    //run yes or not?
                    if (moveSetting.run)
                    {
                        moveSetting.walkVel = moveSetting.runVel * technicalities.val_multiplier;
                    }
                    else
                        moveSetting.walkVel = moveSetting.DefaultSpeed * technicalities.val_multiplier;

                    //move!
                    velocity.z = moveSetting.walkVel * forwardInput * technicalities.val_multiplier;
                    velocity.x = moveSetting.walkVel * strafeInput * technicalities.val_multiplier;
                }
            }
        }

        //If Grounded and jump is pressed, set the velocity.y to the jump velocity.
        //rigidbody.velocity is set to velocity in FixedUpdate() using the body.transform.TransformDirection
        //If you are grounded and not jumping then set the velocity to 0
        //Else if you are not grounded (falling) then apply downward acceleration to the velocity.y
        void Jump()
        {
            if (moveSetting.jumpPressed && physicsSetting.grounded)
            {
                moveSetting.jumpPressed = false;
                physicsSetting.jumped = true;
                //jump
                rBody.AddForce(transform.TransformDirection(Vector3.up) * moveSetting.jumpVel * technicalities.val_multiplier, ForceMode.Force);
                // velocity.y = moveSetting.jumpVel*technicalities.val_multiplier;
            }

            else if (!moveSetting.jumpPressed && physicsSetting.grounded)
            {
                //zero out our velocity.y
                velocity.y = 0;
                physicsSetting.jumped = false;
            }

            else
            {
                //Deprecated
                //decrease velocity.y  
                //if (velocity.y < 0) //this means as long as we are not falling down, the fallspeed wont increase towards terminal velocity
                //{
                //    //It takes roughly 15 second to reach terminal velocity for a human
                //    physicsSetting.timeForTerminalVelocity += Time.deltaTime / 15;
                //    physicsSetting.debugFallVel = Mathf.Lerp(physicsSetting.downAcel, physicsSetting.terminalVelocity, physicsSetting.timeForTerminalVelocity);
                //}
                //velocity.y -= Mathf.Lerp(physicsSetting.downAcel, physicsSetting.terminalVelocity, physicsSetting.timeForTerminalVelocity);

                velocity.y -= physicsSetting.downAcel;
            }
        }

        //Tracks the camera movement and applies the rotation to the body gameobject, turning the player
        //Rotation to Headspeed is the speed at which this occurs. A low value will create a "driving" like rotation. Might need fixing.
        void TrackCamera()
        {
            float angle = AngleSigned(body.transform.forward, cam_tr.forward, Vector3.up);
            Vector3 rot = body.transform.rotation.eulerAngles;
            rot.y = Mathf.Lerp(rot.y, rot.y + angle, Time.deltaTime * RotationToHeadSpeed);
            body.transform.rotation = Quaternion.Euler(rot);
        }

        //Collision Functions for grounded check
        void OnCollisionStay(Collision col)
        {
            //Makes you stick on walls, disabled for now
            //physicsSetting.grounded = RayCastForGround();
            foreach (ContactPoint contact in col.contacts)
            {
                if (Vector3.Angle(contact.normal, Vector3.up) < physicsSetting.maxSlope)
                    physicsSetting.grounded = true;
                physicsSetting.debugFallVel = 0;
                physicsSetting.timeForTerminalVelocity = 0;
            }
        }

        void OnCollisionExit()
        {
            physicsSetting.grounded = false;
        }

        //NOT USED RIGHT NOW BECAUSE OF COLLISION FUNCTIONS
        bool RayCastForGround()
        {
            return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGround, moveSetting.ground);
        }

        public float desiredHeight = 1.8f;
        public float empiricHeight;
        public float highestPoint;

        void TestJump()
        {
            if (rBody.velocity.y > 0)
            {
                empiricHeight = transform.position.y;
                highestPoint = transform.position.y;
            }
            else if (highestPoint < desiredHeight)
            {
                //   Debug.Break();
            }

        }

        public void SetCameraHeight(float val)
        {
            Debug.Log("Set cam height to :" + val);

            Vector3 lPos = _camera.transform.localPosition;
            lPos.y = val;
            _camera.transform.localPosition = lPos;
        }

        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                Vector3.Dot(n, Vector3.Cross(v1, v2)),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }
    }
}