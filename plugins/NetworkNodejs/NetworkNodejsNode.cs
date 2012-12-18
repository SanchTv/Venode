#region usings
using System;

using System.IO;
using System.Text;
using System.ComponentModel.Composition;
using System.Diagnostics;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Nodejs", Category = "Network", Help = "Basic template with one string in/out", Tags = "")]
	#endregion PluginInfo
	public class NetworkNodejsNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("start node.js",IsSingle = true)]
		ISpread<bool> Fstart;
		[Input("stop node.js",IsSingle = true)]
		ISpread<bool> Fstop;
		[Input("NodeApp",IsSingle = true, StringType = StringType.Filename)]
		ISpread<string> FNodeApp;
		[Input("ShowNowindow",IsSingle = true)]
		ISpread<bool> FWin;
		[Output("Output")]
		ISpread<string> FState;
        [Output("Error")]
		ISpread<string> FError; 
		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		Process cmd = new Process();
		

		
	    int running = 0;
		
		public void Evaluate(int SpreadMax)
		{
			FState.SliceCount = 1;
			FError.SliceCount = 1;
			var filename = Path.GetFileName(FNodeApp[0]);
			var path = Path.GetDirectoryName(FNodeApp[0]);
			

          
		
			if(Fstart[0]==true){
			
		    running=1;
			cmd.StartInfo.CreateNoWindow = FWin[0];
			cmd.StartInfo.FileName = "cmd.exe";
		    cmd.StartInfo.UseShellExecute = false;
			//cmd.StartInfo.RedirectStandardOutput = true;
			
			
			cmd.StartInfo.WorkingDirectory = path;
			cmd.StartInfo.Arguments = "/C " +"node "+ filename;
				
			try
			{  
				foreach (Process proc in Process.GetProcessesByName("node"))
           		 {
                proc.Kill();
           		 }
			}
            catch(Exception ex)
            {
          	FError[0]= ex.Message;
            }
				try
			{ 
			cmd.Start();
				
				}
            catch(Exception ex)
            {
            FError[0]= ex.Message;
            }

            }
		if(Fstop[0]==true){
		   running=0;
			try
			{  
				foreach (Process proc in Process.GetProcessesByName("node"))
           		 {
                proc.Kill();
           		 }
			}
            catch(Exception ex)
            {
          	FError[0]= ex.Message;
            }
             	

            }
	/*if(running==1){   ////tryng to get log from cmd here...
			 while (true)
        {
            byte[] buffer = new byte[256];
            var ar = cmd.StandardOutput.BaseStream.BeginRead(buffer, 0, 256, null, null);
            ar.AsyncWaitHandle.WaitOne();
            var bytesRead = cmd.StandardOutput.BaseStream.EndRead(ar);
            if (bytesRead > 0)
            {
                FOutput[0]=Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            else
            {
                cmd.WaitForExit();
                break;
            }
        } 
			}*/
			
			if(running==1) FState[0] = " server running ";
            else FState[0] = " server stopped ";FState[0]="";
			
		}
	}
}
