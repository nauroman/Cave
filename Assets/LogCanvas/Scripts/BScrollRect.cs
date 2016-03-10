using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Flashunity.Logs
{
	public class BScrollRect : MonoBehaviour, IScrollHandler
	{
		public RectTransform items;

		float previousHeight = -1;

		string[] include = {""};
		string[] exclude = {""};



		//string include = "";
		//string exclude = "";

		/*
		enum MoveDirections
		{
			Up,
			Down}
		;
		*/



		Action moveCanvas;

		//	delegate void FunctionMoveCanvas ();

//		func needMoveCanvas;

		//int isScrolledUpCount = 0;
		//int thresholdScrolledUp = 3;

		int isScrolledDownCount = 0;
		int thresholdScrolledDown = 5;

		bool autoMoveCanvasUp = true;


		/*
		public RectTransform AddChild (LogItem logItem)
		{
			RectTransform item = items.GetComponent<ItemsScript> ().AddChild (logItem);
			ApplyIncludeAndExclude (item, include, exclude);
			
//			UpdateChildPosition (item);

			return item;
		}
*/

		/*
		public bool AutoMoveCanvasUp {
			get {
				return autoMoveCanvasUp;
			}
		}
		*/

		void MoveCanvasDown ()
		{
			GetComponent<ScrollRect> ().verticalNormalizedPosition = 1;
		}
		
		void MoveCanvasUp ()
		{
			GetComponent<ScrollRect> ().verticalNormalizedPosition = 0;
		}
		
		bool IsScrolledDown ()
		{
			return GetComponent<ScrollRect> ().verticalNormalizedPosition > 0;

			//return items && ((items.rect.y + items.rect.height) < (transform as RectTransform).rect.height); 
		}
		
		bool IsScrolledUp ()
		{/*
			var itemsTransform = items as RectTransform;

			var rect = (transform as RectTransform).rect;
			var itemsRect = itemsTransform.rect;

			return ((itemsRect.height - itemsTransform.anchoredPosition.y) < rect.height);
*/
			return GetComponent<ScrollRect> ().verticalNormalizedPosition <= 0;

		}




		void OnDisable ()
		{
		}
		
		void OnEnable ()
		{
//			UpdateChildrenHeights ();
//			needUpdateChildrenPositions = true;

			moveCanvas = MoveCanvasUp;
//			needMoveCanvas = MoveDirection.Up;
		}

		public void OnItemAdded (RectTransform item)
		{
			if (autoMoveCanvasUp) {
				moveCanvas = MoveCanvasUp;
			}
//			needMoveCanvas = MoveDirection.Up;
		}

		public void setInclude (string[] str)
		{
			include = str;

			OnValueChangeIncludeOrExlude ();
		}
		
		public void setExclude (string[] str)
		{
			exclude = str;

			OnValueChangeIncludeOrExlude ();
		}
        
		void OnValueChangeIncludeOrExlude ()
		{
			if (include.Length > 0 || exclude.Length > 0) {
				autoMoveCanvasUp = false;
				moveCanvas = MoveCanvasDown;
			} else {
				autoMoveCanvasUp = true;
				moveCanvas = MoveCanvasUp;
			}
		}


		/*
		public void OnDrag (PointerEventData data)
		{
		IDragHandler, 
			LogCanvas.instance.Add (data.ToString ());
		}
*/
		/*
		public void OnBeginDrag (PointerEventData eventData)
		{
			Log.Add ("Ok");

			if (IsScrolledUp ()) {
				autoMoveCanvasUp = true;
			} else {
				isScrolledUpCount = 0;
				autoMoveCanvasUp = false;
			}
		}

		public void OnDrag (PointerEventData eventData)
		{
			Log.Add ("Ok");
		}

		public void OnEnDrag (PointerEventData eventData)
		{
			Log.Add ("Ok");
		}
*/

		/*
		void OnMouseDown ()
		{
			Log.Add ("ok");
		}

		void OnMouseUp ()
		{
			Log.Add ("ok");

		}
		*/

		public void OnScroll (PointerEventData data)
		{
			//	Log.Add ("ok");

//			UpdateAutoMoveCanvasUp ();
		}

		void UpdateAutoMoveCanvasUp ()
		{
			if (IsScrolledDown ()) {
				if (isScrolledDownCount < thresholdScrolledDown) {
					isScrolledDownCount++;
				} else {
					isScrolledDownCount = 0;
					autoMoveCanvasUp = false;
				}
			} else {
				isScrolledDownCount = 0;
				autoMoveCanvasUp = true;
			}

			//autoMoveCanvasUp = IsScrolledUp ();

			/*
			if (IsScrolledUp ()) {
				if (isScrolledUpCount < thresholdScrolledUp) {
					isScrolledUpCount++;
				} else {
					isScrolledUpCount = 0;
					autoMoveCanvasUp = true;
				}
			} else {
				isScrolledUpCount = 0;
				autoMoveCanvasUp = false;
			}
			*/
		}

		void Update ()
		{

			if (previousHeight == -1) {
				previousHeight = GetComponent<RectTransform> ().rect.height;
			} else {
				float height = GetComponent<RectTransform> ().rect.height;
				
				if (previousHeight != height) {

					items.GetComponent<BItems> ().OnUpdateScrollRectSize ();

					//UpdateChildrenHeights ();
					//needUpdateChildrenPositions = true;
					previousHeight = height;
				}
			}
		}

		void LateUpdate ()
		{
			if (autoMoveCanvasUp && (moveCanvas != null)) {
				moveCanvas ();
				moveCanvas = null;
			}

			UpdateAutoMoveCanvasUp ();

			/*
			if (needUpdateChildrenPositions) {
				//	print ("LateUpdate");
				UpdateChildrenPositions ();
				needUpdateChildrenPositions = false;
			}
			*/
		}

	}
}
