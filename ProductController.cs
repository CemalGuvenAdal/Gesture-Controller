using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LightBuzz.Kinect4Azure
{
    public class ProductController : MonoBehaviour
    {
        #region Varaibles
        public GameObject Text;
        public GameObject GestureManager;
        public float wristspeed;
        public float leftwristspeed;
        public List<GameObject> Products = new List<GameObject>();
        public int CurrentProductIndex = 0;
        public GameObject ActiveProduct;
        public float Torque;
        public float Threshold;
        float IsRightHandOpen, IsRightHandOpen2;
        public bool handclosed = false;
        Vector3 DirectionNormalized = Vector3.zero;

        public bool OnClickPressed = false;
        public bool OnClick = false;
        public bool wristcontrol = true;
        public bool leftwristcontrol = true;
        public float LeftX, LeftY;
        public float RightX;
        public bool OnClick2 = false;
        public float HandDistance;
        public bool IsNext, IsBack = false;
        public bool timerstart = false;
        public bool lefttimerstart = false;
        public bool handclap = false;
        public bool handclapleft = false;
        public bool handin = true;
        public bool handinleft = true;
        #endregion
        public int a = 0;
        public int b = 0;
        public int c = 0;
        public int d = 0;
        public float handcontroltimer = 0.0f;
        public float handcontroltimerleft = 0.0f;
        public bool handtimerstart = false;
        public bool handtimerstartleft = false;
        public int waitchangeproductright=0;
        public float wristtimer = 0;
        public float leftwristtimer = 0;
        public float RightChest = 0;
        public float LeftChest = 0;
        public float ychest = 0;
        public float handinittimer = 0.0f;
        public float handinittimerleft = 0.0f;
        public string txt;
        public bool handapart = false;
        public float leftwristchestxdistance = 0;
        public bool handaparttimerstart = false;
        public float handaparttimer = 0;
        public bool handapartin = true;
        public bool scaling = false;
        public bool scalingin = true;
        public bool scaletimerstart = false;
        public float scaletimer = 0;
        void Start()
        {
            Torque = 0;

        }

        void Update()
        {
            handinittimer+= Time.deltaTime;
            handinittimerleft+= Time.deltaTime;
            LeftX = GestureManager.GetComponent<Gesture>().LeftX;
            LeftY = GestureManager.GetComponent<Gesture>().LeftY;
            RightX = GestureManager.GetComponent<Gesture>().RightX;
            HandDistance = GestureManager.GetComponent<Gesture>().RightLeftHandDistance;
            IsRightHandOpen = GestureManager.GetComponent<Gesture>().IsRightHandOpen;
            IsRightHandOpen2 = GestureManager.GetComponent<Gesture>().IsRightHandOpen2;
            wristspeed = GestureManager.GetComponent<Gesture>().velocityWrist;
            leftwristspeed= GestureManager.GetComponent<Gesture>().LeftVelocityWrist;
            float DeltaRightHand = GestureManager.GetComponent<Gesture>().DeltaRightHand;
            Vector3 RightHandDirection = GestureManager.GetComponent<Gesture>().RightHandDirection;
            RightChest= GestureManager.GetComponent<Gesture>().ChestWristDistance;
            LeftChest= GestureManager.GetComponent<Gesture>().LeftChestWristDistance;
            ychest=GestureManager.GetComponent<Gesture>().ychestdistance;
            txt = Text.GetComponent<UnityEngine.UI.Text>().text;
            leftwristchestxdistance= GestureManager.GetComponent<Gesture>().chestleftwristX;
            if(!handclap && !scaling)
            {
                txt = "Rotation not selected";
                Text.GetComponent<UnityEngine.UI.Text>().text = txt;
            }
            if (wristspeed<-1.2 && wristcontrol && RightChest>0.35 && handclap==false &&!scaling)
            {
               
                StartCoroutine(ChangeProduct(true));
                wristcontrol = false;
                timerstart = true;
                Products[CurrentProductIndex].gameObject.transform.rotation = Quaternion.Euler(0, 310, 0);

            }
            
            if (timerstart)
            {
                wristtimer += Time.deltaTime;
            }
            
            if (wristtimer > 1.5)
            {
                wristcontrol = true;
                wristtimer = 0;
                timerstart = false;
            }
            if(leftwristspeed>0.85 && leftwristcontrol && leftwristchestxdistance < -0.45 && handclap==false &&!scaling )
            {
                StartCoroutine(ChangeProduct(false));
                leftwristcontrol = false;
                lefttimerstart = true;
                Products[CurrentProductIndex].gameObject.transform.rotation=Quaternion.Euler(0, 310, 0);

            }
            if (lefttimerstart)
            {
                leftwristtimer+= Time.deltaTime;
            }
            if(leftwristtimer>1.5)
            {
                leftwristcontrol = true;
                leftwristtimer = 0;
                lefttimerstart = false;
            }
            if(ychest >0.5 && handclap==false && scalingin )
            {
                scalingin = false;
                scaling =!scaling;
                scaletimerstart = true;
            }
            if(scaletimerstart)
            {
                scaletimer+= Time.deltaTime;
            }
            if (scaletimer > 0 && scaletimer<2)
            {
                scalingin = false;
                
            }
            if (scaletimer >2)
            {
                scalingin = true;
                scaletimer = 0;
                scaletimerstart = false;
            }
            if(handclap == false && scaling)
            {
                txt = "Scaling mode";
                Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                if(RightChest >0.35 && Products[CurrentProductIndex].gameObject.transform.localScale.x <8)
                {
                    Products[CurrentProductIndex].gameObject.transform.localScale+= new Vector3(+0.002f, +0.002f, +0.002f);
                }
                else if(RightChest<-0.25 && Products[CurrentProductIndex].gameObject.transform.localScale.x > 0.5)
                {
                    Products[CurrentProductIndex].gameObject.transform.localScale+=new Vector3 (-0.002f, -0.002f, -0.002f);
                }
                

            }
           if(HandDistance<0.12 &&handin && handinittimer>0.8)
            {

                handclap = !handclap;
                handtimerstart = true;
                handcontroltimer = 0.0f;
              
                if(handclap==false)
                {
                    txt = "Rotation not selected";
                    Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                }
                if(handclap && handclapleft)
                {
                    txt = "Rotation  selected: Y axis";
                    Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                }
                if (handclap && handclapleft==false)
                {
                    txt = "Rotation  selected: X axis";
                    Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                }

            }
           if(handtimerstart)
            {
            handcontroltimer+= Time.deltaTime;
            }
           
           if(handcontroltimer<2 && handcontroltimer>0)
            {
                handin = false;
            }
            else if(handcontroltimer>2)
            {
                handin = true;
                
            }

            //xxxxxxxx
            if (LeftChest>0.5 && handinleft && handinittimerleft > 0.8 && handclap && !handapart)
            {

                handclapleft = !handclapleft;
                handtimerstartleft = true;
                handcontroltimerleft = 0.0f;
                if(handclapleft)
                {
                    txt = "Rotation Selected Y axis";
                    Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                }
                if(handclapleft==false)
                {
                    txt = "Rotation Selected X axis";
                    Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                }

            }
            if (handtimerstartleft)
            {
                handcontroltimerleft += Time.deltaTime;
            }

            if (handcontroltimerleft < 2 && handcontroltimerleft > 0)
            {
                handinleft = false;
            }
            else if (handcontroltimer > 2)
            {
                handinleft = true;

            }


            if (handclap && RightChest>0.2 &&handclapleft==false && !handapart )
            {
                txt= txt = "Rotation Selected X axis";
                if (a == 0)
                {

                   
                    //Products[CurrentProductIndex].gameObject.GetComponent<Transform>().Rotate(new Vector3(0, Torque * DirectionNormalized.x, 0));
                    Products[CurrentProductIndex].gameObject.transform.Rotate(0.0f, -3.0f, 0.0f, Space.Self);
                }
                a = a + 1;
                if(a==10)
                {
                    a = 0;
                }


           }
            if (handclap && RightChest <- 0.1 && handclapleft==false && !handapart)
            {
                txt = "Rotation Selected X axis";
                if (b == 0)
                {

                    //Products[CurrentProductIndex].gameObject.GetComponent<Transform>().Rotate(new Vector3(0, Torque * DirectionNormalized.x, 0));
                    Products[CurrentProductIndex].gameObject.transform.Rotate(0.0f, +3.0f, 0.0f, Space.Self);
                }
                b = b + 1;
                if (b == 10)
                {
                    b= 0;
                }


            }
            if (handclap && ychest < -0.2 && handclapleft && !handapart)
            {
                txt = "Rotation Selected Y axis";
                if (c == 0)
                {

                    //Products[CurrentProductIndex].gameObject.GetComponent<Transform>().Rotate(new Vector3(0, Torque * DirectionNormalized.x, 0));
                    Products[CurrentProductIndex].gameObject.transform.Rotate(+3.0f, 0.0f, +0.0f, Space.Self);
                }
                c = c + 1;
                if (c == 10)
                {
                    c = 0;
                }
            

            }

            if (handclap && ychest >0.2 && handclapleft && !handapart)
            {
                txt = "Rotation Selected Y axis";
                if (d == 0)
                {

                    
                    //Products[CurrentProductIndex].gameObject.GetComponent<Transform>().Rotate(new Vector3(0, Torque * DirectionNormalized.x, 0));
                    Products[CurrentProductIndex].gameObject.transform.Rotate(-3.0f, 0.0f, +0.0f, Space.Self);
                }
                d = d + 1;
                if (d == 10)
                {
                    d = 0;
                }


            }
            if (RightChest > 0.45 && leftwristchestxdistance < -0.45 && handclap && handapartin )
            {
                handapart = !handapart;
                handaparttimerstart = true;
                handaparttimer = 0.0f;
                if (handclap && handapart)
                {
                    txt = "Parts are shown";
                    Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                }
                else if(handclap && handapart==false)
                {
                    txt = "Rotation mode";
                    Text.GetComponent<UnityEngine.UI.Text>().text = txt;
                }
            }
            if (handaparttimerstart)
            {
                handaparttimer += Time.deltaTime;
            }

            if (handaparttimer < 2 && handaparttimer > 0)
            {
                handapartin = false;
            }
            else if (handaparttimer > 2)
            {
                handapartin = true;

            }
            

        }

        //&& IsRightHandOpen2<0.085

        #region Custom Method
        public void OrbitControl()
        {
            if (IsRightHandOpen < 0.17)
            {
                //Products[CurrentProductIndex].gameObject.GetComponent<Transform>().Rotate(new Vector3(0, Torque * DirectionNormalized.x, 0));
            }

        }

        public IEnumerator ChangeProduct(bool IsRight)
        {
            yield return new WaitForSeconds(0.1f);
            if (IsRight == true && OnClick == false)
            {
                CurrentProductIndex++;
            }
            if (IsRight == false && OnClick == false)
            {
                CurrentProductIndex--;
            }

            ActiveProduct.gameObject.SetActive(false);

            if (CurrentProductIndex >= Products.Count)
            {
                CurrentProductIndex = 0;
            }
            if (CurrentProductIndex == -1)
            {
                CurrentProductIndex = Products.Count - 1;
            }

            ActiveProduct = Products[CurrentProductIndex];
            ActiveProduct.gameObject.SetActive(true);
            OnClick2 = false;
        }



        void Selected()
        {

        }

    }
    #endregion
}