using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuteColossus {
	public class CharacterName {
		private string firstName;
		private string middleName;
		private string surname;
        private Gender gender;

		public CharacterName(string firstName, string middleName, string surname) {
			this.firstName  = firstName;
			this.middleName = middleName;
			this.surname    = surname;
		}

        public CharacterName(string firstName, string middleName, string surname, Gender gender)
        {
            this.firstName = firstName;
            this.middleName = middleName;
            this.surname = surname;
            this.gender = gender;
        }

        public string GetFirstName() {
			return this.firstName;
		}

		public void SetFirstName(string firstName) {
			this.firstName = firstName;
		}

		public string GetMiddleName() {
			return this.middleName;
		}

		public void SetMiddleName(string middleName) {
			this.middleName = middleName;
		}

		public string GetSurname() {
			return this.surname;
		}

		public void SetSurname(string surname) {
			this.surname = surname;
		}

        public Gender GetGender()
        {
            return this.gender;
        }

        public void SetGender(Gender gender)
        {
            this.gender = gender;
        }
        public string GetGenderString()
        {
            return this.gender == Gender.Female ? "Female" : "Male";
        }
    }

}