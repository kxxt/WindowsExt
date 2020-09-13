using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExt.Lib.SystemCommandShell
{
	public enum MkLinkFailedStep
	{
		FileOperationBeforeAll,ValidateLink,ValidateTarget,ValidateParameter,
		StartMkLinkAndWaitForExit,CheckExitCode,None,DeleteExistedFile
	}
}
