using System;

namespace MicroPOS.Helper
{
	// Token: 0x0200007A RID: 122
	public struct KBDLLHOOKSTRUCT
	{
		// Token: 0x040002C1 RID: 705
		public int vkCode;

		// Token: 0x040002C2 RID: 706
		private int scanCode;

		// Token: 0x040002C3 RID: 707
		public int flags;

		// Token: 0x040002C4 RID: 708
		private int time;

		// Token: 0x040002C5 RID: 709
		private int dwExtraInfo;
	}
}
