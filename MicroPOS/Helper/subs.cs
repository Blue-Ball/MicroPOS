using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroPOS.Helper
{
    class subs
    {
        public static string getBetween(string strSource, string strBegin, string strEnd)
        {




            strSource = strSource.Replace("\n", "");
            strSource = strSource.Replace("\r", "");
            strSource = strSource.Replace("\t", "");

            try
            {
                if (strSource.Length < strBegin.Length || strSource.Length < strEnd.Length)
                    return null;

                int startIndex = strSource.IndexOf(strBegin) + strBegin.Length;
                if (startIndex == -1)
                    return null;

                int endIndex = strSource.IndexOf(strEnd, startIndex);

                return strSource.Substring(startIndex, endIndex - startIndex);
            }
            catch (AggregateException x)
            {
                Console.WriteLine("Err getBetween");
            }
            catch (Exception x)
            {
                //lock (Global.lockerLogFile)
                //    File.AppendAllText("DebugLog.txt", "Error: getBetween" + x.Message + Environment.NewLine);
                Console.WriteLine("Error: getBetween" + x.Message);
            }
            return null;
        }

    }
}
