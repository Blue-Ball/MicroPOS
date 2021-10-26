using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000069 RID: 105
	public class RTPaidFatura
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000EC61 File Offset: 0x0000CE61
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000EC69 File Offset: 0x0000CE69
		public string id_fatura { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000EC72 File Offset: 0x0000CE72
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000EC7A File Offset: 0x0000CE7A
		public double valor_fatura { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000EC83 File Offset: 0x0000CE83
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000EC8B File Offset: 0x0000CE8B
		public string tef_id { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000EC94 File Offset: 0x0000CE94
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		public double total_paid { get; set; }
	}
}
