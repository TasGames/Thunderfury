using UnityEngine;

public class DrawBox : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
