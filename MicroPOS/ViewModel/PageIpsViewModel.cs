using GalaSoft.MvvmLight;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.RestAPI;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace MicroPOS.ViewModel
{
	public class PageIpsViewModel : ViewModelBase
	{
		private ICommand _mouseClickSelectCredit;

		private ICommand _mouseClickSelectDebit;

		private IpsInfo _ipsInfo;

		private Color _pageBackColor = (Color)ColorConverter.ConvertFromString("White");

		private Color _pageForeColor = Color.FromRgb(0, 0, 0);

		private Color _pageTitleColor = (Color)ColorConverter.ConvertFromString("Blue");

		private Color _txtHintColor = (Color)ColorConverter.ConvertFromString("Silver");

		private Color _txtForeColor = (Color)ColorConverter.ConvertFromString("Black");

		private SolidColorBrush _txtCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		private SolidColorBrush _txtDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		public RelayCommand<string> CmdSetCompany
		{
			get;
			private set;
		}

		public RelayCommand<string> CmdSetCpfNumber
		{
			get;
			private set;
		}

		public RelayCommand<RTIpsInvoice> CmdSetIpsInvoice
		{
			get;
			private set;
		}

		public RelayCommand<string> CmdSetPayAmount
		{
			get;
			private set;
		}

		public IpsInfo IpsData
		{
			get
			{
				return this._ipsInfo;
			}
			set
			{
				this._ipsInfo = value;
				this.RaisePropertyChanged("IpsData");
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
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageIpsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageIpsViewModel).GetMethod("get_mTextCreditColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtCreditColor, value);
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
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageIpsViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageIpsViewModel).GetMethod("get_mTextDebitColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtDebitColor, value);
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

		public string TotalAmount
		{
			get
			{
				double valorFatura = 0;
				if (this.IpsData.Invoice != null)
				{
					RTFatura[] invoice = this.IpsData.Invoice.faturas;
					for (int i = 0; i < (int)invoice.Length; i++)
					{
						RTFatura rTFatura = invoice[i];
						if (rTFatura.@select)
						{
							valorFatura += rTFatura.valor_fatura;
						}
					}
				}
				return PosHelper.GetCurrencyString(valorFatura);
			}
		}

		public PageIpsViewModel()
		{
			this.IpsData = new IpsInfo();
			this.CmdSetCompany = new RelayCommand<string>(new Action<string>(this.ExcuteSetCompany));
			this.CmdSetCpfNumber = new RelayCommand<string>(new Action<string>(this.ExcuteSetCpfNumber));
			this.CmdSetPayAmount = new RelayCommand<string>(new Action<string>(this.ExecuteSetPayAmount));
			this.CmdSetIpsInvoice = new RelayCommand<RTIpsInvoice>(new Action<RTIpsInvoice>(this.ExecuteSetIpsInvoice));
		}

		private void ExcuteSetCompany(string p_company)
		{
			this._ipsInfo.PayMethod = 0;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.IpsData.Company = p_company;
		}

		private void ExcuteSetCpfNumber(string p_cpf)
		{
			this.IpsData.CpfNumber = p_cpf;
		}

		private void ExecuteSelectCredit()
		{
			this.IpsData.PayMethod = 1;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
		}

		private void ExecuteSelectDebit()
		{
			this.IpsData.PayMethod = 2;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
		}

		private void ExecuteSetIpsInvoice(RTIpsInvoice p_invoice)
		{
			this.IpsData.Invoice = p_invoice;
		}

		private void ExecuteSetPayAmount(string p_amount)
		{
		}
	}
}