using UnityEngine;
using System.Collections;
using Flashunity.Screenshots;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Flashunity.Logs
{
    public class Log
    {
        //        public static bool showProperties = false;

        public static void Add(string label = "", string color = "", int type = LogType.LOG, int screenshotType = ScreenshotType.NONE, int reflection = -1)
        {
            if (BCanvas.instance)
            {
                BCanvas.instance.Add(label, type, color, screenshotType, reflection);
            }
        }

        public static void Error(string label = "", int screenshotType = ScreenshotType.MEMORY, int reflection = -1)
        {
            if (BCanvas.instance)
            {
                BCanvas.instance.Add(label, LogType.ERROR, "red", screenshotType, reflection);
            }
        }

        public static void Warning(string label = "", int screenshotType = ScreenshotType.MEMORY, int reflection = -1)
        {
            if (BCanvas.instance)
            {
                BCanvas.instance.Add(label, LogType.WARNING, "orange", screenshotType, reflection);
            }
        }

        public static void Remove(string label, string reflection = "")
        {
            if (BCanvas.instance)
            {
                BCanvas.instance.Remove(label, reflection);
            }
        }

        public static void Clear()
        {
            if (BCanvas.instance)
            {
                BCanvas.instance.Clear();
            }
        }

        public static bool Paused
        {
            set
            {
                if (BCanvas.instance)
                {
                    BCanvas.instance.Paused = value;
                }
            }
            get
            {
                if (BCanvas.instance)
                {
                    return BCanvas.instance.Paused;
                }
                return false;
            }
        }

        public static bool Visible
        {
            set
            {
                if (BCanvas.instance)
                {
                    BCanvas.instance.Visible = value;
                }
            }
            get
            {
                if (BCanvas.instance)
                {
                    return BCanvas.instance.Visible;
                }
                return false;
            }
        }

        public static int Count
        {
            get
            {
                if (BCanvas.instance)
                {
                    return BCanvas.instance.Count;
                }
                return 0;
            }
        }

        /*
        public static string Arr(<T>[] ar)
        {
            
        }
        */

        public static string Arr(object[] ar, bool props = false)
        {
            var str = "";
            for (int i = 0; i < ar.Length; i++)
            {
                str += props ? Props(ar [i]) : ar [i];
            }
            return str;
        }

        public static string GetPair(object key, object value, bool props = false)
        {
            var str = "";
            str += "key: " + key + ", value: ";
            str += props ? Props(value) : value;
            return str;
        }

        /*

//        public static string Dic(IDictionary<object, object> dic, bool props = false)
        {
            var str = "";

            foreach (var pair in dic)
            {
                str += System.Environment.NewLine + "key: " + pair.Key + ", value: ";
                str += props ? Props(pair.Value) : pair.Value;
            }
            return str;
        }
        */


        //        public static string GetProperties(object obj, bool skipEmpty = false, int depth = 1, int tab = 0)
        public static string Props(object obj, bool skipEmpty = false, string[] include = null, string[] exclude = null)
        {
            var str = "";

            if (obj != null)
            {   
                Type type = obj.GetType();
            
                PropertyInfo[] properties = type.GetProperties();

                /*
            if (properties.Length == 1)
            {
                return obj.ToString();
            }
*/

                //type.Name + ".";

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetIndexParameters().Length == 0)
                    {
                        if (str.Length == 0)
                        {
                            str = type.Name + ".";
                        }

                        object val = null;

                        try
                        {
                            val = property.GetValue(obj, null);
                        } catch
                        {
                            
                        }


                        string valStr = val != null ? val.ToString() : "";

                        if (!skipEmpty || valStr != "")
                        {
                            str += System.Environment.NewLine + property.Name + " : " + valStr;
                        } 

                    } else
                    {
                        str += obj.ToString();
                    }
                }
            } else
            {
                str = "obj = null";
            }

            return str;
        }

    }
}
