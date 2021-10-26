using GalaSoft.MvvmLight;
using MicroPOS.Model;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace MicroPOS.ViewModel
{
	public class PageRechargeViewModel : ViewModelBase
	{
		private ICommand _mouseClickSelectCredit;

		private ICommand _mouseClickSelectDebit;

		private RechargeInfo _rechargeInfo;

		private string _phoneNumber = "";

		private Color _pageBackColor = (Color)ColorConverter.ConvertFromString("White");

		private Color _pageForeColor = Color.FromRgb(0, 0, 0);

		private Color _pageTitleColor = (Color)ColorConverter.ConvertFromString("Blue");

		private Color _txtHintColor = (Color)ColorConverter.ConvertFromString("Silver");

		private Color _txtForeColor = (Color)ColorConverter.ConvertFromString("Black");

		private SolidColorBrush _txtCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		private SolidColorBrush _txtDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		public RelayCommand<string> CmdSetNetworkType
		{
			get;
			private set;
		}

		public RelayCommand<string> CmdSetPhoneNumber
		{
			get;
			private set;
		}

		public RelayCommand<string> CmdSetRechargeAmount
		{
			get;
			private set;
		}

		public ICommand MouseClickCredit
		{
			get
			{
				ICommand command = this._mouseClickSelectCredit;
				if (command == null)
				{
					RelayCommand<object> relayCommand = new RelayCommand<object>((object x) => this.ExecuteSelectCredit());
					ICommand command1 = relayCommand;
					this._mouseClickSelectCredit = relayCommand;
					command = command1;
				}
				return command;
			}
		}

		public ICommand MouseClickDebit
		{
			get
			{
				ICommand command = this._mouseClickSelectDebit;
				if (command == null)
				{
					RelayCommand<object> relayCommand = new RelayCommand<object>((object x) => this.ExecuteSelectDebit());
					ICommand command1 = relayCommand;
					this._mouseClickSelectDebit = relayCommand;
					command = command1;
				}
				return command;
			}
		}

		public SolidColorBrush mPageBackColor
		{
			get
			{
				return new SolidColorBrush(this._pageBackColor);
			}
		}

		public SolidColorBrush mPageForeColor
		{
			get
			{
				return new SolidColorBrush(this._pageForeColor);
			}
		}

		public SolidColorBrush mPageTitleColor
		{
			get
			{
				return new SolidColorBrush(this._pageTitleColor);
			}
		}

		public SolidColorBrush mTextCreditColor
		{
			get
			{
				return this._txtCreditColor;
			}
			set
			{
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageRechargeViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageRechargeViewModel).GetMethod("get_mTextCreditColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtCreditColor, value);
			}
		}

		public SolidColorBrush mTextDebitColor
		{
			get
			{
				return this._txtDebitColor;
			}
			set
			{
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageRechargeViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageRechargeViewModel).GetMethod("get_mTextDebitColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtDebitColor, value);
			}
		}

		public SolidColorBrush mTextForeColor
		{
			get
			{
				return new SolidColorBrush(this._txtForeColor);
			}
		}

		public SolidColorBrush mTextHintColor
		{
			get
			{
				return new SolidColorBrush(this._txtHintColor);
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
				this.RaisePropertyChanged("PhoneNumber");
			}
		}

		public RechargeInfo RcgInfo
		{
			get
			{
				return this._rechargeInfo;
			}
			set
			{
				this._rechargeInfo = value;
				this.RaisePropertyChanged("RcgInfo");
			}
		}

		public PageRechargeViewModel()
		{
			this._rechargeInfo = new RechargeInfo();
			this.CmdSetPhoneNumber = new RelayCommand<string>(new Action<string>(this.ExcuteSetPhoneNumber));
			this.CmdSetNetworkType = new RelayCommand<string>(new Action<string>(this.ExecuteSetNetworkType));
			this.CmdSetRechargeAmount = new RelayCommand<string>(new Action<string>(this.ExecuteSetRechargeAmount));
		}

		private void ExcuteSetPhoneNumber(string phoneNumber)
		{
			this._rechargeInfo.PayMethod = 0;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.RcgInfo.PhoneNumber = phoneNumber;
		}

		private void ExecuteSelectCredit()
		{
			this.RcgInfo.PayMethod = 1;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
		}

		private void ExecuteSelectDebit()
		{
			this.RcgInfo.PayMethod = 2;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
		}

		private void ExecuteSetNetworkType(string networkType)
		{
			if (string.IsNullOrEmpty(networkType))
			{
				return;
			}
			try
			{
				string[] strArrays = networkType.Split(new char[] { '&' });
				this.RcgInfo.NetworkType = strArrays[0];
				double num = 0;
				double.TryParse(strArrays[1], out num);
				this.RcgInfo.FeeFixed = num;
				num = 0;
				double.TryParse(strArrays[2], out num);
				this.RcgInfo.FeePercent = num;
			}
			catch
			{
				Console.WriteLine(string.Concat("******* ExecuteSetNetworkType : ", networkType));
			}
		}

		private void ExecuteSetRechargeAmount(string amount)
		{
			if (!string.IsNullOrEmpty(amount))
			{
				try
				{
					this.RcgInfo.RcgAmount = double.Parse(amount);
				}
				catch
				{
				}
			}
		}
	}
}