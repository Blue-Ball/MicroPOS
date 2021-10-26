using System;

namespace MicroPOS.Model
{
	public class RechargeInfo
	{
		private string _phoneNumber;

		private string _networkType;

		private double _feeFixed;

		private double _feePercent;

		private int _payMethod;

		private double _rcgAmount;

		public double FeeFixed
		{
			get
			{
				return this._feeFixed;
			}
			set
			{
				this._feeFixed = value;
			}
		}

		public double FeePercent
		{
			get
			{
				return this._feePercent;
			}
			set
			{
				this._feePercent = value;
			}
		}

		public string NetworkType
		{
			get
			{
				return this._networkType;
			}
			set
			{
				this._networkType = value;
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

		public string PhoneNumber
		{
			get
			{
				return this._phoneNumber;
			}
			set
			{
				this._phoneNumber = value;
			}
		}

		public double RcgAmount
		{
			get
			{
				return this._rcgAmount;
			}
			set
			{
				this._rcgAmount = value;
			}
		}

		public double Tax
		{
			get
			{
				return this._feeFixed + this._rcgAmount * this._feePercent / 100;
			}
		}

		public double TotalAmount
		{
			get
			{
				return this.RcgAmount + this.Tax;
			}
		}

		public RechargeInfo()
		{
		}
	}
}