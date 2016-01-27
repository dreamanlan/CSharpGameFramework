using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DldUtil
{

public static class BigFileReader
{
	public static IEnumerable<string> ReadFile(string path, string seekText = "")
	{
		FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		BufferedStream bs = new BufferedStream(fs);
		StreamReader sr = new StreamReader(bs);

		string line = "";
		
		bool seekTextRequested = !string.IsNullOrEmpty(seekText);
		//bool seekTextFound = false;
		
		long seekTextFoundAtLine = -1;
		
		
		if (seekTextRequested)
		{
			long currentLine = 0;
			while (true)
			{
				++currentLine;
				line = sr.ReadLine();
				
				if (line == null)
				{
					break;
				}
				
				// if seekText not found yet, skip
				if (line.IndexOf(seekText) == -1)
				{
					continue;
				}
				
				seekTextFoundAtLine = currentLine;
					
				//Debug.Log("seeking: " + line);
				//Debug.Log("seekText found at: " + currentLine);
			}
			//Debug.Log("done seeking");
		
			if (seekTextFoundAtLine != -1)
			{
				/*sr.Close();
				bs.Close();
				fs.Close();
			
				fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				
				//fs.Seek(positionOfLastSeekText, SeekOrigin.Begin);
				fs.Position = positionOfLastSeekText;
				bs = new BufferedStream(fs);
				sr = new StreamReader(bs);*/
				
				fs.Seek(0, SeekOrigin.Begin);
				
				currentLine = 0;
				while (true)
				{
					++currentLine;
					line = sr.ReadLine();
					
					if (line == null)
					{
						break;
					}
					if (currentLine < seekTextFoundAtLine)
					{
						continue;
					}
					
					//Debug.Log("seeked: " + line);
					
					yield return line;
				}
			}
		}
		else
		{
			while (true)
			{
				line = sr.ReadLine();
				
				if (line == null)
				{
					break;
				}
				
				yield return line;
			}
		}
		
		line = "";
		
		sr.Close();
		bs.Close();
		fs.Close();
	}
}

}
