using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MicroPOS.ViewModel
{
	public class viewmodelInputMsgBox : INotifyPropertyChanged
	{
		private string _title = "My Title";

		private string _message = "it is very difficult to create msg box in WPF";

		private string _name;

		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				this.MutateVerbose<string>(ref this._message, value, this.RaisePropertyChanged(), "Message");
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this.MutateVerbose<string>(ref this._name, value, this.RaisePropertyChanged(), "Name");
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
				this.MutateVerbose<string>(ref this._title, value, this.RaisePropertyChanged(), "Title");
			}
		}

		public viewmodelInputMsgBox()
		{
		}

		private Action<PropertyChangedEventArgs> RaisePropertyChanged()
		{
			return (PropertyChangedEventArgs args) => {
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				if (propertyChangedEventHandler == null)
				{
					return;
				}
				propertyChangedEventHandler(this, args);
			};
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}