using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Flashunity.Logs
{
	public class BButtonShow : MonoBehaviour
	{
		public Image imageLogs;
		public Image imageWarnings;
		public Image imageErrors;

		public Text textLogs;
		public Text textWarnings;
		public Text textErrors;

		uint logs = 0;
		uint warnings = 0;
		uint errors = 0;

		public void Awake ()
		{
			UpdateImageAndTextLogs ();
		}

		public void Clear ()
		{
			Logs = 0;
			Warnings = 0;
			Errors = 0;
		}

		public void OnItemAdded (LogItem logItem)
		{
			switch (logItem.type) {
			case LogType.LOG:
				Logs++;
				break;
			
			case LogType.WARNING:
				Warnings++;
				break;
		
			case LogType.ERROR:
				Errors++;
				break;
			}
		}

		public uint Logs {
			set {
				logs = value;

				UpdateImageAndTextLogs ();

				/*
				if (logs > 0 || warnings > 0 || errors > 0) {
					imageLogs.gameObject.SetActive (true);
					textLogs.gameObject.SetActive (true);
					textLogs.text = logs.ToString ();
				} else {
					imageLogs.gameObject.SetActive (false);
					textLogs.gameObject.SetActive (false);
				}
				*/
			}

			get {
				return logs;
			}
		}
            
		public uint Warnings {
			set {
				warnings = value;

				if (warnings > 0) {
					imageWarnings.gameObject.SetActive (true);
					textWarnings.gameObject.SetActive (true);
					textWarnings.text = warnings.ToString ();
				} else {
					imageWarnings.gameObject.SetActive (false);
					textWarnings.gameObject.SetActive (false);
				}

				UpdateImageAndTextLogs ();

			}
			
			get {
				return warnings;
			}
		}

		public uint Errors {
			set {
				errors = value;
				
				if (errors > 0) {
					imageErrors.gameObject.SetActive (true);
					textErrors.gameObject.SetActive (true);
					textErrors.text = errors.ToString ();
				} else {
					imageErrors.gameObject.SetActive (false);
					textErrors.gameObject.SetActive (false);
				}

				UpdateImageAndTextLogs ();
			}
			
			get {
				return errors;
			}
		}

		void UpdateImageAndTextLogs ()
		{
			if (logs > 0 || warnings > 0 || errors > 0) {
				enabled = true;
				transform.localScale = new Vector3 (1, 1, 1);
				imageLogs.gameObject.SetActive (true);
				textLogs.gameObject.SetActive (true);
				textLogs.text = logs.ToString ();
			} else {
				enabled = false;
				transform.localScale = new Vector3 (0, 0, 0);
				imageLogs.gameObject.SetActive (false);
				textLogs.gameObject.SetActive (false);
			}

		}

		/*
		public void SetLogType (int type)
		{
			switch (type) {
			case LogType.WARNING:
				if (!wasWarning && !wasError) {

				}
				break;
			}
		}

		void SetColor (Color color)
		{
			Button button = GetComponent<Button> ();
			
			//button.colors.normalColor = color;
		}
		*/
	}
}
