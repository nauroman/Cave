using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Flashunity.Logs
{
	public class BItems : MonoBehaviour
	{
		public RectTransform item;
		public float gap = 0;

		//string[] include = new string[]{""};
		//string[] exclude = new string[]{""};

		string[] include = {""};
		string[] exclude = {""};

		//string include = "";
		//string exclude = "";


//		float previousHeight = -1;

		bool needUpdateChildrenPositions;
		bool needUpdateHeight;


		public RectTransform AddItem (LogItem logItem)
		{
			RectTransform item = Instantiate (this.item);
		
			item.SetParent (transform, false);
		
			item.GetComponent<BLogItem> ().LogItem = logItem;

			ApplyIncludeAndExclude (item);

			UpdateChildPosition (item);

			UpdateHeight ();

			return item;
		}

		public void setInclude (string[] str)
		{
			include = str;

			ApplyIncludeAndExclude ();
		}

		public void setExclude (string[] str)
		{
			exclude = str;

			ApplyIncludeAndExclude ();
		}

		public void OnValueChangeTogglePinAll (bool isOn)
		{
			int childCount = transform.childCount;
			
			for (int i=0; i<childCount; i++) {
				RectTransform child = transform.GetChild (i) as RectTransform;
				
				if (child.gameObject.activeSelf) {
					
					var script = child.GetComponentInChildren<BLogItem> () as BLogItem;
					script.LogItem.pin = isOn;
					script.toggle.isOn = isOn;
				}
			}

			if (!isOn) {
				ApplyIncludeAndExclude ();
			}
		}

		void ApplyIncludeAndExclude ()
		{
			int childCount = transform.childCount;
			
			for (int i=0; i<childCount; i++) {
				
				ApplyIncludeAndExclude (transform.GetChild (i) as RectTransform);
			}

			needUpdateChildrenPositions = true;
			needUpdateHeight = true;
		}
		
		
		void ApplyIncludeAndExclude (RectTransform item)
		{
			if (gameObject.activeSelf) {
				
				item.gameObject.SetActive (true);

				var logItemScript = item.GetComponent<BLogItem> ();

//				logItemScript.toggle.gameObject.SetActive (true);
				
				if (logItemScript) {
					if (logItemScript.LogItem.pin) {
						item.gameObject.SetActive (true);
					} else {
						LogItem logItem = logItemScript.LogItem;

						item.gameObject.SetActive (logItem.isIncludeAndExclude (include, exclude));

//					item.gameObject.SetActive ((isIncludeText (logItem.labelToLower, include) || isIncludeText (logItem.reflectionToLower, include)) && isExcludeText (logItem.labelToLower, exclude) && isExcludeText (logItem.reflectionToLower, exclude));
						//	item.gameObject.SetActive ((isIncludeText (logItem.labelToLower, include) || isIncludeText (logItem.reflectionToLower, include) || isIncludeText (logItem.labelDateTime, include)) && isExcludeText (logItem.labelToLower, exclude) && isExcludeText (logItem.reflectionToLower, exclude) && isExcludeText (logItem.labelDateTime, exclude));
						//item.gameObject.SetActive ((logItem.label.isIncludeWords (include) || logItem.reflection.isIncludeWords (include) || logItem.labelDateTime.isIncludeWords (include)) && isExcludeText (logItem.labelToLower, exclude) && isExcludeText (logItem.reflectionToLower, exclude) && isExcludeText (logItem.labelDateTime, exclude));
					}
				}
			}
		}
		
		
		/*
		bool isIncludeText (string str, string[] text)
		{
			if (text.Length == 0)
				return true;

			foreach (var world in text) {
				if (isIncludeWord (str, world)) {
					return true;
				}
			}

			return false;


//			return text.Length <= 0 || str.IndexOf (text) >= 0;
		}

		bool isIncludeWord (string str, string word)
		{
			return word.Length <= 0 || str.IndexOf (word) >= 0;
		}

		
		bool isExcludeText (string str, string[] text)
		{
			foreach (var world in text) {
				if (!isExcludeWord (str, world)) {
					return false;
				}
			}
			
			return true;
		}

		bool isExcludeWord (string str, string word)
		{
			return word.Length <= 0 || str.IndexOf (word) == -1;
		}
		*/

		public void OnUpdateScrollRectSize ()
		{
			UpdateChildrenHeights ();
//			UpdateChildrenPositions ();
			needUpdateChildrenPositions = true;
			needUpdateHeight = true;
		}

		void UpdateChildrenHeights ()
		{
			int childCount = transform.childCount;
			
			for (int i=0; i<childCount; i++) {
				UpdateChildHeight (transform.GetChild (i) as RectTransform);
			}
		}

		void UpdateChildrenPositions ()
		{
			int childCount = transform.childCount;
			
			for (int i=0; i<childCount; i++) {
				UpdateChildPosition (transform.GetChild (i) as RectTransform);
			}
			
			//		UpdateCanvasHeight ();
			//	MoveCanvasDown ();
		}
		
		void UpdateChildHeight (RectTransform item)
		{
			bool active = item.gameObject.activeSelf;
			
			item.gameObject.SetActive (true);

			item.GetComponent<BLogItem> ().updateHeight ();
//			item.GetComponent<LogItemBehaviour> ().needUpdateHeight = true;

//			LogItemScript canvasLogItemScript = rectTransform.GetComponent<LogItemScript> () as LogItemScript;
			
//			canvasLogItemScript.updateHeight ();
			
			item.gameObject.SetActive (active);
		}
		
		
		RectTransform GetPreviousActive (int index)
		{
			for (int i=index-1; i>=0; i--) {
				RectTransform item = transform.GetChild (i) as RectTransform;
				
				if (item.gameObject.activeSelf)
					return item;
			}
			
			return null;
		}
		
		RectTransform GetFirstActive ()
		{
			int childCount = transform.childCount;
			
			for (int i=0; i<childCount; i++) {
				RectTransform item = transform.GetChild (i) as RectTransform;
				
				if (item.gameObject.activeSelf)
					return item;
			}
			
			return null;
		}
		
		RectTransform GetLastActive ()
		{
			int childCount = transform.childCount;
			
			for (int i=childCount-1; i>=0; i--) {
				RectTransform child = transform.GetChild (i) as RectTransform;
				
				if (child.gameObject.activeSelf)
					return child;
			}
			
			return null;
		}
		
		
		int GetChildIndex (RectTransform item)
		{
			// DO TO!!!!!!!!!!!!!!!!!!!!!
			//rectTransform.GetSiblingIndex();
			
			int childCount = transform.childCount;
			
			for (int i=0; i<childCount; i++) {
				if ((transform.GetChild (i) as RectTransform) == item)
					return i;
			}
			
			return -1;
		}
		
		void UpdateChildPosition (RectTransform item)
		{
			int childIndex = GetChildIndex (item);
			
			RectTransform previousRectTransform = GetPreviousActive (childIndex);
			
			if (previousRectTransform != null) {
				item.localPosition = new Vector3 (0, -previousRectTransform.sizeDelta.y - gap + previousRectTransform.localPosition.y, 0);
			} else {
				item.localPosition = new Vector3 (0, 0, 0);
			}
		}

		public void UpdateHeight ()
		{
			RectTransform lastActive = GetLastActive ();
			
			//	RectTransform rectTransform = GetComponent<RectTransform> ();
			if (lastActive != null) {
				
				float h = -lastActive.localPosition.y + lastActive.sizeDelta.y;

				(transform as RectTransform).sizeDelta = new Vector2 ((transform as RectTransform).sizeDelta.x, h > 0 ? h : 1);
			} else {
				(transform as RectTransform).sizeDelta = new Vector2 ((transform as RectTransform).sizeDelta.x, 1);
			}
		}

		public void DestroyAllChildren ()
		{
			foreach (RectTransform item in transform) {
				
				RectTransform.Destroy (item.gameObject);
			}
		}

		public void DestroyNotPinItems ()
		{
			foreach (RectTransform item in transform) {
				
				BLogItem logItemScript = item.GetComponentInChildren<BLogItem> () as BLogItem;
				
				
				if (logItemScript && logItemScript.toggle != null && logItemScript.LogItem != null) {
					logItemScript.LogItem.pin = logItemScript.toggle.isOn;
					
					if (logItemScript.toggle.isOn) {
						continue;
					}
				}
				
				RectTransform.Destroy (item.gameObject);
			}

			needUpdateChildrenPositions = true;
		}

		public void RestoreItems (List<LogItem> list)
		{
			DestroyAllChildren ();

			foreach (LogItem logItem in list) {
				AddItem (logItem);
			}
		}

		public void CopyToClipboard ()
		{
			string str = "";

			foreach (Transform item in transform) {
				if (item.gameObject.activeSelf) {
					str += item.GetComponent<BItem> ().LogItem.ToString ();
					str += Environment.NewLine;
				}
			}

			GUIUtility.systemCopyBuffer = str;
		}
		
		void OnEnable ()
		{
			UpdateChildrenHeights ();
			needUpdateChildrenPositions = true;
			needUpdateHeight = true;
		}

		void Awake ()
		{
			DestroyAllChildren ();
		}

		void LateUpdate ()
		{
			if (needUpdateChildrenPositions) {
				//	print ("LateUpdate");
				UpdateChildrenPositions ();
				needUpdateChildrenPositions = false;
			}

			if (needUpdateHeight) {
				UpdateHeight ();
				needUpdateHeight = false;
			}
		}

	}
}
