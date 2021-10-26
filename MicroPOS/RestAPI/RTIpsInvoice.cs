using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000067 RID: 103
	public class RTIpsInvoice
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000EB63 File Offset: 0x0000CD63
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000EB6B File Offset: 0x0000CD6B
		public string id { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000EB74 File Offset: 0x0000CD74
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000EB7C File Offset: 0x0000CD7C
		public string nome_razao { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000EB85 File Offset: 0x0000CD85
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000EB8D File Offset: 0x0000CD8D
		public string doc { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000EB96 File Offset: 0x0000CD96
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000EB9E File Offset: 0x0000CD9E
		public float total_amount { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000EBA7 File Offset: 0x0000CDA7
		// (set) Token: 0x0600029F RID: 671 RVA: 0x0000EBAF File Offset: 0x0000CDAF
		public RTFatura[] faturas { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x0000EBC0 File Offset: 0x0000CDC0
		public RTFee[] fee { get; set; }
	}
}
