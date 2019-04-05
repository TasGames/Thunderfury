using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRagdoll : MonoBehaviour
{

    public Transform pelvis;
    public Transform Lthigh;
    public Transform Lcalf;
    public Transform Rthigh;
    public Transform Rcalf;
    public Transform spine1;
    public Transform head;
    public Transform Lupperarm;
    public Transform Lforearm;
    public Transform Rupperarm;
    public Transform Rforearm;
    public Vector3 pelvisPosition;
    public Vector3 LthighPosition;
    public Vector3 LcalfPosition;
    public Vector3 RthighPosition;
    public Vector3 RcalfPosition;
    public Vector3 spine1Position;
    public Vector3 headPosition;
    public Vector3 LupperarmPosition;
    public Vector3 LforearmPosition;
    public Vector3 RupperarmPosition;
    public Vector3 RforearmPosition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ResetDoll()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("herpaderp");
        this.gameObject.SetActive(false);
        ReturnChildPositions();
    }

    public void GetChildPositions()
    {
        pelvisPosition = pelvis.position;
        LthighPosition = Lthigh.position;
        LcalfPosition = Lcalf.position;
        RthighPosition = Rthigh.position;
        RcalfPosition = Rcalf.position;
        spine1Position = spine1.position;
        headPosition = head.position;
        LupperarmPosition = Lupperarm.position;
        LforearmPosition = Lforearm.position;
        RupperarmPosition = Rupperarm.position;
        RforearmPosition = Rforearm.position;
    }

    public void ReturnChildPositions()
    {
        pelvis.position = pelvisPosition;
        Lthigh.position = LthighPosition;
        Lcalf.position = LcalfPosition;
        Rthigh.position = RthighPosition;
        Rcalf.position = RcalfPosition;
        spine1.position = spine1Position;
        head.position = headPosition;
        Lupperarm.position = LupperarmPosition;
        Lforearm.position = LforearmPosition;
        Rupperarm.position = RupperarmPosition;
        Rforearm.position = RforearmPosition;
    }
}
