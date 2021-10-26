using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MicroPOS.Helper
{
	// Token: 0x0200007B RID: 123
	internal class InterceptKeys
	{
		// Token: 0x06000338 RID: 824 RVA: 0x0000FE00 File Offset: 0x0000E000
		public static IntPtr SetHook(InterceptKeys.LowLevelKeyboardProc proc)
		{
			IntPtr result;
			using (Process currentProcess = Process.GetCurrentProcess())
			{
				using (ProcessModule mainModule = currentProcess.MainModule)
				{
					result = InterceptKeys.SetWindowsHookEx(13, proc, InterceptKeys.GetModuleHandle(mainModule.ModuleName), 0U);
				}
			}
			return result;
		}

		// Token: 0x06000339 RID: 825
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, InterceptKeys.LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		// Token: 0x0600033A RID: 826
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		// Token: 0x0600033B RID: 827
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

		// Token: 0x0600033C RID: 828
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x040002C6 RID: 710
		private const int WH_KEYBOARD_LL = 13;

		// Token: 0x040002C7 RID: 711
		private const int WM_KEYDOWN = 256;

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x0600033F RID: 831
		public delegate IntPtr LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
	}
}
