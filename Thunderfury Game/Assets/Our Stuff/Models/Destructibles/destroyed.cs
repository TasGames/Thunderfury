using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyed : MonoBehaviour {

    public GameObject destroyedversion;

    void OnMouseDown ()
    {
        Instantiate(destroyedversion, this.transform.position, this.transform.rotation);
        Destroy(gameObject);

    }
}
