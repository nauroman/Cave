using UnityEngine;
using System;
using System.Collections;

namespace Flashunity.Logs
{
	public class LogString
	{
		public readonly string str;
		public readonly string lower;
		public readonly string colored;
		public readonly string shorter;

		public LogString (string str, string color = "", int shorterStringMaxLength = 32)
		{
			this.str = str.Trim ();

			if (this.str.Length > shorterStringMaxLength) {
				shorter = this.str.Substring (0, shorterStringMaxLength);
			} else {
				shorter = this.str;
			}

			lower = this.str.ToLower ();

			if (this.str.Length > 0 && color != "") {
				colored = "<color=" + color + ">" + this.str + "</color>";
			} else {
				colored = this.str;
			}
		}

		public bool isIncludeWords (string[] text)
		{
			if (str.Length == 0) {
				return false;
			}

			if (text.Length == 0)
				return true;
			
			foreach (var world in text) {
				if (isIncludeWord (world)) {
					return true;
				}
			}
			
			return false;
		}
		
		bool isIncludeWord (string word)
		{
			return word.Length <= 0 || lower.IndexOf (word) >= 0;
		}
		
		
		public bool isExcludeWords (string[] text)
		{
			if (str.Length == 0)
				return true;

			foreach (var world in text) {
				if (!isExcludeWord (world)) {
					return false;
				}
			}
			
			return true;
		}
		
		bool isExcludeWord (string word)
		{
			return word.Length <= 0 || lower.IndexOf (word) == -1;
		}

		/*
		public static LogString operator == (LogString x, LogString y)
		{
			/*
			if (System.Object.ReferenceEquals (x, null)) {
				if (System.Object.ReferenceEquals (y, null)) {
					return true;
				}				
				return false;
			}
			*/

		/*
			if (x == null) {
				if (y == null) {
					return true;
				}				
				return false;
			}
			*/
            

		/*
			return x.str == y.str;
*/
		/*
			if (x.lower == "" || y.lower == "")
				return false;
			
			return x.lower.IndexOf (y.lower) >= 0;
			*/
		//}

		/*
		// Inequality operator. Returns dbNull if either operand is
		// dbNull, otherwise returns dbTrue or dbFalse:
		public static LogString operator != (LogString x, LogString y)
		{
			return !(x == y);
		}
*/
		public override int GetHashCode ()
		{
			return str.GetHashCode ();
		}
		
		public override string ToString ()
		{
			return str;
		}
		/*
		public string Colored {
			get{ return colored;}
        }
        */
        
		/*
		public string Colored {
			get{ return colored;}
		}

		public string Shorter {
			get{ return shorter;}
		}
		*/


	}
}
