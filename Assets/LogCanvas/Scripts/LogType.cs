using System.Collections;
namespace Flashunity.Logs
{
	public class LogType
	{
		public const int LOG = 0;
		public const int WARNING = 1;
		public const int ERROR = 2;

		public static string GetName (int type)
		{
			switch (type) {
			case LOG:
				return "LOG";
			case WARNING:
				return "WARNING";
			case ERROR:
				return "ERROR";
			}

			return "";
		}

	}
}
