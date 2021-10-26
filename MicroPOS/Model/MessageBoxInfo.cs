using System;

namespace MicroPOS.Model
{
	public class MessageBoxInfo
	{
		private string _title;

		private string _message;

		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				this._message = value;
			}
		}

		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
			}
		}

		public MessageBoxInfo()
		{
			this._title = "This is my title";
			this._message = "Let go to next step.";
		}
	}
}