using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using WindowsExt.Lib;
using WindowsExt.Lib.SystemCommandShell;

namespace PlayGround
{
	static class Program
	{
		static void Main(string[] args)
		{
			//TestEnumMkLinkParameter();
			TestClassMkLink();
			ReadLine();
		}
		static void PrintEnumInfo(MkLinkParameter para)
		{
			Write("name:");
			WriteLine(para);
			Write("value:");
			WriteLine((int)(para));
		}
		static void PrintMkLinkResult(this MkLinkResult r)
		{
			WriteLine("-----");
			WriteLine(r.IsSuccessful);
			WriteLine(r.FailedStep);
			if(r.Exception!=null)
			WriteLine(RsWork.Functions.Log.Logger.GetExceptionInfo(r.Exception));
		}
		static void TestClassMkLink()
		{
			WriteLine("---- Test MkLink Class ----");
			MkLink.Execute("A-D", "A", MkLinkParameter.Directory).PrintMkLinkResult();
			MkLink.Execute("P-S", "PlayGround.exe", MkLinkParameter.SymbolicLink).PrintMkLinkResult();
			MkLink.Execute("P-SH", "PlayGround.exe", MkLinkParameter.HardLink).PrintMkLinkResult();
			MkLink.Execute("A-J", "A", MkLinkParameter.Junction).PrintMkLinkResult();
			MkLink.Execute("A-JD", "A", MkLinkParameter.Junction | MkLinkParameter.Directory).PrintMkLinkResult();
			MkLink.Execute("A-JH", "A", MkLinkParameter.Junction|MkLinkParameter.HardLink).PrintMkLinkResult();
			MkLink.Execute("A-JHD", "A", MkLinkParameter.Junction | MkLinkParameter.HardLink|MkLinkParameter.Directory).PrintMkLinkResult();
			MkLink.Execute("A-HD", "A", MkLinkParameter.Directory|MkLinkParameter.HardLink).PrintMkLinkResult();

		}
		static void TestEnumMkLinkParameter()
		{
			WriteLine("---- Test MkLink Parameter ----");
			//MkLinkParameter p1, p2, p3;
			PrintEnumInfo(MkLinkParameter.Directory);
			PrintEnumInfo(MkLinkParameter.HardLink);
			PrintEnumInfo(MkLinkParameter.Junction);
			PrintEnumInfo(MkLinkParameter.SymbolicLink);
			PrintEnumInfo(MkLinkParameter.SymbolicLink | MkLinkParameter.Directory);
			WriteLine("---- Ended ----");
		}
	}
}
