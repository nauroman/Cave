using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Flashunity.Logs
{
	public class BItem : MonoBehaviour
	{
		protected LogItem _logItem;

		public Text textNumber;

		public Text textDateTime;

		public Text textLabel;

		public Text textReflection;

		public Toggle toggle;

		public Button buttonCopy;



		public LogItem LogItem {
			set {
				_logItem = value;

				textNumber.text = value.number.ToString ();

				textLabel.text = value.label.colored;//.ToString ();

				textReflection.text = value.reflection.colored;

				textDateTime.text = value.labelDateTime.ToString ();

				//			toggle.isOn = value.pin;

				//			updateHeight ();
			}
			get {
				return _logItem;
			}
		}

		public void OnButtonCopyClick ()
		{
			GUIUtility.systemCopyBuffer = _logItem.ToString ();

			//Log.Add (ToString ());
		}

		public override string ToString ()
		{
			return _logItem.ToString ();// + "  " + _logItem.labelDateTime + "  " + _logItem.reflection + "  " + _logItem.label;
		}

		/*
		public void updateHeight ()
		{
			//print ("transform.parent:" + transform.parent);
			RectTransform rectTransform = GetComponent<RectTransform> ();

			float textHeight = LayoutUtility.GetPreferredHeight (textLabel.rectTransform);
			
			if (rectTransform.sizeDelta.y < textHeight) {
				rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, textHeight);
			}

			textHeight = LayoutUtility.GetPreferredHeight (textReflection.rectTransform);
			
			if (rectTransform.sizeDelta.y < textHeight) {
				rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, textHeight);
			}
		}

		void Update ()
		{
			Vector3[] v = new Vector3[4];
			GetComponent<RectTransform> ().GetWorldCorners (v);
			
			float maxY = Mathf.Max (v [0].y, v [1].y, v [2].y, v [3].y);
			float minY = Mathf.Min (v [0].y, v [1].y, v [2].y, v [3].y);

			if (maxY < 0 || minY > Screen.height) {

				textDateTime.gameObject.SetActive (false);
				textReflection.gameObject.SetActive (false);
				textLabel.gameObject.SetActive (false);
			} else {
				textDateTime.gameObject.SetActive (true);
				textReflection.gameObject.SetActive (true);
				textLabel.gameObject.SetActive (true);
			}
		}
		*/


	}
}
