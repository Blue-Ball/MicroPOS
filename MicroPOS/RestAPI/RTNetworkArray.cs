using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000062 RID: 98
	public class RTNetworkArray
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000EA20 File Offset: 0x0000CC20
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000EA28 File Offset: 0x0000CC28
		public string[] network { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000EA31 File Offset: 0x0000CC31
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000EA39 File Offset: 0x0000CC39
		public RTFee[] fee { get; set; }
	}
}
