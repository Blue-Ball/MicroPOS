using System;

namespace MicroPOS.Helper
{
	// Token: 0x0200007D RID: 125
	public class ReceiptItem
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0000FE64 File Offset: 0x0000E064
		public ReceiptItem(string name, string value, string description = "", bool isValRect = false)
		{
			this.name = name;
			this.value = value;
			this.sub_desc = description;
			this.isValRect = isValRect;
		}

		// Token: 0x040002C8 RID: 712
		public readonly string name;

		// Token: 0x040002C9 RID: 713
		public readonly string sub_desc;

		// Token: 0x040002CA RID: 714
		public readonly string value;

		// Token: 0x040002CB RID: 715
		public readonly bool isValRect;
	}
}
