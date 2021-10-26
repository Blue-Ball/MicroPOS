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
	public class PageShareViewModel : ViewModelBase
	{
		private ICommand _mouseClickSelectCredit;

		private ICommand _mouseClickSelectDebit;

		private ShareMachineInfo _shareInfo;

		private Color _pageBackColor = (Color)ColorConverter.ConvertFromString("White");

		private Color _pageForeColor = Color.FromRgb(0, 0, 0);

		private Color _pageTitleColor = (Color)ColorConverter.ConvertFromString("Blue");

		private Color _txtHintColor = (Color)ColorConverter.ConvertFromString("Silver");

		private Color _txtForeColor = (Color)ColorConverter.ConvertFromString("Black");

		private SolidColorBrush _txtCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		private SolidColorBrush _txtDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		public RelayCommand<string> CmdSetCpfNumber
		{
			get;
			private set;
		}

		public RelayCommand<string> CmdSetName
		{
			get;
			private set;
		}

		public RelayCommand<string> CmdSetPayAmount
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
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageShareViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageShareViewModel).GetMethod("get_mTextCreditColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtCreditColor, value);
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
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageShareViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageShareViewModel).GetMethod("get_mTextDebitColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtDebitColor, value);
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

		public ShareMachineInfo ShareInfo
		{
			get
			{
				return this._shareInfo;
			}
			set
			{
				this._shareInfo = value;
				this.RaisePropertyChanged("ShareInfo");
			}
		}

		public PageShareViewModel()
		{
			this._shareInfo = new ShareMachineInfo();
			this.CmdSetCpfNumber = new RelayCommand<string>(new Action<string>(this.ExcuteSetCpfNumber));
			this.CmdSetName = new RelayCommand<string>(new Action<string>(this.ExecuteSetName));
			this.CmdSetPayAmount = new RelayCommand<string>(new Action<string>(this.ExecuteSetPayAmount));
		}

		private void ExcuteSetCpfNumber(string cpfNumber)
		{
			this._shareInfo.PayMethod = 0;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.ShareInfo.CpfNumber = cpfNumber;
		}

		private void ExecuteSelectCredit()
		{
			this.ShareInfo.PayMethod = 1;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
		}

		private void ExecuteSelectDebit()
		{
			this.ShareInfo.PayMethod = 2;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
		}

		private void ExecuteSetName(string p_name)
		{
			this.ShareInfo.Name = p_name;
		}

		private void ExecuteSetPayAmount(string p_amount)
		{
			double num = 0;
			if (!double.TryParse(p_amount, out num))
			{
				num = 0;
			}
			this.ShareInfo.Amount = num;
		}
	}
}