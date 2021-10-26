using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000066 RID: 102
	public class RTRechargeResponse
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000EB30 File Offset: 0x0000CD30
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000EB38 File Offset: 0x0000CD38
		public int code { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000EB41 File Offset: 0x0000CD41
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000EB49 File Offset: 0x0000CD49
		public string message { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000EB52 File Offset: 0x0000CD52
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000EB5A File Offset: 0x0000CD5A
		public string pin { get; set; }
	}
}
