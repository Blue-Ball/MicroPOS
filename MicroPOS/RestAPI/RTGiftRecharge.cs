using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000065 RID: 101
	public class RTGiftRecharge
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000EADB File Offset: 0x0000CCDB
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000EAE3 File Offset: 0x0000CCE3
		public string recharge_network { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000EAEC File Offset: 0x0000CCEC
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		public int type { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000EAFD File Offset: 0x0000CCFD
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000EB05 File Offset: 0x0000CD05
		public double recharge_amount { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000EB0E File Offset: 0x0000CD0E
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000EB16 File Offset: 0x0000CD16
		public string tef_id { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000EB1F File Offset: 0x0000CD1F
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000EB27 File Offset: 0x0000CD27
		public double total_paid { get; set; }
	}
}
