#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using VVVV.Core.Logging;
using System.Collections.Generic;

#endregion usings

namespace VVVV.Nodes
{
	

	
	
	[PluginInfo(Name = "Json", Category = "String", Help = "Basic template with one string in/out", Tags = "")]
	public class StringJsonNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("JSON", DefaultString = "hello")]
		IDiffSpread<string> FInput;

		

		
	
		
		
	    [Output("Output json")]
		ISpread<JObject> FJOutput;
		
		 [Output("status")]
		ISpread<string> FStatu;
	
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		//called when data for any output pin is requested
		
        
		
		public void Evaluate(int SpreadMax)
		{
		 
	
	if(FInput.IsChanged ){

	  try 
    {
    		 FJOutput.SliceCount = 1;
		  FStatu.SliceCount = 1;
           FJOutput[0] = JObject.Parse(FInput[0]);
		   FStatu[0] ="ok";
    }
        catch(Exception  e) 
    {
    	 FJOutput[0] = null;
    	 FJOutput.SliceCount = 0;
    	 FStatu.SliceCount = 1;
    	 FStatu[0] =e.ToString();;
    } 
           
        
		 
		  
	}
         
     
}

	}


	
	
	
    [PluginInfo(Name = "JObject", Category = "String", Help = "Basic template with one string in/out", Tags = "")]
	public class JObjectNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("JObject")]
		ISpread<JObject> FInput;

	    [Input("path")]
		ISpread<string> FInputp;
		
	
		[Output("Output")]
		ISpread<string> FOutput;

		[Output("Status")]
		ISpread<string> FStatus;
		
		
	
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		//called when data for any output pin is requested
		
        
		
		public void Evaluate(int SpreadMax)
		{
		 
		  
	    if( FInput.SliceCount!=0){
		  FOutput.SliceCount = SpreadMax;
		  FStatus.SliceCount = SpreadMax;
		for (int i = 0; i < FInputp.SliceCount; i++)
          	
          	{
          	try 
            {
            	if( FInput[0].SelectToken(FInputp[i])!= null){
            		
          		    FOutput[i]=  FInput[0].SelectToken(FInputp[i]).ToString();
            		FStatus[i]= "ok";
            		
              	}else{
              		
              		FOutput[i]= "";
              		FStatus[i]= "no token found";
              		
              	}
            	    
          	 }
         	 catch(Exception e) 
          	 {
    	        FOutput[i]= "";
    	        FStatus[i]= e.ToString();
          	 	FOutput.SliceCount = SpreadMax;
		        FStatus.SliceCount = SpreadMax;
          	 } 	
           
           } 
		  
		}else{
		 
		  FOutput.SliceCount = 0;
		  FStatus.SliceCount = 1;	
		  FStatus[0]= "null";	
		}
	


	}
}
	
	
	
	
	
	
    [PluginInfo(Name = "JArray", Category = "String", Help = "Basic template with one string in/out", Tags = "")]
	public class JArray : IPluginEvaluate
	{
		#region fields & pins
		[Input("Jobject")]
		ISpread<JObject> FInput;

	    [Input("path")]
		ISpread<string> FInputp;
		
	   [Input("key1")]
		ISpread<string> FInputk;
	
		[Input("key2")]
		ISpread<string> FInputk2;
		
		[Output("Output")]
		ISpread<string> FOutput;
		[Output("Output2")]
		ISpread<string> FOutput2;
		[Output("Status")]
		ISpread<string> Fstatu;
        [Output("count")]
		ISpread<int> FcOutput;
		
	   // [Output("Output json")]
		//ISpread<Vson> FJOutput;
		
		
	
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		//called when data for any output pin is requested
		
        
		
		public void Evaluate(int SpreadMax)
		{ int i = 0;
		
		try{	
		 if(FInput.IsChanged ){
          
			
		   if( FInput.SliceCount!=0  ){
		   	 if( FInput[0]!=null  ){
          		if( FInput[0].SelectToken(FInputp[0])!= null){
          			
          		   	var results  = FInput[0].SelectToken(FInputp[0]);
          			
          				foreach (JToken child in results.Children())
							{
							FOutput.SliceCount = i+1;
					        FOutput2.SliceCount = i+1;
					        FcOutput.SliceCount = 1;
          			        Fstatu.SliceCount = i+1;
							FcOutput[0] = i;
					        FOutput[i] = child.SelectToken(FInputk[0]).ToString();
	      					FOutput2[i] = child.SelectToken(FInputk2[0]).ToString();
							Fstatu[i]	= "ok";
           					i++;
          					}
                    
			        
          	 	 
          			
          			
          			
          		}
		   		
		   }	}else{ 
		   		
		   	FOutput.SliceCount = 0;
			FOutput2.SliceCount = 0;
		    FcOutput.SliceCount = 0;
		   	Fstatu.SliceCount = 1;	
		   	Fstatu[0]= "null";
		   	
		 }
			}}
          			catch(Exception e) 
         		 	{
    	    	    Fstatu.SliceCount = 1;
					Fstatu[0]= e.ToString();
         		 	FOutput2.SliceCount = 0;
		            FcOutput.SliceCount = 0;
         		 	FOutput.SliceCount = 0;
         		   
         		 	
          			} 	
		
	}
	
	
	}
	
	
	
	
}
