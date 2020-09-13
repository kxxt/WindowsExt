using System;

namespace WindowsExt.Lib.SystemCommandShell
{
	public struct MkLinkResult
	{
		public Exception Exception;
		public bool IsSuccessful;
		public string FailedStep;
	}
}