using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsExt.Lib.SystemCommandShell
{
	public class MkLinkWrapper
	{
		//FileInfo file;
		//DirectoryInfo directory;
		string source;
		string link;
		bool isFile;
		bool isSource;
		public static MkLinkParameter FileParameter=MkLinkParameter.SymbolicLink;
		public static MkLinkParameter DirectoryParameter = MkLinkParameter.Directory;
		public static bool CopyAndDeleteInsteadOfMove=false;
		public void Initialize(string filename, bool isSource)
		{
			if (isSource)
			{
				this.isSource = true;
				if (File.Exists(filename))
				{
					source = filename;
					isFile = true;
					//file = new FileInfo(source);
				}
				else if (Directory.Exists(filename))
				{
					source = filename;
					isFile = false;
					//directory = new DirectoryInfo(filename);
				}
			}
			else
			{
				this.isSource = false;
				link = filename;
			}
		}
		public MkLinkWrapper(string filename,bool isSource)
		{
			Initialize(filename, isSource);
		}
		private MkLinkParameter GetParameter(MkLinkParameter? filePara,MkLinkParameter? dirPara)
		{
			filePara ??= FileParameter;
			dirPara ??= DirectoryParameter;
			return isFile ? FileParameter : DirectoryParameter;
		}
		public MkLinkResult MkLinkTo(string link,bool overwrite, MkLinkParameter? filePara = null, MkLinkParameter? dirPara = null)
		{
			if (!isSource) throw new NotSupportedException("Wrapper instance created using link is not supported for this function");
			return MkLink.Execute(link, source, GetParameter(filePara,dirPara), overwrite);
		}
		// from : https://code.4noobz.net/c-copy-a-folder-its-content-and-the-subfolders/
		private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
		{
			Directory.CreateDirectory(target.FullName);

			// Copy each file into the new directory.
			foreach (FileInfo fi in source.GetFiles())
			{
				//Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
				fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				DirectoryInfo nextTargetSubDir =
					target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir);
			}
		}
		public MkLinkResult MoveAndMkLinkHere(string link,bool mix,bool? copyAndDelete=null,MkLinkParameter? filePara=null,MkLinkParameter? dirPara=null)
		{
			copyAndDelete ??= CopyAndDeleteInsteadOfMove;
			
			if (!isSource) throw new NotSupportedException("Wrapper instance created using link is not supported for this function");
			try
			{
				if (isFile)
				{
					if (copyAndDelete.Value)
					{
						File.Copy(source, link);
						File.Delete(source);
					}
					else
					File.Move(source, link);
				}
				else
				{
					if (copyAndDelete.Value)
					{
						DirectoryInfo di = new DirectoryInfo(source);
						CopyAll(di, new DirectoryInfo(link));
					}
					else Directory.Move(source, link);
				}
			}
			catch(Exception e)
			{
				return new MkLinkResult() { IsSuccessful = false, FailedStep = MkLinkFailedStep.FileOperationBeforeAll, Exception = e };

			}
			return MkLink.Execute(source, link, GetParameter(filePara,dirPara));
		}
		public MkLinkResult MkLinkHere(string source,bool overwrite,MkLinkParameter? filePara= null, MkLinkParameter? dirPara = null)
		{
			if (isSource) throw new NotSupportedException("Wrapper instance created using source is not supported for this function");
			if (File.Exists(source)) isFile = true;
			else if (Directory.Exists(source)) isFile = false;
			else
			{
				// File Not Found , do not throw exception here.
				isFile = true;
				source = "";
			}
			return MkLink.Execute(link, source, GetParameter(filePara, dirPara), overwrite);
		}
	}
}
