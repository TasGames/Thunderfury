using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour 
{
	[SerializeField] protected float timer = 0.5f;
    [SerializeField] protected float bobbingSpeed = 0.15f;
    [SerializeField] protected float bobbingAmount = 0.15f;
    [SerializeField] protected float midpoint = 0.2f;
	[SerializeField] protected RigidbodyFirstPersonController rBFC;
	[SerializeField] protected AudioSource walkSound;
	[SerializeField] protected float minVolume;
	[SerializeField] protected float maxVolume;
	[SerializeField] protected float minPitch;
	[SerializeField] protected float maxPitch;
  
 	void Update()
    {
		if (rBFC.GetIsGrounded() == true)
		{
			float waveslice = 0.0f;
			float vertical = Input.GetAxis("Vertical");
	
			Vector3 cSharpConversion = transform.localPosition; 
	
			if (Mathf.Abs(vertical) == 0)
				timer = 0.0f;
			else 
			{
				waveslice = Mathf.Sin(timer);
				timer = timer + bobbingSpeed;

				if (timer > Mathf.PI * 2)
					timer = timer - (Mathf.PI * 2);
			}

			if (waveslice != 0) 
			{
				float translateChange = waveslice * bobbingAmount;
				float totalAxes = Mathf.Abs(vertical);
				totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
				translateChange = totalAxes * translateChange;
				cSharpConversion.y = midpoint + translateChange;
			}
			else
				cSharpConversion.y = midpoint;
	
			transform.localPosition = cSharpConversion;

			if (walkSound.isPlaying == false && vertical != 0)
			{
				walkSound.volume = Random.Range(minVolume, maxVolume);
				walkSound.pitch = Random.Range(minPitch, maxPitch);
				walkSound.Play();
			}
		}

  	}
}
