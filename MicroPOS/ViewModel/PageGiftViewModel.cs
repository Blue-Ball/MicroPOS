using GalaSoft.MvvmLight;
using MicroPOS.Helper;
using MicroPOS.Model;
using NLog;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace MicroPOS.ViewModel
{
	public class PageGiftViewModel : ViewModelBase
	{
		private ICommand _mouseClickSelectCredit;

		private ICommand _mouseClickSelectDebit;

		private GiftCardInfo _cardInfo;

		private Color _pageBackColor = (Color)ColorConverter.ConvertFromString("White");

		private Color _pageForeColor = Color.FromRgb(0, 0, 0);

		private Color _pageTitleColor = (Color)ColorConverter.ConvertFromString("Blue");

		private Color _txtHintColor = (Color)ColorConverter.ConvertFromString("Silver");

		private Color _txtForeColor = (Color)ColorConverter.ConvertFromString("Black");

		private SolidColorBrush _txtCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		private SolidColorBrush _txtDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		public RelayCommand<string> CmdSetCardType
		{
			get;
			private set;
		}

		public RelayCommand<string> CmdSetPayAmount
		{
			get;
			private set;
		}

		public GiftCardInfo GiftInfo
		{
			get
			{
				return this._cardInfo;
			}
			set
			{
				this._cardInfo = value;
				this.RaisePropertyChanged("CardInfo");
			}
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
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageGiftViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageGiftViewModel).GetMethod("get_mTextCreditColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtCreditColor, value);
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
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageGiftViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageGiftViewModel).GetMethod("get_mTextDebitColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtDebitColor, value);
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

		public PageGiftViewModel()
		{
			this._cardInfo = new GiftCardInfo();
			this.CmdSetCardType = new RelayCommand<string>(new Action<string>(this.ExcuteSetCardType));
			this.CmdSetPayAmount = new RelayCommand<string>(new Action<string>(this.ExecuteSetPayAmount));
		}

		private void ExcuteSetCardType(string p_cardType)
		{
			this._cardInfo.PayMethod = 0;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			PosHelper.Logger.Info(string.Concat("ExcuteSetCardType : ", p_cardType));
			if (string.IsNullOrEmpty(p_cardType))
			{
				return;
			}
			try
			{
				string[] strArrays = p_cardType.Split(new char[] { '&' });
				this.GiftInfo.CardType = strArrays[0];
				double num = 0;
				double.TryParse(strArrays[1], out num);
				this.GiftInfo.FeeFixed = num;
				num = 0;
				double.TryParse(strArrays[2], out num);
				this.GiftInfo.FeePercent = num;
			}
			catch
			{
				Console.WriteLine(string.Concat("******* ExecuteSetNetworkType : ", p_cardType));
			}
		}

		private void ExecuteSelectCredit()
		{
			this.GiftInfo.PayMethod = 1;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
		}

		private void ExecuteSelectDebit()
		{
			this.GiftInfo.PayMethod = 2;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
		}

		private void ExecuteSetPayAmount(string p_amount)
		{
			if (!string.IsNullOrEmpty(p_amount))
			{
				try
				{
					this.GiftInfo.Amount = double.Parse(p_amount);
				}
				catch
				{
				}
			}
		}
	}
}