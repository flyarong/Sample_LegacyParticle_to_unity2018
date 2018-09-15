/*
 *	@file	ParticleControler
 *	@attention	None
 *	@note		None
 */
#define UseParticleSystem

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Linq; // need LINQ/Lambda


public class ParticleControler : MonoBehaviour {

#if (UseParticleSystem)	// for Perticle System
	ParticleSystem ps_FX;
	Gradient[] grad = new Gradient[2];
#else					// for Legacy Perticle
	ParticleEmitter pe_FX = null;
	ParticleAnimator pa_FX = null;
	Color[,] ColorsAnim = new Color[2, 5];
#endif


	private void Start()
	{
#if (UseParticleSystem) // for Perticle System
		ps_FX = GetComponent<ParticleSystem>();

		grad[0] = new Gradient();
		grad[0].SetKeys( // Color white, no alpha gradietion. 
			new GradientColorKey[] { // RGB指定。 
						new GradientColorKey(Color.white, 1.0f),
						new GradientColorKey(Color.white, 1.0f),
						new GradientColorKey(Color.white, 1.0f),
						new GradientColorKey(Color.white, 1.0f),
						new GradientColorKey(Color.white, 1.0f)
			},
			new GradientAlphaKey[] { // ALPHAグラデーション指定。 
						new GradientAlphaKey(  255 / 255, 0.0f), // Start
						new GradientAlphaKey(  255 / 255, 1.0f)  // End
			}
			);
		grad[1] = new Gradient();
		grad[1].SetKeys( // Color red, alpha gradietion. 
			new GradientColorKey[] { // RGB指定。 
						new GradientColorKey(new Vector4(255,  13,  13) / 255, 0f),    // Start
						new GradientColorKey(new Vector4(176,  86, 255) / 255, 1f/4f), // 1/4 lapse time.
						new GradientColorKey(new Vector4(255,   2, 185) / 255, 2f/4f), // 2/4 lapse time.
						new GradientColorKey(new Vector4(255,   0,   0) / 255, 3f/4f), // 3/4 lapse time.
						new GradientColorKey(new Vector4(255,  28, 202) / 255, 1f)     // End
			},
			new GradientAlphaKey[] { // ALPHAグラデーション指定。 
						new GradientAlphaKey(  0 / 255, 0f),
						new GradientAlphaKey(255 / 255, 1f/4f),
						new GradientAlphaKey( 16 / 255, 2f/4f),
						new GradientAlphaKey( 13 / 255, 3f/4f),
						new GradientAlphaKey(  0 / 255, 1f)
			}
			);
#else	// for Legacy Perticle
		pe_FX = GetComponent<ParticleEmitter>();
		pa_FX = GetComponent<ParticleAnimator>();
		ColorsAnim[0, 0] = Color.white;
		ColorsAnim[0, 1] = Color.white;
		ColorsAnim[0, 2] = Color.white;
		ColorsAnim[0, 3] = Color.white;
		ColorsAnim[0, 4] = Color.white;

		ColorsAnim[1, 0] = new Vector4(255, 13, 13, 0) / 255;
		ColorsAnim[1, 1] = new Vector4(176, 86, 255, 255) / 255;
		ColorsAnim[1, 2] = new Vector4(255, 2, 185, 16) / 255;
		ColorsAnim[1, 3] = new Vector4(255, 0, 0, 13) / 255;
		ColorsAnim[1, 4] = new Vector4(255, 28, 202, 0) / 255;
#endif	// for Legacy Perticle
	}
	void Set_FX(bool sw, int colorPattern)
	{
#if (UseParticleSystem)        // for Perticle System
		if (ps_FX != null)
		{
			if (sw == true)
			{
				var col = ps_FX.colorOverLifetime;
				col.enabled = true; // 一応ONにしておく。 
				col.color = grad[colorPattern];
				ps_FX.Play();
			}
			else
			{
				ps_FX.Stop();
			}
		}

#else							// for Legacy Perticle
		if (pe_FX != null && pa_FX != null)
		{
			if (sw == false)
			{
				pe_FX.emit = sw;
				return;
			}
			else
			{
				Color[] colors = pa_FX.colorAnimation;
				colors[0] = ColorsAnim[colorPattern, 0];
				colors[1] = ColorsAnim[colorPattern, 1];
				colors[2] = ColorsAnim[colorPattern, 2];
				colors[3] = ColorsAnim[colorPattern, 3];
				colors[4] = ColorsAnim[colorPattern, 4];
				pa_FX.colorAnimation = colors;
				pe_FX.emit = sw;
			}
		}
#endif
	}

	float lapTime;
	int mode = 0;
	void Update () {
		lapTime += Time.deltaTime;
		if (lapTime > 3)
		{
			lapTime = 0;
			mode++;
			mode = mode % 3;

			switch (mode) {
				case 0:
					Set_FX(false, 0); // OFF
					break;
				case 1:
					Set_FX(true, 0); // WHITE
					break;
				case 2:
					Set_FX(true, 1); // RED
					break;
				default:
					break;
			}
		}
	}
}
