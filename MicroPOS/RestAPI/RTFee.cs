using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000063 RID: 99
	public class RTFee
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000EA42 File Offset: 0x0000CC42
		// (set) Token: 0x06000271 RID: 625 RVA: 0x0000EA4A File Offset: 0x0000CC4A
		public int fixed_amount { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000EA53 File Offset: 0x0000CC53
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000EA5B File Offset: 0x0000CC5B
		public int percentual_amount { get; set; }
	}
}
