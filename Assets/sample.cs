using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;
using Valve.VR.InteractionSystem;

public class sample : MonoBehaviour
{   

    public SteamVR_ActionSet myAction; 
    
    public SteamVR_Action_Vector2 RightStick;
    public SteamVR_Action_Vector2 LeftStick;

    public SteamVR_Action_Boolean RightGrip;
    public GameObject playerCamera;

    public GameObject playerObject;

    public GameObject RightHand;    

    public float distanceOfCanGetItem;

    private List<string> grabedObjects = new List<string>();

    // setting
    public float playerSpeed;
    public float rotationSpeed;


    // Start is called before the first frame update
    void Start()
    {
        myAction.Activate();
    }    
    private void Update()
    {
        Transform playerTransform = playerCamera.transform;

        /*
         * 
         * 右スティック: snap turn
         * 
         * -----------------------------------------------*/

        Vector2 stickVector = RightStick.GetAxis(SteamVR_Input_Sources.RightHand);

        playerTransform.Rotate (0.0f, 1.0f * stickVector.x * rotationSpeed, 0.0f);

        /*
         * 
         * 左スティック: 移動
         * 
         * -----------------------------------------------*/

        Vector2 leftStickVector = LeftStick.GetAxis(SteamVR_Input_Sources.LeftHand);
        float rad = Mathf.Atan2(leftStickVector.y, leftStickVector.x);
        float leftStickDistance = Vector2.Distance(leftStickVector, Vector2.zero);
        
        //rotationの前方を取得(radian)
        float forwordDirectionRad = playerTransform.localEulerAngles.y * Mathf.Deg2Rad;

        //移動パターン（歩く、走るなど）
        float moveSpeedPattern = 0.0f;

        if ( leftStickDistance < 0.5f && leftStickDistance > 0.05f){
            moveSpeedPattern = 0.5f;
        }

        if ( leftStickDistance <= 1.0f && leftStickDistance > 0.5f){
            moveSpeedPattern = 1.0f;
        }

        playerTransform.position += 
        playerSpeed * moveSpeedPattern * new Vector3(Mathf.Cos(rad - forwordDirectionRad), 0 , Mathf.Sin(rad - forwordDirectionRad));
    

        /*
         *
         *　右グリップ：掴み処理
         *
         * -----------------------------------------------*/

        if (RightGrip.GetStateDown(SteamVR_Input_Sources.RightHand)){

            List<string> grabbableObjects = RightHand.GetComponent<colliderTest>().getGrabbableObjects();

            if (grabbableObjects.Count > 0){

                foreach(string objectName in grabbableObjects){

                    if (GameObject.Find(objectName).tag == "grabbable"){
                        GameObject.Find(objectName).transform.SetParent(RightHand.transform);
                        GameObject.Find(objectName).GetComponent<Rigidbody>().isKinematic = true;
                        grabedObjects.Add(objectName);
                    }
                }
            }
        }
             
        if (RightGrip.GetStateUp(SteamVR_Input_Sources.RightHand)){
            
            if (grabedObjects.Count > 0){

                foreach(string objectName in grabedObjects){
                    Debug.Log(objectName);
                    GameObject.Find(objectName).transform.parent = null;
                    GameObject.Find(objectName).GetComponent<Rigidbody>().isKinematic = false;
                    GameObject.Find(objectName).GetComponent<Rigidbody>().AddForce(transform.forward * RightHand.GetComponent<VelocityEstimator>().GetVelocityEstimate().magnitude, ForceMode.VelocityChange);
                    Debug.Log(objectName + "is released");
                }
                grabedObjects.Clear();            }
        }
        

        /* default action setを利用した際のsnap turn
        // if (snapTurnLeft.GetState(SteamVR_Input_Sources.RightHand)){
        //     Debug.Log("rotateLeft");
        //     playerCam.Rotate (0.0f, 1.0f, 0.0f);
        // }

        // if (snapTurnRight.GetState(SteamVR_Input_Sources.RightHand)){
        //     Debug.Log("rotateRight");
        //     playerCam.Rotate (0.0f, -1.0f, 0.0f);
        // }
        */
    }

    // playerから距離distanceOfCanGetItem以内にいる一番近い武器を取得
    protected GameObject GetNearestWeaponInRange()
    {
        float tmpDis = 0;
        float nearestDistance = distanceOfCanGetItem;
        GameObject targetObj = null;
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("WeaponOnGround"))
        {
            tmpDis = Vector3.Distance(obs.transform.position, playerObject.transform.position);

            if (nearestDistance > tmpDis)
            {
                nearestDistance = tmpDis;
                targetObj = obs;
            }
        }
        return targetObj;
    }

    private Collider GetCollider(GameObject gameObject){
        Collider collider = gameObject.GetComponent<Collider>();
        return collider;
    }
}
