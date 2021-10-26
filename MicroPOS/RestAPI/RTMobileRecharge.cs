using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000064 RID: 100
	public class RTMobileRecharge
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000EA64 File Offset: 0x0000CC64
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		public string ddd { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000EA75 File Offset: 0x0000CC75
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000EA7D File Offset: 0x0000CC7D
		public string number { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000EA86 File Offset: 0x0000CC86
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000EA8E File Offset: 0x0000CC8E
		public string recharge_network { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000EA97 File Offset: 0x0000CC97
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000EA9F File Offset: 0x0000CC9F
		public int type { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000EAA8 File Offset: 0x0000CCA8
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
		public double recharge_amount { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000EAB9 File Offset: 0x0000CCB9
		// (set) Token: 0x06000280 RID: 640 RVA: 0x0000EAC1 File Offset: 0x0000CCC1
		public string tef_id { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000EACA File Offset: 0x0000CCCA
		// (set) Token: 0x06000282 RID: 642 RVA: 0x0000EAD2 File Offset: 0x0000CCD2
		public double total_paid { get; set; }
	}
}
