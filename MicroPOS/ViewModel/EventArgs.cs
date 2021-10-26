using System;

namespace MicroPOS.ViewModel
{
	// Token: 0x02000047 RID: 71
	public class EventArgs<T> : EventArgs
	{
		// Token: 0x0600017B RID: 379 RVA: 0x0000B1F2 File Offset: 0x000093F2
		public EventArgs(T value)
		{
			this.Value = value;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000B201 File Offset: 0x00009401
		// (set) Token: 0x0600017D RID: 381 RVA: 0x0000B209 File Offset: 0x00009409
		public T Value { get; private set; }
	}
}
