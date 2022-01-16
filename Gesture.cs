using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightBuzz.Kinect4Azure
{
    public class Gesture : MonoBehaviour
    {
        #region Varaibles
        private KinectSensor _sensor;
        public Vector2 Hand2DtcLeft;
        public Vector2 Hand2DtcRight;
        public Vector3 Hand3DtcRight;
        public Vector3 Hand3DtcLeft;
        public Vector3 ElbowLeft;
        public Vector3 ElbowRight;

        public Vector3 HandTipRight;
        public Vector3 WristRight;
        public Vector3 ThumbRight;
        public float RightLeftHandDistance;
        public float timer = 0.0f;
        public float velocityWrist;
        public float init_time = 0.0f;
        public float Left_init_time = 0.0f;
        public float IsRightHandOpen, IsRightHandOpen2;
        public float LeftVelocityWrist;
        public float lefttimer = 0.0f;
        Vector3 PrevRightHand, PrevLeftHand = Vector3.zero;
        public float ChestWristDistance;
        public float DeltaRightHand;
        public float LeftChestWristDistance;
        public float RightX, RightY, RightZ;
        public float LeftX, LeftY, LeftZ;
        public float init_timer = 0.0f;
        public Vector3 RightHandDirection;
        public Vector3 LeftHandDirection;
        public Vector3 LeftArmDirection;
        public Vector3 LeftArmDirection1;
        public Vector3 WristRightprev;
        public Vector3 WristLeft;
        public Vector3 WristLeftprev;
        public Vector3 chest;
        public bool handclosed;
        public float handdegree;
        public Vector3 a;
        public Vector3 b;
        public float dot_product;
        public float gnitude;
        public Vector4D fingeror;
        public float ychestdistance;
        public LightBuzz.Kinect4Azure.TrackingState fingervis;

        public float chestrightwristX;
        public float chestleftwristX;
        #endregion

        private void Start()
        {
            _sensor = KinectSensor.GetDefault();
            _sensor.Open();
        }

        private void Update()
        {
            if (_sensor == null || !_sensor.IsOpen) return;

            Frame frame = _sensor.Update();

            if (frame != null)
            {
                if (frame.BodyFrameSource != null)
                {
                    Body body = frame.BodyFrameSource.Bodies.Closest();

                    if (body != null)
                    {
                        //RightHand
                        Hand2DtcRight = body.Joints[JointType.HandRight].PositionColor;
                        Hand3DtcRight = body.Joints[JointType.HandRight].Position;
                        timer += Time.deltaTime;
                        lefttimer+= Time.deltaTime;
                        
                        RightX = Hand3DtcRight.x;
                        RightY = Hand3DtcRight.y;
                        RightZ = Hand3DtcRight.z;
                        //Chest
                        chest = body.Joints[JointType.SpineChest].Position;
                        //RightHandFinger
                        HandTipRight = body.Joints[JointType.HandtipRight].Position;
                        WristRight = body.Joints[JointType.WristRight].Position;
                        ThumbRight = body.Joints[JointType.ThumbRight].Position;

                        //LeftHand
                        Hand2DtcLeft = body.Joints[JointType.HandLeft].PositionColor;
                        Hand3DtcLeft = body.Joints[JointType.HandLeft].Position;
                        ElbowLeft = body.Joints[JointType.ElbowLeft].Position;
                        WristLeft = body.Joints[JointType.WristLeft].Position;
                        ElbowRight = body.Joints[JointType.ElbowRight].Position;

                        LeftX = Hand3DtcLeft.x;
                        LeftY = Hand3DtcLeft.y;
                        LeftZ = Hand3DtcLeft.z;

                        RightLeftHandDistance = Vector3.Distance(Hand3DtcRight, Hand3DtcLeft);

                        IsRightHandOpen = Vector3.Distance(HandTipRight, WristRight);
                        IsRightHandOpen2 = Vector3.Distance(ThumbRight, WristRight);
                        DeltaRightHand = Vector3.Distance(Hand3DtcRight, PrevRightHand);
                        RightHandDirection = Hand3DtcRight - PrevRightHand;
                        
                        LeftChestWristDistance = (chest.y-WristLeft.y);
                        ychestdistance = (chest.y - WristRight.y);

                        ChestWristDistance = (chest.x - WristRight.x);
                        chestleftwristX = (chest.x - WristLeft.x);
                     

                        //additional variables
                        // Left Arm

                        LeftArmDirection1 =(WristLeft - ElbowLeft);
                        LeftArmDirection = LeftArmDirection1.normalized;

                        a = ElbowRight - Hand3DtcRight;
                        b = HandTipRight - Hand3DtcRight;

                        dot_product = Vector3.Dot(a, b);
                        gnitude = Vector3.Magnitude(a) * Vector3.Magnitude(b);
                        
                        handdegree = (Mathf.Acos(dot_product/gnitude)/Mathf.PI)*180;

                        if (timer<0.01)
                        {
                            WristRightprev= body.Joints[JointType.WristRight].Position;
                            init_time = timer;
                        }

                        if (timer>0.1)
                        {
                            velocityWrist = (WristRight.x-WristRightprev.x)/(timer-init_timer);
                            timer = 0;
                        }
                        if(lefttimer<0.01)
                        {
                           WristLeftprev= body.Joints[JointType.WristLeft].Position;
                            Left_init_time = lefttimer;
                        }
                        if(lefttimer>0.1)
                        {
                            LeftVelocityWrist = (WristLeft.x - WristLeftprev.x) / (lefttimer - Left_init_time);
                            lefttimer = 0;
                        }

                    }
                }
            }
        }

        private void OnDestroy()
        {
            _sensor?.Close();
        }

       
    }
}