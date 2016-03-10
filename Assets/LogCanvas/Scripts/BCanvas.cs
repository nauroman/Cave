using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

//using System.Globalization;
//using System.Diagnostics;
//using System.Reflection;
using UnityEngine.EventSystems;
using Flashunity.Screenshots;

namespace Flashunity.Logs
{
    public class BCanvas : MonoBehaviour
    {
        [Tooltip("Number of stack trace frames. Minimum value is 0")]
        public int
            reflection = 1;
		
        [Tooltip("Show the log canvas panel if error is occured")]
        public bool
            showOnError;
		
        [Tooltip("Set time scale 0 if error is occured")]
        public bool
            stopTimeOnError;
		
        [Tooltip("Play this audio if error is occured")]
        public AudioClip
            audioError;
		
        [Tooltip("Separators array for include and exlude input fields")]
        public string[]
            separator = { "," };

        int number = 0;

        List<LogItem> list = new List<LogItem>();

        public InputField inputFieldInclude;

        public InputField inputFieldExclude;


        public Toggle togglePinAll;

        public Toggle togglePause;

        public Button buttonShow;

        public RectTransform visibleContent;

        public RectTransform scrollRect;

        public RectTransform items;

        public RectTransform lastItem;



        bool paused;

        private static BCanvas _instance;

        public static BCanvas instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<BCanvas>();
				
                    if (_instance && _instance.gameObject)
                    {
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }
			
                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            } else
            {
                if (this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }

            UpdatePlaceholder(inputFieldInclude.placeholder.GetComponent<Text>(), "Include...");
            UpdatePlaceholder(inputFieldExclude.placeholder.GetComponent<Text>(), "Exclude...");
			
//			buttonShow.gameObject.SetActive (!VisibleContentGameObject.activeSelf);
//			togglePause.isOn = paused;
        }

        void Start()
        {
        }

        void UpdatePlaceholder(Text text, string begin)
        {
            text.text = begin;

            if (separator.Length > 0)
            {
                text.text += " (use ";

                foreach (string str in separator)
                {
                    text.text += str;
                    text.text += " ";
                }
				
                text.text += separator.Length == 1 ? "as a separator)" : "as separators)";
            }
        }

