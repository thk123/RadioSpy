using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTextDisplayer : MonoBehaviour {

	public TextMesh Line1;
	public TextMesh Line2;
	public int iMaxLength = 20;
	public float fScrollPeriod = 0.5f;
	// Use this for initialization
	void Start () {
		DisplayText("Title", "abcbasjdhaskdhakdhakdhakdakdhajshdk");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DisplayText(string sTitle, string sLine)
	{
		StopAllCoroutines();
		ApplyText(sTitle, Line1);
		ApplyText(sLine, Line2);
	}

	void ApplyText(string sText, TextMesh xTextMesh)
	{
		if(sText.Length > iMaxLength)
		{
			StartCoroutine(ScrollText(sText, xTextMesh));
		}
		else
		{
			xTextMesh.text = sText;
		}
	}

	IEnumerator ScrollText(string sFullText, TextMesh xTextMesh)
	{
		int iPos = 0;
		while(true)
		{
			string sSubString = sFullText.Substring(iPos, Mathf.Min(iMaxLength, sFullText.Length - iPos));
			xTextMesh.text = sSubString;

			// If we are at the start, allow a little longer to read
			if(iPos == 0)
			{
				yield return new WaitForSeconds(fScrollPeriod * 2.0f);
			}
			yield return new WaitForSeconds(fScrollPeriod);

			if(iPos == sFullText.Length)
			{
				iPos = 0;
			}
			else
			{
				++iPos;
			}


		}
	}
}
