using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HingeJointListener : MonoBehaviour
{
    public enum HingeJointState { Min, Max, None};
    public HingeJointState hingeJointState = HingeJointState.None;

    private HingeJoint hinge;

    public float angleThreshold = 1f;

    public UnityEvent OnMinLimit;
    public UnityEvent OnMaxLimit;
    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float angleWithMinLimit = Mathf.Abs(hinge.angle - hinge.limits.min);
        float angleWithMaxLimit = Mathf.Abs(hinge.angle - hinge.limits.max);

        if (angleWithMinLimit < angleThreshold)
        {
            if(hingeJointState != HingeJointState.Min)
            {
                OnMinLimit.Invoke();
            }

            hingeJointState = HingeJointState.Min;
        }

        else if (angleWithMaxLimit < angleThreshold)
        {
            if (hingeJointState != HingeJointState.Max)
            {
                OnMaxLimit.Invoke();
            }

            hingeJointState = HingeJointState.Max;
        }
        else
        {
            hingeJointState = HingeJointState.None;
        }
    }
}
