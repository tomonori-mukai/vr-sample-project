using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRTest : MonoBehaviour
{   
    // actionにMenuを追加してコメントアウト
    // private SteamVR_Action_Boolean actionToHaptic = SteamVR_Actions._default.InteractUI;
    private SteamVR_Action_Boolean actionToHaptic = SteamVR_Actions._default.Menu;
    private SteamVR_Action_Vibration haptic = SteamVR_Actions._default.Haptic;
    private SteamVR_Action_Boolean snapTurnLeft = SteamVR_Actions._default.SnapTurnLeft;
    private SteamVR_Action_Boolean snapTurnRight = SteamVR_Actions._default.SnapTurnRight;

    private GameObject cameraRig;

    // Start is called before the first frame update
    void Start()
    {
        
    }    
    private void Update()
    {
        if (actionToHaptic.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
            haptic.Execute(0, 0.005f, 0.005f, 1, SteamVR_Input_Sources.LeftHand);
        }

        if (snapTurnLeft != null){
            cameraRig.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
    }
}
