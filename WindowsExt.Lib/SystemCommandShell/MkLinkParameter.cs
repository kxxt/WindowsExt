using System;
using System.Text;

namespace WindowsExt.Lib.SystemCommandShell
{
	[Flags]
	public enum MkLinkParameter
	{
		SymbolicLink=1,
		HardLink =2,
		Junction=4,
		Directory=8
	}
	public static class MkLinkParameterExtension
	{
		public static string GetStringForm_Single(this MkLinkParameter para)
		{
			return para switch { 
				MkLinkParameter.Directory => "/D" ,
				MkLinkParameter.HardLink=>"/H",
				MkLinkParameter.Junction=>"/J",
				MkLinkParameter.SymbolicLink=>"",
				_=>throw new ArgumentException("`para` can't be combinations",nameof(para))
			};
		}
		public static string GetStringForm(this MkLinkParameter para)
		{
			//{
			//	if (para.HasFlag(MkLinkParameter.Directory))
			//	{
			//		if (para.HasFlag(MkLinkParameter.Junction))
			//		{
			//			return "/D /J";
			//		}
			//		else if(para.HasFlag(MkLinkParameter.HardLink))
			//		{
			//			return
			//		}
			//	}
			//	else
			//	{

			//	}
			bool hasFlagInBuffer = false;
			StringBuilder ret=new StringBuilder();
			if (para.HasFlag(MkLinkParameter.Directory))
			{
				ret.Append("/D");
				hasFlagInBuffer = true;
			}
			if (para.HasFlag(MkLinkParameter.HardLink))
			{
				if (hasFlagInBuffer)
				{
					ret.Append(' ');
					hasFlagInBuffer = false;
				}
				ret.Append("/H");
				hasFlagInBuffer = true;
			}
			if (para.HasFlag(MkLinkParameter.Junction))
			{
				if (hasFlagInBuffer)
				{
					ret.Append(' ');
					hasFlagInBuffer = false;
				}
				ret.Append("/J");
				hasFlagInBuffer = true;
			}
			return ret.ToString();
		}
	}
}