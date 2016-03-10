using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Flashunity.Logs
{
    //, IPointerClickHandler
    public class BLogItem : BItem
    {
        public RectTransform background;
        public float minHeight = 14;

        new public LogItem LogItem
        {
            set
            {
                _logItem = value;

                textNumber.text = value.number.ToString();

                textLabel.text = value.label.colored;//.ToString ();

                textReflection.text = value.reflection.colored;

                textDateTime.text = value.labelDateTime.ToString();

                toggle.isOn = value.pin;

                updateHeight();
            }
            get
            {
                return _logItem;
            }
        }

        public void updateHeight()
        {
            //print ("transform.parent:" + transform.parent);
            RectTransform rectTransform = GetComponent<RectTransform>();

            var active = textLabel.gameObject.activeSelf;

            textLabel.gameObject.SetActive(true);
            textReflection.gameObject.SetActive(true);

            float max = Mathf.Max(LayoutUtility.GetPreferredHeight(textLabel.rectTransform), LayoutUtility.GetPreferredHeight(textReflection.rectTransform), minHeight);

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, max);

            textLabel.gameObject.SetActive(active);
            textReflection.gameObject.SetActive(active);
        }

        void Update()
        {
            Vector3[] v = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(v);

            float maxY = Mathf.Max(v [0].y, v [1].y, v [2].y, v [3].y);
            float minY = Mathf.Min(v [0].y, v [1].y, v [2].y, v [3].y);

            SetLabelsActive(maxY >= 0 && minY <= Screen.height);

            /*
			if (maxY < 0 || minY > Screen.height) {

				textDateTime.gameObject.SetActive (false);
				textReflection.gameObject.SetActive (false);
				textLabel.gameObject.SetActive (false);
				textNumber.gameObject.SetActive (false);
			} else {
				textDateTime.gameObject.SetActive (true);
				textReflection.gameObject.SetActive (true);
				textLabel.gameObject.SetActive (true);
				textNumber.gameObject.SetActive (true);
			}
			*/
        }

        void SetLabelsActive(bool value)
        {
            textNumber.gameObject.SetActive(value);
            textDateTime.gameObject.SetActive(value);
            textReflection.gameObject.SetActive(value);
            textLabel.gameObject.SetActive(value);
            toggle.gameObject.SetActive(value);
            buttonCopy.gameObject.SetActive(value);
            background.gameObject.SetActive(value);

            //GetComponent<Image> ().gameObject.SetActive (value);
        }

        void Awake()
        {
            //	minHeight = (transform as RectTransform).sizeDelta.y;
        }

        void LateUpdate()
        {
            /*
			if (autoMoveCanvasUp && (moveCanvas != null)) {
				moveCanvas ();
				moveCanvas = null;
			}
			*/
			
            /*
			if (needUpdateChildrenPositions) {
				//	print ("LateUpdate");
				UpdateChildrenPositions ();
				needUpdateChildrenPositions = false;
			}
			*/
        }


        /*
		public void OnPointerClick (PointerEventData eventData)
		{
			//	EventSystem.
			//PointerEventData.

			//Grab the number of consecutive clicks and assign it to an integer varible.
			//int i = eventData.clickCount;
			//Display the click count.
			LogCanvas.instance.Add ("PointerClick");
		}
*/

        /*
		public void OnPointerClick ()
		{
			LogCanvas.instance.Add ("PointerClick");
		}
        */
        

    }
}