        void OnGUI()
        {
            if (Event.current.type == EventType.KeyUp)
            {

                if (Event.current.alt && Event.current.command && Event.current.control)
                {

                    /*
					switch (Event.current.keyCode) {
					case KeyCode.L:
						Visible = !Visible;
					case KeyCode.A:
						RemoveAll ();
					}
					*/


                    if (Event.current.keyCode == KeyCode.L)
                    {
                        Visible = !Visible;
                    }
				
                    if (Event.current.keyCode == KeyCode.A)
                    {
                        Clear();
                    }
                }

                //GameObject child = transform.GetChild (0).gameObject;			
                //child.SetActive (!child.activeSelf);
            }

            //print ("Current event detected: " + Event.current.type);
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public void Add(string label = "", int type = LogType.LOG, string color = "", int screenshotType = ScreenshotType.NONE, int reflection = -1)
        {
            if (!togglePause.isOn)
            {

                if (reflection == -1)
                {
                    reflection = this.reflection;
                }

                LogItem logItem = new LogItem(label, type, color, screenshotType, reflection, ++number);

                list.Add(logItem);

                var item = items.GetComponent<BItems>().AddItem(logItem);

                scrollRect.GetComponent<BScrollRect>().OnItemAdded(item);
                buttonShow.GetComponent<BButtonShow>().OnItemAdded(logItem);

                SetLastItem(logItem);


                if (showOnError && type == LogType.ERROR)
                {
                    Visible = true;
                }

                if (stopTimeOnError && type == LogType.ERROR)
                {
                    Time.timeScale = 0;
                }

                PlaySounds(type);

                //	UpdateButtonShow (type);
            }
        }

        void PlaySounds(int type)
        {
            switch (type)
            {
                case LogType.ERROR:
                    if (audioError != null)
                    {
                        AudioSource.PlayClipAtPoint(audioError, new Vector3());
//					audioError.Play ();
                    }
                    break;
            }
        }


        public void Remove(string label, string reflection = "")
        {
            var ar = SplitStringToTrimmedStrings(label);

            list.RemoveAll(s => s.label.isIncludeWords(ar));
            list.RemoveAll(s => s.reflection.isIncludeWords(ar));
            
            /*
            var ar = SplitStringToTrimmedStrings (label);

			foreach (string str in ar) {
				list.RemoveAll (s => s.labelToLower.IndexOf (str) >= 0);
			}

			ar = SplitStringToTrimmedStrings (reflection);
			
			foreach (string str in ar) {
				list.RemoveAll (s => s.reflectionToLower.IndexOf (str) >= 0);
			}
			*/


//			items.GetComponent<ItemsBehaviour> ().DestroyBy ();
            /*
			if (label != null && label.Length > 0) {
				string labelToLower = label.ToLower ().Trim ();
				list.RemoveAll (s => s.labelToLower.IndexOf (labelToLower) >= 0);
			}

			if (reflection != null && reflection.Length > 0) {
				string labelToLower = reflection.ToLower ().Trim ();
				list.RemoveAll (s => s.reflection.IndexOf (labelToLower) >= 0);
			}
*/
            RestoreItems();
        }

        /*
		void RemoveByWord (string word)
		{

		}


		bool isIncludeText (string str, string[] text)
		{
			foreach (var world in text) {
				if (isIncludeWord (str, world)) {
					return true;
				}
			}
			
			return false;
		}
		
		bool isIncludeWord (string str, string word)
		{
			return word.Length <= 0 || str.IndexOf (word) >= 0;
		}
		*/

        void SetLastItem(LogItem logItem = null)
        {
            if (logItem != null)
            {
                lastItem.gameObject.SetActive(true);

                var logScript = lastItem.GetComponent<BItem>();

                if (!logScript.toggle.isOn)
                {
                    logScript.LogItem = logItem;
                }
            } else
            {
                lastItem.gameObject.SetActive(false);
            }
        }


        public void Clear()
        {
            DestroyNotPinItems();

            bool a = buttonShow.gameObject.activeSelf;

            buttonShow.gameObject.SetActive(true);
            buttonShow.GetComponent<BButtonShow>().Clear();

            SetLastItem();

            buttonShow.gameObject.SetActive(a);
        }

        public void CopyToClipboard()
        {
            items.GetComponent<BItems>().CopyToClipboard();
        }

        public void ClearInputFieldInclude()
        {
            inputFieldInclude.text = "";

            if (VisibleContentGameObject.activeSelf)
            {
                inputFieldInclude.Select();
                inputFieldInclude.ActivateInputField();
            }
        }

        public void ClearInputFieldExclude()
        {
            inputFieldExclude.text = "";
			
            if (VisibleContentGameObject.activeSelf)
            {
                inputFieldExclude.Select();
                inputFieldExclude.ActivateInputField();
            }
        }

        void DestroyNotPinItems()
        {
            items.GetComponent<BItems>().DestroyNotPinItems();

            list.RemoveAll(s => !s.pin);
            /*
			foreach (var logItem in list) {
				
				if (logItem.pin) {
					continue;
				}

				list.Remove (logItem);
			}
*/

            //		RestoreItems ();
        }

        public void OnValueChangeInputTextInclude()
        {
            //var itemsScript = items.GetComponent<ItemsScript> ();

            var ar = SplitStringToTrimmedStrings(inputFieldInclude.text);
            /*
			var text = inputFieldInclude.text.Trim ().ToLower ();

			//var ar = text.Split (new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
			var ar = text.Split (separator, StringSplitOptions.RemoveEmptyEntries);

			ar = Trim (ar);
*/
            //items.GetComponent<ItemsBehaviour> ().setInclude (text);
            items.GetComponent<BItems>().setInclude(ar);

            scrollRect.GetComponent<BScrollRect>().setInclude(ar);
        }

        public void OnValueChangeInputTextExclude()
        {
            //var itemsScript = items.GetComponent<ItemsScript> ();

            //var text = inputFieldExclude.text.Trim ().ToLower ();
            
            var ar = SplitStringToTrimmedStrings(inputFieldExclude.text);
            /*
				text.Split (separator, StringSplitOptions.RemoveEmptyEntries);

			ar = Trim (ar);
*/
            items.GetComponent<BItems>().setExclude(ar);

            scrollRect.GetComponent<BScrollRect>().setExclude(ar);
        }

        string[] Trim(string[] ar)
        {
            //values.ConvertInPlace(x => x.Trim ());
            return Array.ConvertAll(ar, x => x.Trim());
        }

        string[] SplitStringToTrimmedStrings(string str)
        {
            str = str.Trim().ToLower();
			
            var ar = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			
            return Trim(ar);
        }

        public void OnValueChangeTogglePause()
        {
            paused = togglePause.isOn;
        }

        public void OnValueChangeTogglePinAll()
        {
            items.GetComponent<BItems>().OnValueChangeTogglePinAll(togglePinAll.isOn);
        }

        public void OnClickButtonShow()
        {
            Visible = true;
        }

        public void OnClickButtonHide()
        {
            Visible = false;
        }

        public bool Visible
        {
            get
            {
                return VisibleContentGameObject.activeSelf;
            }

            set
            {

                if (value && !VisibleContentGameObject.activeSelf)
                {

                    VisibleContentGameObject.SetActive(true);

                    inputFieldInclude.Select();
                    inputFieldInclude.ActivateInputField();

                    togglePause.isOn = paused;

                    RestoreItems();
                } else
                {
                    if (!value)
                    {
                        VisibleContentGameObject.SetActive(false);
                    }
                }

                buttonShow.gameObject.SetActive(!value);
            }
        }

        void RestoreItems()
        {
            items.GetComponent<BItems>().RestoreItems(list);
        }


        GameObject VisibleContentGameObject
        {
            get
            {
//				return transform.GetChild (0).gameObject;
                return visibleContent.gameObject;
            }
        }

        public bool Paused
        {
            set
            {
                paused = value;
                if (togglePause.IsActive())
                {
                    togglePause.isOn = value;
                }
            }
            get
            {
                return paused;
            }
        }


        /*
		public void OnPointerDown (PointerEventData eventData) 
		{
			//Grab the number of consecutive clicks and assign it to an integer varible.
			int i = eventData.clickCount;
			//Display the click count.
			Debug.Log (i);
		}
		*/

        //		public void OnPointerClick (BaseEventData eventData PointerEventData eventData)

    }
}
