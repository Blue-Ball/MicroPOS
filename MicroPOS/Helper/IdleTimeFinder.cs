using System;
using System.Runtime.InteropServices;

namespace MicroPOS.Helper
{
	// Token: 0x02000079 RID: 121
	public class IdleTimeFinder
	{
		// Token: 0x06000333 RID: 819
		[DllImport("User32.dll")]
		private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		// Token: 0x06000334 RID: 820
		[DllImport("Kernel32.dll")]
		private static extern uint GetLastError();

		// Token: 0x06000335 RID: 821 RVA: 0x0000FD80 File Offset: 0x0000DF80
		public static uint GetIdleTime()
		{
			LASTINPUTINFO lastinputinfo = default(LASTINPUTINFO);
			lastinputinfo.cbSize = (uint)Marshal.SizeOf<LASTINPUTINFO>(lastinputinfo);
			IdleTimeFinder.GetLastInputInfo(ref lastinputinfo);
			return (uint)(Environment.TickCount - (int)lastinputinfo.dwTime);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000FDB8 File Offset: 0x0000DFB8
		public static long GetLastInputTime()
		{
			LASTINPUTINFO lastinputinfo = default(LASTINPUTINFO);
			lastinputinfo.cbSize = (uint)Marshal.SizeOf<LASTINPUTINFO>(lastinputinfo);
			if (!IdleTimeFinder.GetLastInputInfo(ref lastinputinfo))
			{
				throw new Exception(IdleTimeFinder.GetLastError().ToString());
			}
			return (long)((ulong)lastinputinfo.dwTime);
		}
	}
}
