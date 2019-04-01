using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
 {
	protected Vector3 startPos;
	protected Vector3 endPos;
	protected float travelTime;
	protected float timer;
	
	void Update() 
	{
		timer += Time.deltaTime;
		transform.position = Vector3.Lerp(startPos, endPos, timer / travelTime);
		if (timer >= travelTime)
			Destroy(gameObject);
	}

	public void SetValues(Vector3 start, Vector3 end, float duration)
	{
		startPos = start;
		endPos = end;
		travelTime = duration;
		timer = 0;
	}
}
