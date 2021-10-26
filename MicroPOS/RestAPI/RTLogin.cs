using System;

namespace MicroPOS.RestAPI
{
	// Token: 0x02000061 RID: 97
	public class RTLogin
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000E9DC File Offset: 0x0000CBDC
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000E9E4 File Offset: 0x0000CBE4
		public string access_token { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000E9ED File Offset: 0x0000CBED
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000E9F5 File Offset: 0x0000CBF5
		public string token_type { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000E9FE File Offset: 0x0000CBFE
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000EA06 File Offset: 0x0000CC06
		public int expires_in { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000EA0F File Offset: 0x0000CC0F
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000EA17 File Offset: 0x0000CC17
		public string error { get; set; }
	}
}
