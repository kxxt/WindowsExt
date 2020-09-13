using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExt.Lib.SystemCommandShell
{
    public static class MkLink
    {
		public static MkLinkResult Execute(string link, string target, MkLinkParameter para,bool overwriteExistedFile=false)
		{
			//throw new NotImplementedException();
			var lnkV = ValidateLink(link,overwriteExistedFile);
			if (lnkV.Item1 != null)
			{
				return new MkLinkResult() { IsSuccessful = false, Exception = lnkV.Item1, FailedStep = "Validate Link" };
			}
			var tarV = ValidateTarget(target);
			if (tarV.Item1 != null)
			{
				return new MkLinkResult() { IsSuccessful = false, Exception = tarV.Item1, FailedStep = "Validate Target" };
			}
			var paVal = ValidateParameter(para, tarV.Item3);
			if (paVal != null)
			{
				return new MkLinkResult() { IsSuccessful = false, Exception = paVal, FailedStep = "Validate Parameter" };
			}
			_mkLinkCommand.Arguments = GenerateArguments(lnkV.Item2, tarV.Item2, para);
			Process p;
			try
			{
				p = Process.Start(_mkLinkCommand);
				p.WaitForExit();
			}
			catch (Exception e)
			{
				return new MkLinkResult() { IsSuccessful = false, Exception = e, FailedStep = "Start Mklink Or Wait for its exit" };
			}
			if (p.ExitCode == 0) return new MkLinkResult() { IsSuccessful = true, Exception = null, FailedStep = null };
			return new MkLinkResult() { IsSuccessful = false,Exception=new Exception($"MkLink utility exited with code {p.ExitCode} (Not Zero).") ,FailedStep="Validate Exit Code"};
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <returns>last bool value means it's a file if its value is true</returns>
		private static (Exception,string,bool) ValidateTarget(string target)
		{
			if (File.Exists(target)) return (null,new FileInfo(target).FullName,true);
			if (Directory.Exists(target)) return (null,new DirectoryInfo(target).FullName,false);
			return (new FileNotFoundException("Target Not Found",target),null,false);
		}

		private static (Exception,string) ValidateLink(string link,bool overwrite=false)
		{
			FileInfo fi;
			try
			{
				fi = new FileInfo(link);
				if (fi.Exists&&!overwrite)
				{
					return (new Exception("File already exists!"),null);
				}
				if (!fi.Directory.Exists)
				{
					fi.Directory.Create();
				}
				return (null,fi.FullName);
			}
			catch(Exception e)
			{
				return (e,null);
			}
		}

		private static WarningException ValidateParameter(MkLinkParameter para,bool isFile)
		{
            if (para == (MkLinkParameter.Directory | MkLinkParameter.HardLink)) return new WarningException("This parameter combination may fail.");
			if (
				(para.HasFlag(MkLinkParameter.Directory) || para.HasFlag(MkLinkParameter.Junction))
				&& isFile
				)
				return new WarningException("The link made probably doesn't work.Don't make directory/junction link for files.");
			if(!isFile&&
				(
				!para.HasFlag(MkLinkParameter.Directory)
				&&
				!para.HasFlag(MkLinkParameter.Junction)
				)
				)
				return new WarningException("The link made probably doesn't work.You should make directory/junction link for directories.");
			return null;
		}

		private static ProcessStartInfo _mkLinkCommand = new ProcessStartInfo()
		{
			FileName = "cmd",
			WindowStyle = ProcessWindowStyle.Hidden,
			UseShellExecute = true,
		};

		private static string GenerateArguments(string link,string target,MkLinkParameter para)
		{
			string parameter = para.GetStringForm();
			return $"/c mklink {parameter} {link} {target}";
		}
    }
}
