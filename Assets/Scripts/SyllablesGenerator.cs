using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

//this is a supporting component, used to create natural-ish syllables
public class SyllablesGenerator : MonoBehaviour
{
	[TextArea(3, 10)] public string basis;

	[TextArea(3, 10)] public string output;

	void Start ()
	{
		string[] Words = basis.Split(' ');


		List<string> slbs = new List<string>();

		bool afterSngSlb = false;
		foreach (string word in Words)
		{
			if (word.Length == 1)
			{
				slbs.Add(word);
			}
			else
			{
				if (word.Length % 2 == 0)
				{
					for (int k = 0; k < word.Length - 1; k++)
					{
						if (k % 2 == 0)
						{
							slbs.Add(word.Substring(k, 2));
						}
					}
				}
				else
				{
					afterSngSlb = false;
					int sng_slb = Random.Range(0, word.Length);
					while (sng_slb % 2 != 0)
					{
						sng_slb = Random.Range(0, word.Length);
					}

					for (int k = 0; k < word.Length - 1; k++)
					{
						if (k == sng_slb)
						{
							afterSngSlb = true;
							slbs.Add(word[k].ToString());
						}

						if (afterSngSlb)
						{
							if (k % 2 != 0)
								slbs.Add(word.Substring(k, 2));
						}
						else
						{
							if (k % 2 == 0)
								slbs.Add(word.Substring(k, 2));
						}
					}

					if (!afterSngSlb)
					{
						slbs.Add(word[word.Length - 1].ToString());
					}
				}
			}

			
		}

		while (slbs.Count < 18)
		{
			string abc = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
			string st = abc[Random.Range(0, abc.Length)].ToString();
			if (Random.Range(0, 2) == 0)
			{
				st += abc[Random.Range(0, abc.Length)].ToString();
			}

			slbs.Add(st);
		}
		for (int i = 0; i < slbs.Count; i++)
		{
			string temp = slbs[i];
			int randomIndex = Random.Range(i, slbs.Count);
			slbs[i] = slbs[randomIndex];
			slbs[randomIndex] = temp;
		}

	    output = String.Join(" ", slbs.ToArray());
	}
		
	void Update () {
		
	}
}
