using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandPose;
    public HandData leftHandPose;

    private Vector3 startHandPos;
    private Vector3 finalHandPos;
    private Quaternion startHandRot;
    private Quaternion finalHandRot;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    private Vector3[] startingFingerPos;
    private Vector3[] finalFingerPos;

    public GameObject localRHandModel, localLHandModel;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);

    }

    private void Update()
    {
        localRHandModel = GameObject.FindGameObjectWithTag("LocalRHandPlayer");
        localLHandModel = GameObject.FindGameObjectWithTag("LocalLHandPlayer");
    }

    public void SetupPose(BaseInteractionEventArgs args)
    {
        if(args.interactorObject is XRDirectInteractor)
        {
            if (args.interactorObject.transform.CompareTag("RightHand"))
            {
                HandData handData = localRHandModel.transform.GetComponentInChildren<HandData>();
                handData.animator.enabled = false;

                SetHandDataValues(handData, rightHandPose);
                SetHandData(handData, finalHandPos, finalHandRot, finalFingerRotations, finalFingerPos);
            }else if (args.interactorObject.transform.CompareTag("LeftHand"))
            {
                HandData handData = localLHandModel.transform.GetComponentInChildren<HandData>();
                handData.animator.enabled = false;

                SetHandDataValues(handData, leftHandPose);
                SetHandData(handData, finalHandPos, finalHandRot, finalFingerRotations, finalFingerPos);
            }
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startHandPos = h1.root.localPosition;
        finalHandPos = h2.root.localPosition;

        startHandRot = h1.root.localRotation;
        finalHandRot = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        startingFingerPos = new Vector3[h1.fingerBones.Length];

        finalFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerPos = new Vector3[h1.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            startingFingerPos[i] = h1.fingerBones[i].localPosition;

            finalFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerPos[i] = h2.fingerBones[i].localPosition;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition,Quaternion newRotation, Quaternion [] newBonesRotation, Vector3[] newBonesPosition)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
            h.fingerBones[i].localPosition = newBonesPosition[i];
        }
    }
}
