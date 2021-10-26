using System;

namespace MicroPOS.Model
{
	public class GiftCardInfo
	{
		private string _cardType;

		private double _feeFixed;

		private double _feePercent;

		private double _amount;

		private int _payMethod;

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

		public string CardType
		{
			get
			{
				return this._cardType;
			}
			set
			{
				this._cardType = value;
			}
		}

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

		public double Tax
		{
			get
			{
				return this._feeFixed + this._amount * this._feePercent / 100;
			}
		}

		public double TotalAmount
		{
			get
			{
				return this.Amount + this.Tax;
			}
		}

		public GiftCardInfo()
		{
		}
	}
}