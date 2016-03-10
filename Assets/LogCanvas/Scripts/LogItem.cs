using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Diagnostics;
using Flashunity.Screenshots;

namespace Flashunity.Logs
{
	public class LogItem : IComparer<LogItem>
	{
		readonly public int number;

		readonly public int type;
		readonly public string typeName;
	
		readonly public DateTime dateTime;
		readonly public LogString labelDateTime;
	
		readonly public LogString reflection;

		readonly public LogString label;

		public bool pin;

		public Texture2D screenshot;

	
		public LogItem (string label, int type, string color, int screenshotType, int reflection, int number)
		{
			this.number = number;
			this.type = type;

			this.typeName = LogType.GetName (type);

			this.label = new LogString (label, color);

		
			dateTime = DateTime.Now;
		
			labelDateTime = new LogString (dateTime.ToString ("mm:ss.fff"));

			this.reflection = new LogString (reflection > 0 ? GetReflection (new System.Diagnostics.StackTrace (), reflection) : "", color);

			AddScreenshot (screenshotType);
		}

		void AddScreenshot (int screenshotType)
		{
			if ((screenshotType & ScreenshotType.FILE) != 0) {
				ScreenshotBehaviour.instance.ToPNG (GetScreenshotFilename (), (screenshotType & ScreenshotType.SHOW_LOG) != 0);
			}
			
			if ((screenshotType & ScreenshotType.MEMORY) != 0) {
				ScreenshotBehaviour.instance.ToTexture (ref screenshot, (screenshotType & ScreenshotType.SHOW_LOG) != 0);
			}
		}

		string GetScreenshotFilename ()
		{
			return number.ToString () + "_" + dateTime.ToString ("hh-mm-ss.fff") + "_" + typeName + "_" + reflection.shorter + "_" + label.shorter + ".png";
		}


		string GetReflection (StackTrace stackTrace, int reflection)
		{
			string r = "";

			StackFrame[] frames = stackTrace.GetFrames ();
			
			for (int i=3; i<reflection+3 && i<frames.Length; i++) {
				
				StackFrame frame = frames [i];
				MethodBase method = frame.GetMethod ();
				string methodName = method.Name;
				Type methodsClass = method.DeclaringType;
				
				r += "-" + methodsClass.Name + "." + methodName + "()";
				r += "    ";
				//r += Environment.NewLine;
				//r += Environment.NewLine;
			}


			return r;
		}

	
		public override string ToString ()
		{
			return number.ToString () + "   " + labelDateTime + "   " + typeName + "   " + reflection + "   " + label;
		}
	
		public bool isIncludeAndExclude (string[] include, string[] exclude)
		{
			return ((label.isIncludeWords (include) || reflection.isIncludeWords (include) || labelDateTime.isIncludeWords (include)) && label.isExcludeWords (exclude) && reflection.isExcludeWords (exclude) && labelDateTime.isExcludeWords (exclude));
		}
        

		//		public override int Compare (T x, T y)
		public int Compare (LogItem x, LogItem y)
		{
			return -1;
		}
	
		/*
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			Part objAsPart = obj as Part;
			if (objAsPart == null)
				return false;
			else
				return Equals (objAsPart);
		}
		public override int GetHashCode ()
		{
			return PartId;
		}
		public bool Equals (Part other)
		{
			if (other == null)
				return false;
			return (this.PartId.Equals (other.PartId));
		}
	*/
		// Should also override == and != operators.
	
	}
}