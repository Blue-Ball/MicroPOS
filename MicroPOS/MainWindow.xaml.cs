using BespokeFusion;
using Cappta.Gp.Api.Com.Model;
using MaterialDesignThemes.Wpf;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.PinpadSDK;
using MicroPOS.Popup;
using MicroPOS.RestAPI;
using MicroPOS.View;
using MicroPOS.ViewModel;
using Newtonsoft.Json;
using NLog;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MicroPOS
{
    public partial class MainWindow : Window
    {
        private clsCapptaClient m_pinpadSDK;

        private const int INTERVALO_MILISEGUNDOS = 500;

        public NavigationService m_naviService;

        private readonly static    InterceptKeys.LowLevelKeyboardProc _proc;

        private static IntPtr _hookID;

        private static clsRestAPI m_restAPI;

        private uint IDLE_TIME = 210;

        private DispatcherTimer m_idleTimer;

        private DispatcherTimer m_alertTimer;

        private string m_pendingMessage = "";

        public static clsRestAPI RestProvider
        {
            get
            {
                return MainWindow.m_restAPI;
            }
        }

        static MainWindow()
        {
      
            MainWindow._proc = new InterceptKeys.LowLevelKeyboardProc(MainWindow.CBF_KeyboardHook);
            MainWindow._hookID = IntPtr.Zero;
        }

        public MainWindow()
        {
            this.InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.MainWindow_Loaded);
            base.Closed += new EventHandler(this.MainWindow_Closed);
            this._txtTitle.MouseUp += new MouseButtonEventHandler(this.MainWindow_Home);
            this._btnClose.Click += new RoutedEventHandler(this.MainWindow_Close);
            this._btnMinimize.Click += new RoutedEventHandler(this.MainWindow_Minimize);
            this.m_pinpadSDK = new clsCapptaClient(this);

            string screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth.ToString();
            string screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight.ToString();

            //Caso a resolução seja 1024x768 ajustamos o tamanho dos elementos
            if (screenWidth.Equals("1024") && screenHeight.Equals("768"))
            {
                _naviCanvas.Width = 1024;
                _naviCanvas.Height = 550;

                _naviFrame.Width = 1024;
                _naviFrame.Height = 550;

                _naviCaption.Width = 1024;
                //_naviCaption.Height = 100;
               
            }
        }

        private void _btnAlertClose_Click(object sender, RoutedEventArgs e)
        {
            this.HideAlertMessage();
        }

        private void _btnTest_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Console.WriteLine(string.Format("ret = {0}", this.ShowKMessageBox("dddkdddddd", "title", KMessageBoxButtons.OK))));
        }

        public bool AbortPayment()
        {
            PosHelper.Logger.Info("### AbortPayment");
            if (this.m_pinpadSDK.AbortOperation())
            {
                return true;
            }
            this.showAlertMessage("Rede Totem", this.m_pinpadSDK.Message, AlertType.warning, -1);
            return false;
        }

        public static IntPtr CBF_KeyboardHook(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            if (nCode >= 0 && (wParam - 256 <= 1 || wParam - 260 <= 1) && (lParam.vkCode == 9 && lParam.flags == 32 || lParam.vkCode == 27 && lParam.flags == 32 || lParam.vkCode == 115 && lParam.flags == 32 || lParam.vkCode == 27 && lParam.flags == 0 || lParam.vkCode == 91 && lParam.flags == 1 || lParam.vkCode == 92 && lParam.flags == 1))
            {
                PosHelper.Logger.Info("*** Prevented!!! **** ");
                return (IntPtr)1;
            }
            return InterceptKeys.CallNextHookEx(MainWindow._hookID, nCode, wParam, ref lParam);
        }

        private void closingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void DialogHost_Loaded(object sender, RoutedEventArgs e)
        {
           this.ss_initPinpad();
        }

    

        public bool ExecutePayBilling(Page p_owner, BillingInfo p_billInfo)
        {
            solucaoTemp.billInfo = p_billInfo;
            PosHelper.Logger.Info(string.Format("### ExecutePayBilling {0}", p_billInfo));
            if (p_billInfo == null)
            {
                return false;
            }
            bool flag = false;
            if (p_billInfo.PayMethod != 1)
            {
                if (p_billInfo.PayMethod != 2)
                {
                    return false;
                }
                if(p_billInfo.Amount>= p_billInfo.TotalAmount)
                    flag = this.m_pinpadSDK.PaymentDebit(p_billInfo.Amount);
                else
                    flag = this.m_pinpadSDK.PaymentDebit(p_billInfo.TotalAmount);
            }
            else
            {
                bool flag1 = (p_billInfo.Parcelas == 1 ? false : true);

                if (p_billInfo.Amount >= p_billInfo.TotalAmount)
                    flag = this.m_pinpadSDK.PaymentCredit(p_billInfo.Amount, p_billInfo.Parcelas, MicroPOS.PinpadSDK.InstallmentType.administrative, flag1);
                else
                    flag = this.m_pinpadSDK.PaymentCredit(p_billInfo.TotalAmount, p_billInfo.Parcelas, MicroPOS.PinpadSDK.InstallmentType.administrative, flag1);

            }
            if (!flag)
            {
                this.showAlertMessage("Rede Totem", this.m_pinpadSDK.Message, AlertType.warning, -1);
                return false;
            }

      
        


            pageBilling05 pOwner = (pageBilling05)p_owner;

            pOwner._btnCancel.IsEnabled = true;
            pOwner._btnContinue.IsEnabled = false;
            this.IterarOperacaoTef(-1, null);
            return true;
        }

        public bool ExecutePayInternet(Page p_owner, IpsInfo p_ipsInfo)
        {
            PosHelper.Logger.Info(string.Format("### ExecuteRechargeMobile {0}", p_ipsInfo));
            if (p_ipsInfo == null)
            {
                return false;
            }
            bool flag = false;
            if (p_ipsInfo.PayMethod != 1)
            {
                if (p_ipsInfo.PayMethod != 2)
                {
                    return false;
                }
                flag = this.m_pinpadSDK.PaymentDebit(p_ipsInfo.TotalAmount);
            }
            else
            {
                flag = this.m_pinpadSDK.PaymentCredit(p_ipsInfo.TotalAmount, 1, MicroPOS.PinpadSDK.InstallmentType.administrative, false);
            }
            if (!flag)
            {
                this.showAlertMessage("Rede Totem", this.m_pinpadSDK.Message, AlertType.warning, -1);
                return false;
            }
            pagePayIPS04 pOwner = (pagePayIPS04)p_owner;
            pOwner._btnCancel.IsEnabled = true;
            pOwner._btnContinue.IsEnabled = false;
            this.IterarOperacaoTef(3, p_ipsInfo);
            return true;
        }

        public bool ExecutePrinting(string p_number)
        {
            PosHelper.Logger.Info(string.Concat("### ExecutePrinting : ", p_number));
            if (!this.m_pinpadSDK.ReprintCoupon(ReimpressaoType.cliente, p_number))
            {
                return false;
            }
            this.IterarOperacaoTef(-1, null);
            return true;
        }

        public bool ExecuteRechargeGiftCard(Page p_owner, GiftCardInfo p_giftInfo)
        {
            PosHelper.Logger.Info(string.Format("### ExecuteRechargeGiftCard {0}", p_giftInfo));
            if (p_giftInfo == null)
            {
                return false;
            }
            bool flag = false;
            if (p_giftInfo.PayMethod != 1)
            {
                if (p_giftInfo.PayMethod != 2)
                {
                    return false;
                }
                flag = this.m_pinpadSDK.PaymentDebit(p_giftInfo.TotalAmount);
            }
            else
            {
                flag = this.m_pinpadSDK.PaymentCredit(p_giftInfo.TotalAmount, 1, MicroPOS.PinpadSDK.InstallmentType.administrative, false);
            }
            if (!flag)
            {
                this.showAlertMessage("Rede Totem", this.m_pinpadSDK.Message, AlertType.warning, -1);
                return false;
            }
            pageBuyGiftCard04 pOwner = (pageBuyGiftCard04)p_owner;
            pOwner._btnCancel.IsEnabled = true;
            pOwner._btnContinue.IsEnabled = false;
            this.IterarOperacaoTef(2, p_giftInfo);
            return true;
        }

        public bool ExecuteRechargeMobile(Page p_owner, RechargeInfo p_rchgInfo)
        {
            PosHelper.Logger.Info(string.Format("### ExecuteRechargeMobile {0}", p_rchgInfo));
            if (p_rchgInfo == null)
            {
                return false;
            }
            bool flag = false;
            if (p_rchgInfo.PayMethod != 1)
            {
                if (p_rchgInfo.PayMethod != 2)
                {
                    return false;
                }
                flag = this.m_pinpadSDK.PaymentDebit(p_rchgInfo.TotalAmount);
            }
            else
            {
                flag = this.m_pinpadSDK.PaymentCredit(p_rchgInfo.TotalAmount, 1, MicroPOS.PinpadSDK.InstallmentType.administrative, false);
            }
            if (!flag)
            {
                this.showAlertMessage("Rede Totem", this.m_pinpadSDK.Message, AlertType.warning, -1);
                return false;
            }
            pagePhoneRecharge06 pOwner = (pagePhoneRecharge06)p_owner;
            pOwner._btnCancel.IsEnabled = true;
            pOwner._btnContinue.IsEnabled = false;
            this.IterarOperacaoTef(1, p_rchgInfo);
            return true;
        }

        public void GotoHome(bool hideAlert = true)
        {
            PosHelper.Logger.Info(">>>  GotoHome");
            Application.Current.Dispatcher.Invoke(() => {
                if (this._metroPanel.Visibility == System.Windows.Visibility.Hidden)
                {
                    this._naviCanvas.Visibility = System.Windows.Visibility.Hidden;
                    this._metroPanel.Visibility = System.Windows.Visibility.Visible;
                }
                this.m_naviService = null;
                if (hideAlert)
                {
                    this.SetPendingAsEmpty();
                    this._txtAlertTitle.Text = "";
                    this._txtAlertDesc.Text = "";
                    this._panelAlert.Visibility = System.Windows.Visibility.Hidden;
                }
            });
        }

        public void GotoMainForm(bool hideAlert = true)
        {
            PosHelper.Logger.Info(">>>  GotoHome");
            Application.Current.Dispatcher.Invoke(() => {
                this._naviCanvas.Visibility = System.Windows.Visibility.Hidden;
                this._metroPanel.Visibility = System.Windows.Visibility.Hidden;
                if (hideAlert)
                {
                    this.SetPendingAsEmpty();
                    this._txtAlertTitle.Text = "";
                    this._txtAlertDesc.Text = "";
                    this._panelAlert.Visibility = System.Windows.Visibility.Hidden;
                }
            });
        }

        public void HideAlertMessage()
        {
            Application.Current.Dispatcher.Invoke(() => {
                this._txtAlertTitle.Text = string.Empty;
                this._txtAlertDesc.Text = string.Empty;
                this._txtPending.Text = string.Empty;
                this.SetPendingAsEmpty();
                this._panelAlert.Visibility = System.Windows.Visibility.Hidden;
            });
        }

        private async void InvokeReceiptPrint(object sender, DialogOpenedEventArgs eventArgs)
        {
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(50);
            }
            Console.WriteLine("wait finished");
            eventArgs.Session.Close(false);
        }

        public bool IsPendingState()
        {
            return this.m_pinpadSDK.PendingState;
        }

        private void IterarOperacaoTef(int p_reqType = -1, object p_reqObject = null)
        {
            PosHelper.Logger.Info(">>>  IterarOperacaoTef");
            TaskManagerForm.Start(async () => {
                PosHelper.Logger.Info(">>>>>  Start IterarOperacaoTef ");
                IIteracaoTef iterationTEF = null;
                do
                {
                    iterationTEF = this.m_pinpadSDK.GetIterationTEF();
                    if (iterationTEF == null)
                    {
                        break;
                    }
                    if (iterationTEF is IMensagem)
                    {
                        this.showAlertMessage("Rede Totem", ((IMensagem)iterationTEF).Descricao, AlertType.info, -1);
                        PosHelper.Logger.Info(string.Concat("#### IOT 001 [IMensagem] : ", ((IMensagem)iterationTEF).Descricao));
                        Thread.Sleep(500);
                    }
                    if (iterationTEF is IRequisicaoParametro)
                    {
                        PosHelper.Logger.Info("#### IOT 002 [IRequisicaoParametro]");
                        this.ss_requireParameter((IRequisicaoParametro)iterationTEF);
                    }
                    if (iterationTEF is IRespostaTransacaoPendente)
                    {
                        PosHelper.Logger.Info("#### IOT 003 [IRespostaTransacaoPendente]");
                        this.ss_resolvePendingTransaction((IRespostaTransacaoPendente)iterationTEF);
                    }
                    if (iterationTEF is IRespostaOperacaoRecusada)
                    {
                        PosHelper.Logger.Info("#### IOT 004 [IRespostaOperacaoRecusada]");
                        this.ss_showDeclinedInfo((IRespostaOperacaoRecusada)iterationTEF);
                    }
                    if (iterationTEF is IRespostaOperacaoAprovada)
                    {
                        PosHelper.Logger.Info("#### IOT 005 [IRespostaOperacaoAprovada]");
                        IRespostaOperacaoAprovada respostaOperacaoAprovada = (IRespostaOperacaoAprovada)iterationTEF;
                        this.ss_showApprovedInfo(respostaOperacaoAprovada);
                        if (this.m_pinpadSDK.FinishPayment())
                        {
         

                            this.PrintPaymentCoupon(respostaOperacaoAprovada.NumeroControle, respostaOperacaoAprovada.CupomCliente);

                            /*teste print boleto*/
                            if (solucaoTemp.billInfo != null)
                            {
                                if (solucaoTemp.billInfo.isBoleto)
                                {

                                    m_restAPI.PostBoletoPaymentConfirm(solucaoTemp.billInfo.Barcode, solucaoTemp.billInfo.transactionID.ToString(), respostaOperacaoAprovada.NumeroControle, solucaoTemp.billInfo.TotalAmount.ToString(), solucaoTemp.billInfo.Amount.ToString());

                                    solucaoTemp.billInfo.printoData = solucaoTemp.billInfo.printoData.Replace("\\r\\n", "\r\n");
                                    solucaoTemp.billInfo.printoData = solucaoTemp.billInfo.printoData.Replace("\\/", "/");
                                    solucaoTemp.billInfo.printoData = solucaoTemp.billInfo.printoData.Replace("??", "ÇÃ");
                                    solucaoTemp.billInfo.printoData = solucaoTemp.billInfo.printoData.Replace("<VIA1> ", "");
                                    solucaoTemp.billInfo.printoData = solucaoTemp.billInfo.printoData.Replace("</VIA1> ", "");
                                    solucaoTemp.billInfo.printoData = solucaoTemp.billInfo.printoData.Replace("VALOR TITULO", "\r\nVALOR TITULO     ");
                                    this.PrintPaymentCoupon(respostaOperacaoAprovada.NumeroControle, solucaoTemp.billInfo.printoData, 8f);

                                }
                            }


                            if (p_reqType != -1 && p_reqObject != null)
                            {
                                bool flag = (bool)await this.showDProgressBoxAsync("Request API..", new MainWindow.ProcessWithWaitDlg(this.PostAndPrintReceiptAsync), new object[] { p_reqType, p_reqObject, respostaOperacaoAprovada.NumeroControle });
                            }



                        }
                    }
                    if (!(iterationTEF is IRespostaRecarga))
                    {
                        continue;
                    }
                    PosHelper.Logger.Info("#### IOT 006 [IRespostaOperacaoAprovada]");
                    this.ss_showRechargeInfo((IRespostaRecarga)iterationTEF);
                }
                while (this.OperationNotCompleted(iterationTEF));
                PosHelper.Logger.Info("<<<<<  End of IteratorOfTEF!   ");
                this.GotoHome(true);
            });
            PosHelper.Logger.Info("<<<  IterarOperacaoTef");
        }

        private void MainWindow_Close(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.m_idleTimer.Stop();
            InterceptKeys.UnhookWindowsHookEx(MainWindow._hookID);
        }

        private void MainWindow_Home(object sender, RoutedEventArgs e)
        {
            this.GotoHome(true);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PosHelper.Logger.Info("*************************************************************");
            PosHelper.Logger.Info("start program");
            PosHelper.Logger.Info("*************************************************************");
            this._metroPanel.Visibility = System.Windows.Visibility.Visible;
            this._naviCanvas.Visibility = System.Windows.Visibility.Hidden;
            this._panelAlert.Visibility = System.Windows.Visibility.Hidden;
            this._panelWindowBar.Visibility = System.Windows.Visibility.Hidden;
            this.ss_initMetroPanel();
            MainWindow.m_restAPI = new clsRestAPI();
            if (MainWindow.m_restAPI != null && MainWindow.m_restAPI.Login() == null)
            {
                this.showAlertMessage("Rede Totem", "Failed to connecting server", AlertType.info, -1);
            }
            this.m_idleTimer = new DispatcherTimer();
            this.m_idleTimer.Tick += new EventHandler(this.OnIdleEvent);
            this.m_idleTimer.Interval = TimeSpan.FromSeconds(5);
            this.m_idleTimer.Start();
            this.m_alertTimer = new DispatcherTimer();
            this.m_alertTimer.Tick += new EventHandler(this.OnHideFooterMsg);
            this.m_idleTimer.Interval = TimeSpan.FromSeconds(10);
        }

        private void MainWindow_Minimize(object sender, RoutedEventArgs e)
        {
            base.WindowState = System.Windows.WindowState.Minimized;
        }

        private void MainWindow_OnBilling(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.m_restAPI.isLogined())
            {
                this.showAlertMessage("Rede Totem", "It is not connected to Server.", AlertType.info, -1);
                return;
            }
            this._metroPanel.Visibility = System.Windows.Visibility.Hidden;
            this._naviCanvas.Visibility = System.Windows.Visibility.Visible;
            this._naviTitle.Text = "Pagar Boleto";
            this._naviTitle.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            this.m_naviService = this._naviFrame.NavigationService;
            this.m_naviService.Navigate(new pageBoleto01());
        }

        private void MainWindow_OnGiftCard(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.m_restAPI.isLogined())
            {
                this.showAlertMessage("Rede Totem", "It is not connected to Server.", AlertType.info, -1);
                return;
            }
            this._metroPanel.Visibility = System.Windows.Visibility.Hidden;
            this._naviCanvas.Visibility = System.Windows.Visibility.Visible;
            this._naviTitle.Text = "COMPRAR GIFT CARD";
            this._naviTitle.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            this.m_naviService = this._naviFrame.NavigationService;
            this.m_naviService.Navigate(new pageBuyGiftCard01());
        }

        private void MainWindow_OnProvide(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.m_restAPI.isLogined())
            {
                this.showAlertMessage("Rede Totem", "It is not connected to Server.", AlertType.info, -1);
                return;
            }
            this._metroPanel.Visibility = System.Windows.Visibility.Hidden;
            this._naviCanvas.Visibility = System.Windows.Visibility.Visible;
            this._naviTitle.Text = "PAGAR INTERNET";
            this._naviTitle.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            this.m_naviService = this._naviFrame.NavigationService;
            this.m_naviService.Navigate(new pagePayIPS00());
        }

        private void MainWindow_OnRecharge(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.m_restAPI.isLogined())
            {
                this.showAlertMessage("Rede Totem", "It is not connected to Server.", AlertType.info, -1);
                return;
            }
            this._metroPanel.Visibility = System.Windows.Visibility.Hidden;
            this._naviCanvas.Visibility = System.Windows.Visibility.Visible;
            this._naviTitle.Text = "RECARGA DE CELULAR";
            this._naviTitle.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            this.m_naviService = this._naviFrame.NavigationService;
            this.m_naviService.Navigate(new pagePhoneRecharge01());
        }

        private void MainWindow_OnShare(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.m_restAPI.isLogined())
            {
                this.showAlertMessage("Rede Totem", "It is not connected to Server.", AlertType.info, -1);
                return;
            }
            this._metroPanel.Visibility = System.Windows.Visibility.Hidden;
            this._naviCanvas.Visibility = System.Windows.Visibility.Visible;
            this._naviTitle.Text = "MAQUINA COMPARTILHADA";
            this._naviTitle.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            this.m_naviService = this._naviFrame.NavigationService;
            this.m_naviService.Navigate(new pageShareMachine01());
        }

        public MicroPOS.Helper.Receipt MakeReceiptForGift(string p_tefID, GiftCardInfo p_giftInfo, string p_pinCode)
        {
            PosHelper.Logger.Info(string.Concat("*** Receipt_For_Printing : ", p_tefID));
            return new MicroPOS.Helper.Receipt("Rede Totem", "COMPRAR GIFT CARD", DateTime.Now, "DATA", new List<ReceiptItem>()
            {
                new ReceiptItem("TEF ID", p_tefID, "", false),
                new ReceiptItem("Nome do Serviço", p_giftInfo.CardType, "", false),
                new ReceiptItem("Valor", PosHelper.GetCurrencyString(p_giftInfo.Amount), "", false),
                new ReceiptItem("Nossa Tarifa", PosHelper.GetCurrencyString(p_giftInfo.Tax), "", false),
                new ReceiptItem("Total a pagar", PosHelper.GetCurrencyString(p_giftInfo.TotalAmount), "", false),
                new ReceiptItem("Método de Pagamento", PosHelper.GetPayMethodName(p_giftInfo.PayMethod), "", false),
                new ReceiptItem("Pin Code", p_pinCode, "Instruções:\r\nUtilize esse PIN para recarregar o serviço desejado", true)
            }, null, null);
        }        

        private MicroPOS.Helper.Receipt MakeReceiptForInternet(string p_tefID, IpsInfo p_ipsInfo)
        {
            PosHelper.Logger.Info(string.Concat("*** Receipt_For_Printing : ", p_tefID));
            //RV - Adding new fields(Invoice Id and Due Date)
            string inv = "";
            string dueDate = "";
            try
            {
                var invoiceObj = p_ipsInfo.Invoice.faturas.Where(item => item.select == true).FirstOrDefault();
                if (invoiceObj != null)
                {
                    inv = invoiceObj.id_fatura;
                    dueDate = invoiceObj.DecoDate;
                }
            }   
            catch (Exception)
            { }
            return new MicroPOS.Helper.Receipt("Rede Totem", "PAGAR INTERNET", DateTime.Now, "DATA", new List<ReceiptItem>()
            {
                new ReceiptItem("TEF ID", p_tefID, "", false),
                new ReceiptItem("Nome", p_ipsInfo.Invoice.nome_razao, "", false),
                new ReceiptItem("CPF", p_ipsInfo.CpfNumber.Replace(" ", string.Empty), "", false),
                new ReceiptItem("Valor", PosHelper.GetCurrencyString(p_ipsInfo.Amount), "", false),
                new ReceiptItem("Nossa Tarifa", PosHelper.GetCurrencyString(p_ipsInfo.Tax), "", false),
                new ReceiptItem("Total a pagar", PosHelper.GetCurrencyString(p_ipsInfo.TotalAmount), "", false),
                new ReceiptItem("Método de Pagamento", PosHelper.GetPayMethodName(p_ipsInfo.PayMethod), "", false),
                //RV - Adding new fields(Invoice Id and Due Date)
                new ReceiptItem("Fatura", inv, "", false),
                new ReceiptItem("Vencimento", dueDate, "", false)

            }, null, null); 
        }

        private MicroPOS.Helper.Receipt MakeReceiptForMobile(string p_tefID, RechargeInfo p_mobileInfo)
        {
            PosHelper.Logger.Info(string.Concat("*** Receipt_For_Printing : ", p_tefID));
            return new MicroPOS.Helper.Receipt("Rede Totem", "RECARGA DE CELULAR", DateTime.Now, "DATA", new List<ReceiptItem>()
            {
                new ReceiptItem("TEF ID", p_tefID, "", false),
                new ReceiptItem("Nome do telefone", p_mobileInfo.PhoneNumber.Replace(" ", string.Empty), "", false),
                new ReceiptItem("Operadora", p_mobileInfo.NetworkType, "", false),
                new ReceiptItem("Valor", PosHelper.GetCurrencyString(p_mobileInfo.RcgAmount), "", false),
                new ReceiptItem("Nossa Tarifa", PosHelper.GetCurrencyString(p_mobileInfo.Tax), "", false),
                new ReceiptItem("Total a pagar", PosHelper.GetCurrencyString(p_mobileInfo.TotalAmount), "", false),
                new ReceiptItem("Método de Pagamento", PosHelper.GetPayMethodName(p_mobileInfo.PayMethod), "", false)
            }, null, null);
        }

        private async void my_openingEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            List<double> giftCardAmount = await MainWindow.RestProvider.GetGiftCardAmount("XBOX");
            Console.WriteLine(string.Format("GetGiftCardAmount Count {0}", giftCardAmount.Count));
            eventArgs.Session.Close(false);
        }

        private void OnHideFooterMsg(object sender, EventArgs e)
        {
            if (this._panelAlert.Visibility == System.Windows.Visibility.Visible)
            {
                this._panelAlert.Visibility = System.Windows.Visibility.Hidden;
                this.m_alertTimer.Stop();
            }
        }

        private void OnIdleEvent(object sender, EventArgs e)
        {
            if (IdleTimeFinder.GetIdleTime() / 1000 < this.IDLE_TIME)
            {
                return;
            }
            if (this.m_naviService != null || this._naviCanvas.Visibility == System.Windows.Visibility.Visible || this._panelAlert.Visibility == System.Windows.Visibility.Visible || this._metroPanel.Visibility == System.Windows.Visibility.Hidden)
            {
                PosHelper.Logger.Info("*****  OnIdleEvent - GotoHome! ");
                this.m_pinpadSDK.AbortOperation();
                this.GotoHome(true);
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            MainWindow._hookID = InterceptKeys.SetHook(MainWindow._proc);
        }

        private bool OperationNotCompleted(IIteracaoTef iteracaoTef)
        {
            if (iteracaoTef.TipoIteracao == 1 || iteracaoTef.TipoIteracao == 2)
            {
                return false;
            }
            return iteracaoTef.TipoIteracao != 12;
        }

        private async Task<object> PostAndPrintReceiptAsync(params object[] p_params)
        {
            object obj;
            if (p_params == null || (int)p_params.Length != 3)
            {
                obj = false;
            }
            else
            {
                MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
                int pParams = (int)p_params[0];
                object pParams1 = p_params[1];
                string str = (string)p_params[2];
                obj = await mainWindow.PostRechargeRequest(pParams, pParams1, str);
            }
            return obj;
        }

        public async Task<bool> PostRechargeRequest(int p_rechargeType, object p_rechargeInfo, string p_tefID)
        {
            RechargeInfo pRechargeInfo;
            GiftCardInfo giftCardInfo;
            IpsInfo ipsInfo;
            bool flag = false;
            int pRechargeType = p_rechargeType;
            switch (pRechargeType)
            {
                case 1:
                    {
                        pRechargeInfo = (RechargeInfo)p_rechargeInfo;
                        RTMobileRecharge rTMobileRecharge = new RTMobileRecharge()
                        {
                            type = 1,
                            recharge_network = pRechargeInfo.NetworkType,
                            recharge_amount = pRechargeInfo.RcgAmount,
                            tef_id = p_tefID,
                            total_paid = pRechargeInfo.TotalAmount,
                            ddd = PosHelper.GetAreaCodeFromPhoneNumber(pRechargeInfo.PhoneNumber),
                            number = PosHelper.GetNumberFromPhoneNumber(pRechargeInfo.PhoneNumber)
                        };
                        if (!await MainWindow.RestProvider.PostRechargeOnMobile(rTMobileRecharge))
                        {
                            PosHelper.Logger.Info("PostRechargeOnMobile : failed!");
                            flag = false;
                            break;
                        }
                        else
                        {
                            PosHelper.Logger.Info("PostRechargeOnMobile : successes!");
                            this.PrintReceipt(p_tefID, this.MakeReceiptForMobile(p_tefID, pRechargeInfo));
                            flag = true;
                            break;
                        }
                    }
                case 2:
                    {
                        giftCardInfo = (GiftCardInfo)p_rechargeInfo;
                        RTGiftRecharge rTGiftRecharge = new RTGiftRecharge()
                        {
                            type = 2,
                            recharge_network = giftCardInfo.CardType,
                            recharge_amount = giftCardInfo.Amount,
                            tef_id = p_tefID,
                            total_paid = giftCardInfo.TotalAmount
                        };
                        string str = await MainWindow.RestProvider.PostRechargeOnGift(rTGiftRecharge);
                        if (string.IsNullOrEmpty(str))
                        {
                            PosHelper.Logger.Info("PostRechargeOnGift : failed!");
                            flag = false;
                            break;
                        }
                        else
                        {
                            PosHelper.Logger.Info("PostRechargeOnGift : successes!");
                            MicroPOS.Helper.Receipt receipt = this.MakeReceiptForGift(p_tefID, giftCardInfo, str);
                            this.PrintReceipt(p_tefID, receipt);
                            flag = true;
                            break;
                        }
                    }
                case 3:
                    {
                        ipsInfo = (IpsInfo)p_rechargeInfo;
                        List<RTPaidFatura> rTPaidFaturas = new List<RTPaidFatura>();
                        RTFatura[] invoice = ipsInfo.Invoice.faturas;
                        for (pRechargeType = 0; pRechargeType < (int)invoice.Length; pRechargeType++)
                        {
                            RTFatura rTFatura = invoice[pRechargeType];
                            if (rTFatura.@select)
                            {
                                RTPaidFatura rTPaidFatura = new RTPaidFatura()
                                {
                                    id_fatura = rTFatura.id_fatura,
                                    tef_id = p_tefID,
                                    valor_fatura = rTFatura.valor_fatura,
                                    total_paid = (double)ipsInfo.Invoice.fee[0].fixed_amount + (rTFatura.valor_fatura / 100 + 1) * (double)ipsInfo.Invoice.fee[0].percentual_amount
                                };
                                rTPaidFaturas.Add(rTPaidFatura);
                            }
                        }
                        if (!await MainWindow.RestProvider.PostPaidInvoice(rTPaidFaturas))
                        {
                            PosHelper.Logger.Info("PostPaidInvoice : failed!");
                            flag = false;
                            break;
                        }
                        else
                        {
                            PosHelper.Logger.Info("PostPaidInvoice : successes!");
                            this.PrintReceipt(p_tefID, this.MakeReceiptForInternet(p_tefID, ipsInfo));
                            flag = true;
                            break;
                        }
                    }
            }
            pRechargeInfo = null;
            giftCardInfo = null;
            ipsInfo = null;
            return flag;
        }

        private void PrintPaymentCoupon(string p_coupon, string p_text, float FontSize=12f)
        {
            PosHelper.Logger.Info(string.Concat("### PrintPaymentCoupon : ", p_coupon));
            this.showAlertMessage("Rede Totem", "o cupom está sendo impresso ...", AlertType.info, -1);
            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Cupom Cliente", 300, 400);
            printDocument.DocumentName = string.Concat("Coupon_", p_coupon);
            printDocument.PrintController = new StandardPrintController();
            printDocument.PrintPage += new PrintPageEventHandler((object sender, PrintPageEventArgs e) => {
                e.Graphics.MeasureString(p_text, new Font("Times New Roman", FontSize), 300);
                e.Graphics.DrawString(p_text, new Font("Times New Roman", FontSize), new SolidBrush(System.Drawing.Color.Black), new PointF(10f, 20f));
            });
            try
            {
                printDocument.Print();
            }
            catch (Exception exception)
            {
                PosHelper.Logger.Error(exception, "printing");
            }
            this.HideAlertMessage();
        }

        private void PrintReceipt(string p_coupon, MicroPOS.Helper.Receipt p_printReceipt)
        {
            PosHelper.Logger.Info(string.Format("### PrintReciept : {0}", p_printReceipt));
            this.showAlertMessage("Rede Totem", "o recibo está sendo impresso ...", AlertType.info, -1);
            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Recibo", 300, 400);
            printDocument.DocumentName = string.Concat("Recibo_", p_coupon);
            printDocument.PrintController = new StandardPrintController();
            printDocument.PrintPage += new PrintPageEventHandler((object sender, PrintPageEventArgs e) => p_printReceipt.print(300, e.Graphics));
            try
            {
                printDocument.Print();
            }
            catch (Exception exception)
            {
                PosHelper.Logger.Error(exception, "printing");
            }
            this.HideAlertMessage();
        }

        public void PrintReceiptWithProgressDlg(string p_tefID, MicroPOS.Helper.Receipt receipt)
        {
            DialogOpenedEventHandler dialogOpenedEventHandler2 = null;
            TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
                ucProcessBox _ucProcessBox = new ucProcessBox();
                _ucProcessBox._progressMsg.Text = "Printing";
                DialogOpenedEventHandler u003cu003e9_1 = dialogOpenedEventHandler2;
                if (u003cu003e9_1 == null)
                {
                    DialogOpenedEventHandler dialogOpenedEventHandler = (object sender, DialogOpenedEventArgs args) => {
                        PosHelper.Logger.Info(string.Concat("PrintReceiptWithProgressDlg : ", p_tefID));
                        this.PrintReceipt(p_tefID, receipt);
                        args.Session.Close(false);
                    };
                    DialogOpenedEventHandler dialogOpenedEventHandler1 = dialogOpenedEventHandler;
                    dialogOpenedEventHandler2 = dialogOpenedEventHandler;
                    u003cu003e9_1 = dialogOpenedEventHandler1;
                }
                await DialogHost.Show(_ucProcessBox, "RootDialog", u003cu003e9_1);
            });
        }

        public bool SendPayRequest()
        {
            return true;
        }

        public void SetPaymentPendingText(double p_totalPayment, int p_payMethod, int p_installment = 1, double parcelas=0)
        {
         
            string empty = string.Empty;
            if (p_payMethod ==2)
                empty = string.Format("{0} - {1}", PosHelper.GetCurrencyString(p_totalPayment).Replace(" ", string.Empty), PosHelper.GetPayMethodName(p_payMethod));
            else
            {
                empty = string.Format("Crédito - {1}x parcela(s) de {2} ",  PosHelper.GetCurrencyString(p_totalPayment).Replace(" ", string.Empty), parcelas,  PosHelper.GetCurrencyString( p_totalPayment / parcelas).Replace(" ", string.Empty));
            
            }
  
            this.m_pendingMessage = empty;
        }

        public void SetPendingAsEmpty()
        {
            this.m_pendingMessage = "";
        }

        public void showAlertMessage(string subject, string desc, AlertType msgType = AlertType.info, int hideInterval = -1)
        {
            TaskManagerForm.InvokeControlAction<UIElement>(this, (UIElement arg) => {
                switch (msgType)
                {
                    case AlertType.debug:
                        {
                            this._alertMessage.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("LightGray"));
                            this._alertMessage.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("DarkGray"));
                            break;
                        }
                    case AlertType.info:
                        {
                            this._alertMessage.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("LightGreen"));
                            this._alertMessage.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("DarkGreen"));
                            break;
                        }
                    case AlertType.warning:
                        {
                            this._alertMessage.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("DarkOrange"));
                            this._alertMessage.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Peru"));
                            break;
                        }
                    case AlertType.error:
                        {
                            this._alertMessage.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Tomato"));
                            this._alertMessage.BorderBrush = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Red"));
                            break;
                        }
                }
                this._txtAlertTitle.Text = subject;
                if (desc != "Processando, por favor, aguarde...")
                {
                    this._txtAlertDesc.Text = desc;
                }
                else
                {
                    this._txtAlertDesc.Text = "Insira seu cartão";
                }
                if (string.IsNullOrEmpty(this.m_pendingMessage))
                {
                    this._pendingStatus.Visibility = System.Windows.Visibility.Hidden;
                    this._btnAlertClose.Visibility = System.Windows.Visibility.Visible;
                    this._txtPending.Text = string.Empty;
                }
                else
                {
                    this._pendingStatus.Visibility = System.Windows.Visibility.Visible;
                    this._btnAlertClose.Visibility = System.Windows.Visibility.Hidden;
                    this._txtPending.Text = this.m_pendingMessage;
                }
                this._panelAlert.Visibility = System.Windows.Visibility.Visible;
                if (hideInterval == -1)
                {
                    this.m_alertTimer.Stop();
                    return;
                }
                this.m_alertTimer.Interval = TimeSpan.FromSeconds(10);
                this.m_alertTimer.Start();
            });
        }

        public void ShowBMessageBox(string message, string title)
        {
            MaterialMessageBox.Show(message, title);
        }

        public string showDMessageBox(string p_message, string p_title, DMessageBoxButtons buttons = 0)
        {
            TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
                object obj = await DialogHost.Show(new ucDMessageBox()
                {
                    Title = "Rede Totem",
                    Message = p_message,
                    Buttons = buttons
                }, "RootDialog", new DialogClosingEventHandler(this.closingEventHandler));
                bool flag = (bool)obj;
                Console.WriteLine(string.Concat("Dialog was closed, the CommandParameter used to close it was: ", obj.ToString()));
            });
            return "";
        }

        public async Task<object> showDProgressBoxAsync(string p_message, MainWindow.ProcessWithWaitDlg p_call, params object[] p_params)
        {
            DialogOpenedEventHandler dialogOpenedEventHandler1 = null;
            object obj = null;
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
                ucProcessBox _ucProcessBox = new ucProcessBox();
                _ucProcessBox._progressMsg.Text = p_message;
                DialogOpenedEventHandler u003cu003e9_1 = dialogOpenedEventHandler1;
                if (u003cu003e9_1 == null)
                {
                    DialogOpenedEventHandler retObj = async (object s, DialogOpenedEventArgs args) => {
                        obj = await p_call(p_params);
                        args.Session.Close(false);
                        taskCompletionSource.SetResult(true);
                    };
                    DialogOpenedEventHandler dialogOpenedEventHandler = retObj;
                    dialogOpenedEventHandler1 = retObj;
                    u003cu003e9_1 = dialogOpenedEventHandler;
                }
                DialogHost.Show(_ucProcessBox, "RootDialog", u003cu003e9_1);
            });
            await taskCompletionSource.Task;
            return obj;
        }

        private string showInputMsgBox(string p_title, string p_message)
        {
            string text = "";
            TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
                ucInputMsgBox _ucInputMsgBox = new ucInputMsgBox()
                {
                    DataContext = new viewmodelInputMsgBox()
                };
                _ucInputMsgBox._msgTitle.Text = p_title;
                _ucInputMsgBox._msgMessage.Text = p_message;
                object obj = await DialogHost.Show(_ucInputMsgBox, "RootDialog", new DialogClosingEventHandler(this.closingEventHandler));
                if ((bool)obj)
                {
                    text = _ucInputMsgBox._inputText.Text;
                }
                Console.WriteLine(string.Concat("Dialog was closed, the CommandParameter used to close it was: ", obj.ToString()));
            });
            return text;
        }

        private async void ShowInputMsgBox(MessageBoxInfo p_msgInfo)
        {
            ucInputMsgBox _ucInputMsgBox = new ucInputMsgBox()
            {
                DataContext = new viewmodelInputMsgBox()
            };
            _ucInputMsgBox._msgTitle.Text = p_msgInfo.Title;
            _ucInputMsgBox._msgMessage.Text = p_msgInfo.Message;
            object obj = await DialogHost.Show(_ucInputMsgBox, "RootDialog");
            Console.WriteLine(string.Concat("Dialog was closed, the CommandParameter used to close it was: ", obj.ToString()));
        }

        private async Task<string> showInputMsgBoxAsync(string p_title, string p_message)
        {
            string text = "";
            ucInputMsgBox _ucInputMsgBox = new ucInputMsgBox()
            {
                DataContext = new viewmodelInputMsgBox()
            };
            _ucInputMsgBox._msgTitle.Text = p_title;
            _ucInputMsgBox._msgMessage.Text = p_message;
            object obj = await DialogHost.Show(_ucInputMsgBox, "RootDialog", new DialogClosingEventHandler(this.closingEventHandler));
            if ((bool)obj)
            {
                text = _ucInputMsgBox._inputText.Text;
            }
            Console.WriteLine(string.Concat("Dialog was closed, the CommandParameter used to close it was: ", obj.ToString()));
            return text;
        }

        public bool ShowKMessageBox(string message, string title, KMessageBoxButtons buttons = 0)
        {
            bool valueOrDefault = false;
            Application.Current.Dispatcher.Invoke(() => {
                KMessageBox kMessageBox = new KMessageBox()
                {
                    Owner = this,
                    Subject = title,
                    Message = message,
                    Buttons = buttons
                };
                kMessageBox.ShowDialog();
                bool? dialogResult = kMessageBox.DialogResult;
                valueOrDefault = dialogResult.GetValueOrDefault() & dialogResult.HasValue;
            });
            return valueOrDefault;
        }

        protected void ss_initMetroPanel()
        {
            WrapPanel wrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = 550
            };
            int num = 5;
            int num1 = 20;
            ucTileButton _ucTileButton = new ucTileButton();
            _ucTileButton._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
            _ucTileButton._tileIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MicroPOS;component/Images/tile_pagar_contas.png"));
            _ucTileButton._tileText.Text = "PAGAR BOLETO";
            _ucTileButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            _ucTileButton.Margin = new Thickness((double)num1, (double)num, (double)num1, (double)num);
            _ucTileButton.MouseUp += new MouseButtonEventHandler(this.MainWindow_OnBilling);
            wrapPanel.Children.Add(_ucTileButton);
            ucTileButton solidColorBrush = new ucTileButton();
            solidColorBrush._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
            solidColorBrush._tileIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MicroPOS;component/Images/tile_recarga_celular.png"));
            solidColorBrush._tileText.Text = "RECARGA DE CELULAR";
            solidColorBrush._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            solidColorBrush.Margin = new Thickness((double)num1, (double)num, (double)num1, (double)num);
            solidColorBrush.MouseUp += new MouseButtonEventHandler(this.MainWindow_OnRecharge);
            wrapPanel.Children.Add(solidColorBrush);
            ucTileButton bitmapImage = new ucTileButton();
            bitmapImage._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
            bitmapImage._tileIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MicroPOS;component/Images/ico_purchase.png"));
            bitmapImage._tileText.Text = "MAQUINA COMPARTILHADA";
            bitmapImage._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            bitmapImage.Margin = new Thickness((double)num1, (double)num, (double)num1, (double)num);
            bitmapImage.MouseUp += new MouseButtonEventHandler(this.MainWindow_OnShare);
            bitmapImage.Visibility = System.Windows.Visibility.Collapsed;
            wrapPanel.Children.Add(bitmapImage);
            ucTileButton thickness = new ucTileButton();
            thickness._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
            thickness._tileIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MicroPOS;component/Images/tile_pagar_provedor.png"));
            thickness._tileText.Text = "PAGAR INTERNET";
            thickness._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            thickness.Margin = new Thickness((double)num1, (double)num, (double)num1, (double)num);
            thickness.MouseUp += new MouseButtonEventHandler(this.MainWindow_OnProvide);
            wrapPanel.Children.Add(thickness);
            ucTileButton _ucTileButton1 = new ucTileButton();
            _ucTileButton1._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
            _ucTileButton1._tileIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MicroPOS;component/Images/tile_gift_card.png"));
            _ucTileButton1._tileText.Text = "SERVIÇOS DIGITAIS";
            _ucTileButton1._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            _ucTileButton1.Margin = new Thickness((double)num1, (double)num, (double)num1, (double)num);
            _ucTileButton1.MouseUp += new MouseButtonEventHandler(this.MainWindow_OnGiftCard);
            wrapPanel.Children.Add(_ucTileButton1);
            this._metroPanel.Children.Add(wrapPanel);
        }

        protected void ss_initPinpad()
        {
            this.m_pinpadSDK = new clsCapptaClient(this);
            if (!this.m_pinpadSDK.AuthenticatePdv())
            {
                this.showDMessageBox(this.m_pinpadSDK.Message, "Rede Totem", DMessageBoxButtons.OK);
                return;
            }
            this.m_pinpadSDK.GetRechargeOperators();
            if (!this.m_pinpadSDK.ConfigrateIntegrationMode(true))
            {
                this.showDMessageBox(this.m_pinpadSDK.Message, "Rede Totem", DMessageBoxButtons.OK);
                return;
            }
            if (this.m_pinpadSDK.GetShopList() != null)
            {
                return;
            }
            this.showDMessageBox(this.m_pinpadSDK.Message, "Rede Totem", DMessageBoxButtons.OK);
        }

        private async void ss_requireParameter(IRequisicaoParametro requisicaoParametros)
        {
            int num;
            string result = "";
            string str = "Rede Totem";
            string str1 = string.Concat(requisicaoParametros.Mensagem, Environment.NewLine, Environment.NewLine);
            PosHelper.Logger.Info(str1);
            Task<string> task = this.showInputMsgBoxAsync(str, str1);
            await task;
            result = task.Result;
            clsCapptaClient mPinpadSDK = this.m_pinpadSDK;
            string str2 = result;
            num = (string.IsNullOrWhiteSpace(result) ? 2 : 1);
            mPinpadSDK.SendParameter(str2, num);
        }

        private async void ss_resolvePendingTransaction(IRespostaTransacaoPendente transacaoPendente)
        {
            int num;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(transacaoPendente.Mensagem);
            ITransacaoPendente[] listaTransacoesPendentes = transacaoPendente.ListaTransacoesPendentes;
            for (int i = 0; i < (int)listaTransacoesPendentes.Length; i++)
            {
                ITransacaoPendente transacaoPendente1 = listaTransacoesPendentes[i];
                stringBuilder.AppendLine(string.Concat("Número de Controle: ", transacaoPendente1.NumeroControle));
                stringBuilder.AppendLine(string.Concat("Bandeira: ", transacaoPendente1.NomeBandeiraCartao));
                stringBuilder.AppendLine(string.Concat("Adquirente: ", transacaoPendente1.NomeAdquirente));
                stringBuilder.AppendLine(string.Format("Valor: {0}", transacaoPendente1.Valor));
                stringBuilder.AppendLine(string.Format("Data: {0}", transacaoPendente1.DataHoraAutorizacao));
            }
            string result = "";
            string str = "Rede Totem";
            string str1 = stringBuilder.ToString();
            PosHelper.Logger.Info(str1);
            Task<string> task = this.showInputMsgBoxAsync(str, str1);
            await task;
            result = task.Result;
            clsCapptaClient mPinpadSDK = this.m_pinpadSDK;
            string str2 = result;
            num = (string.IsNullOrWhiteSpace(result) ? 2 : 1);
            mPinpadSDK.SendParameter(str2, num);
        }

        private void ss_showApprovedInfo(IRespostaOperacaoAprovada resposta)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (resposta.CupomCliente != null)
            {
                stringBuilder.Append(resposta.CupomCliente.Replace("\"", string.Empty)).AppendLine().AppendLine();
            }
            if (resposta.CupomLojista != null)
            {
                stringBuilder.Append(resposta.CupomLojista.Replace("\"", string.Empty)).AppendLine();
            }
            if (resposta.CupomReduzido != null)
            {
                stringBuilder.Append(resposta.CupomReduzido.Replace("\"", string.Empty)).AppendLine();
            }
            string str = JsonConvert.SerializeObject(resposta, Formatting.Indented);
            PosHelper.Logger.Info(str);
            PosHelper.Logger.Info(string.Format("--- ss_showApprovedInfo : {0}", stringBuilder));
            this.showAlertMessage("Pending Approved", stringBuilder.ToString(), AlertType.info, -1);
        }

        private void ss_showDeclinedInfo(IRespostaOperacaoRecusada resposta)
        {
            string str = string.Format("Código: {0}{1}{2}", resposta.CodigoMotivo, Environment.NewLine, resposta.Motivo);
            PosHelper.Logger.Info(str);
            this.HideAlertMessage();
        }

        private void ss_showRechargeInfo(IRespostaRecarga resposta)
        {
            string.IsNullOrEmpty(resposta.CupomCliente);
            StringBuilder stringBuilder = new StringBuilder();
            if (resposta.CupomCliente != null)
            {
                stringBuilder.Append(resposta.CupomCliente.Replace("\"", string.Empty)).AppendLine().AppendLine();
            }
            if (resposta.CupomLojista != null)
            {
                stringBuilder.Append(resposta.CupomLojista.Replace("\"", string.Empty)).AppendLine();
            }
            PosHelper.Logger.Info(string.Format("--- ss_showRechargeInfo : {0}", stringBuilder));
            this.showAlertMessage("Pending Approved", stringBuilder.ToString(), AlertType.info, -1);
        }

        public delegate Task<object> ProcessWithWaitDlg(params object[] p_params);
    }
}