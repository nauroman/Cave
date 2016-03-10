using UnityEngine;
using System.Collections;
using Flashunity.Logs;

namespace Flashunity.Screenshots
{
    public class ScreenshotBehaviour : MonoBehaviour
    {
        //	bool LogVisible;

        //LogItem logItem;

        //	Texture2D texture;


        private static ScreenshotBehaviour _instance;

        public static ScreenshotBehaviour instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<ScreenshotBehaviour>();
					
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
            /*
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else
            {
                if (this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }
            */
        }
        /*
		public void Add (int screenshotType, string filename = "", ref Texture2D texture)
		{
			if (screenshotType | ScreenshotType.FILE)
			{
				ToPNG(filename);
			}

			if (screenshotType | ScreenshotType.MEMORY)
			{
				ToTexture(texture);
			}
		}
		*/

        public void ToTexture(ref Texture2D texture, bool showLog = false)
        {
            bool logVisible = Log.Visible;

            /*
			if (!showLog) {
				Log.Visible = false;
			}
			*/

            StartCoroutine(TakeScreenshot(texture, logVisible));
        }

        // png
        public void ToPNG(string filename, bool showLog = false)
        {
            bool logVisible = Log.Visible;

            /*
			if (!showLog) {
				Log.Visible = false;
			}
*/

            StartCoroutine(TakeScreenshotPNG(filename, logVisible));
        }

        private IEnumerator TakeScreenshotPNG(string filename, bool logVisible)
        {
            yield return new WaitForEndOfFrame();

            Application.CaptureScreenshot(filename);

//			Log.Visible = logVisible;
        }

        private IEnumerator TakeScreenshot(Texture2D texture, bool logVisible)
        {
            yield return new WaitForEndOfFrame();
			
            var width = Screen.width;
            var height = Screen.height;
            texture = new Texture2D(width, height, TextureFormat.RGB24, false);

            // Read screen contents into the texture
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

//			Log.Visible = logVisible;

            /*
			byte[] screenshot = tex.EncodeToPNG ();
			
			var wwwForm = new WWWForm ();
			wwwForm.AddBinaryData ("image", screenshot, "InteractiveConsole.png");
			wwwForm.AddField ("message", "herp derp.  I did a thing!  Did I do this right?");
			FB.API ("me/photos", HttpMethod.POST, handleResult, wwwForm);
			*/
        }

    }
}
