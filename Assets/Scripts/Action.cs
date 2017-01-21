using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {

	public enum Names
	{
		A,
		R,
		M,
		C,
		G, 
		F, 
		L,
		N,
		K, 
		D
	};

	static Dictionary<Names, int> Houses = new Dictionary<Names, int>(){
		{Names.A, 1},
		{Names.R, 1},
		{Names.M, 1},
		{Names.C, 2},
		{Names.G, 2},
		{Names.F, 2},
		{Names.L, 2},
		{Names.N, 3},
		{Names.K, 3},
		{Names.D, 3},
	};

	public enum Actions
	{
		None,
		Raid,
		Follow,
		Arrest
	};

	Names eTargetPerson;
	Actions eAction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string GetActionTag()
	{
		string sAction = eAction.ToString().ToLower();

		if(eAction==Actions.Raid)
		{
			int iHouse = Houses[eTargetPerson];
			return sAction + "-" + iHouse.ToString();
		}
		else
		{
			return sAction + "-" + eTargetPerson.ToString();
		}
	}
}
