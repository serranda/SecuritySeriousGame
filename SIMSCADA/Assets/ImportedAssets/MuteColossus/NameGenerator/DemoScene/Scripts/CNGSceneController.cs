using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MuteColossus;

public class CNGSceneController : MonoBehaviour {
	public Dropdown genders;
	public Toggle   surnames;
	public Toggle   middleNames;
	public Text     nameDisplay;

	public void GenerateName() {
		string name = CNG.instance.gen.GenerateName(this.mapGender(this.genders.value), surnames.isOn, middleNames.isOn);
		nameDisplay.text = name; 
	}

	private Gender mapGender(int gender) {
		switch(gender) {
			case 0:
				return Gender.Female;
			case 1:
				return Gender.Male;
			case 2:
				return Gender.Other;
		}

		return Gender.Female;
	}
}
