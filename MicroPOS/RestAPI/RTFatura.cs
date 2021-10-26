using System;
using MicroPOS.Helper;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000068 RID: 104
	public class RTFatura
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000EBC9 File Offset: 0x0000CDC9
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000EBD1 File Offset: 0x0000CDD1
		public bool select { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000EBDA File Offset: 0x0000CDDA
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000EBE2 File Offset: 0x0000CDE2
		public string id_fatura { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000EBEB File Offset: 0x0000CDEB
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000EBF3 File Offset: 0x0000CDF3
		public string vencimento_fatura { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000EBFC File Offset: 0x0000CDFC
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000EC04 File Offset: 0x0000CE04
		public double valor_fatura { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000EC0D File Offset: 0x0000CE0D
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000EC15 File Offset: 0x0000CE15
		public bool overdue { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000EC20 File Offset: 0x0000CE20
		public string DecoDate
		{
			get
			{
				DateTime now = DateTime.Now;
				if (DateTime.TryParse(this.vencimento_fatura, out now))
				{
					return now.ToString("dd/MM/yyyy");
				}
				return "";
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000EC54 File Offset: 0x0000CE54
		public string DecoValue
		{
			get
			{
				return PosHelper.GetCurrencyString(this.valor_fatura);
			}
		}
	}
}
