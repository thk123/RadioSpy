using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

	public enum Names
	{
		G,
		C,
		V,
		F,
		R, 
		A, 
		L,
		D,
		M, 
		K,
        None
	};

	static Dictionary<Names, int> Houses = new Dictionary<Names, int>(){
		/*{Names.A, 1},
		{Names.R, 1},
		{Names.M, 1},
		{Names.C, 2},
		{Names.G, 2},
		{Names.F, 2},
		{Names.L, 2},
		{Names.N, 3},
		{Names.K, 3},
		{Names.D, 3},*/
		// TODO: not being used but should link names to houses
	};

	public enum Actions
	{
		None,
		Raid,
		Follow,
		Arrest
	};

	public Names eTargetPerson;
    public Actions eAction;

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
        else if(eAction == Actions.None)
        {
            return "NONE";
        }
		else
		{
			return sAction + "-" + eTargetPerson.ToString().ToLower();
		}
	}

    public override bool Equals(object obj)
    {
        if(obj is Action)
        {
            return ((Action)obj).GetActionTag() == GetActionTag();
        }
        else
        {
            return false;
        }
    }

    // Factory methods
    public static Action NoAction()
    {
        return new Action { eTargetPerson = Names.None, eAction = Actions.None };
    }

    public static Action Raid(int iTargetHouse)
    {
        Names eName = Names.None;
        foreach(KeyValuePair<Names, int> person in Houses)
        {
            if(person.Value == iTargetHouse)
            {
                eName = person.Key;
                break;
            }
        }

        if(eName == Names.None)
        {
            Debug.LogError("Couldn't find any person in house " + iTargetHouse);
        }
        return new Action { eTargetPerson = eName, eAction = Actions.Raid };
    }

    public static Action Follow(Names eName)
    {
        return new Action { eTargetPerson = eName, eAction = Actions.Follow };
    }

    public static Action Arrest(Names eName)
    {
        return new Action { eTargetPerson = eName, eAction = Actions.Arrest };
    }
}
