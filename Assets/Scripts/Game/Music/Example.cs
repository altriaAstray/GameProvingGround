/*
 * Copyright (c) 2015 Allan Pichardo
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
	public List<GameObject> list = new List<GameObject>();


	void Start ()
	{
		AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
		processor.onBeat.AddListener (onOnbeatDetected);
		processor.onSpectrum.AddListener (onSpectrum);
	}

	//每次检测到节拍时都会调用此事件。更改检查器中的阈值参数以调整灵敏度
	void onOnbeatDetected ()
	{
		//Debug.Log ("Beat!!!");
	}

	//播放音乐时，每帧都会调用此事件
	void onSpectrum (float[] spectrum)
	{
		//The spectrum is logarithmically averaged to 12 bands
		//频谱对数平均到 12 个波段

		for (int i = 0; i < spectrum.Length; ++i) 
		{
			//Vector3 start = new Vector3 (i * 0.1f, 0, 0);
			//Vector3 end = new Vector3 (i * 0.1f, spectrum [i], 0);
			//Debug.DrawLine (start, end);
			if(list[i] != null)
            {
				list[i].GetComponent<RectTransform>().sizeDelta = new Vector2(40, spectrum[i] * 1000);
			}
		}
	}
}
