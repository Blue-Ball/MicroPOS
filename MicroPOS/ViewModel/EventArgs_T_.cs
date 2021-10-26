using System;
using System.Runtime.CompilerServices;

namespace MicroPOS.ViewModel
{
	public class EventArgs<T> : EventArgs
	{
		public T Value
		{
			get;
			private set;
		}

		public EventArgs(T value)
		{
			this.Value = value;
		}
	}
}