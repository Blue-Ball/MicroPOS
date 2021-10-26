using System;

namespace MicroPOS.Model
{
	public class ShareMachineInfo
	{
		private string _name;

		private string _cpfNumber;

		private int _payMethod;

		private double _amount;

		public double Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				this._amount = value;
			}
		}

		public string CpfNumber
		{
			get
			{
				return this._cpfNumber;
			}
			set
			{
				this._cpfNumber = value;
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
				this._name = value;
			}
		}

		public int PayMethod
		{
			get
			{
				return this._payMethod;
			}
			set
			{
				this._payMethod = value;
			}
		}

		public ShareMachineInfo()
		{
		}
	}
}