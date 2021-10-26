using MicroPOS;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.RestAPI;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using WindowsInput;

namespace MicroPOS.View
{
    public partial class pagePayIPS01 : Page
    {
        public delegate void numvaluefunctioncall(string Numvalue);
        private event numvaluefunctioncall numformFunctionPointer;

        public pagePayIPS01()
        {
            this.InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.OnPageLoaded);
            //_txtCPF.Text = CPFData;
        }

        public static string CPFData { get; set; }

        private void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this._txtCPF.Text = "";
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            //if (mainWindow != null)
            //{
            //    //mainWindow.GotoHome(true);
            //   
            //}

            //2020-01-16 Tao Change to Back
            if (mainWindow != null && mainWindow.m_naviService != null && mainWindow.m_naviService.CanGoBack)
            {
                mainWindow.m_naviService.GoBack();
            }


        }

        private void _btnInput_Click(object sender, RoutedEventArgs e)
        {
            this._txtCPF.Text = "118.650.044 - 13";
        }

        private async void _btnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                CPFData = this._txtCPF.Text;
                string numberFromMaskText = PosHelper.GetNumberFromMaskText(this._txtCPF.Text);
                if (string.IsNullOrEmpty(numberFromMaskText) || numberFromMaskText.Length != 11)
                {
                    mainWindow.showDMessageBox("Insira seu número de CPF.", "Informação do cartão requerida", DMessageBoxButtons.OK);
                    return;
                }
                else
                {
                    IpsInfo _ipsInfo = _buttonPanel.Tag as IpsInfo;
                    _ipsInfo.CpfNumber = numberFromMaskText;
                    MainWindow mainWindow1 = mainWindow;
                    MainWindow.ProcessWithWaitDlg processWithWaitDlg = new MainWindow.ProcessWithWaitDlg(this.GetOpenInvoiceAsync);
                    object[] objArray = new object[] { numberFromMaskText };
                    RTIpsInvoice rTIpsInvoice = (RTIpsInvoice)await mainWindow1.showDProgressBoxAsync("Request API..", processWithWaitDlg, objArray);
                    if (rTIpsInvoice != null)
                    {
                        mainWindow.HideAlertMessage();
                        this._mainGrid.Tag = rTIpsInvoice;
                        Button button = sender as Button;
                        button.Command.Execute(button.CommandParameter);
                        mainWindow.m_naviService.Navigate(new pagePayIPS02());
                    }
                    else
                    {
                        mainWindow.showAlertMessage("Rede Totem", "Nenhum cliente encontrado.", AlertType.info, -1);
                        return;
                    }
                }
            }
        }

        private async Task<object> GetOpenInvoiceAsync(params object[] p_params)
        {
            object openInvoice;
            if (p_params.Length != 0)
            {
                string pParams = (string)p_params[0];
                openInvoice = await MainWindow.RestProvider.GetOpenInvoice(pParams);
            }
            else
            {
                openInvoice = null;
            }
            return openInvoice;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this.ss_init_controls();
        }

        private void ss_init_controls()
        {
            this._txtPageSubject.Text = "";
            this._txtPageSubject.Visibility = System.Windows.Visibility.Hidden;
            this._txtCPF.Focus();

            numformFunctionPointer += new numvaluefunctioncall(NumaricKeypad);
            numPad.NumaricKeypad = numformFunctionPointer;
        }

        private void NumaricKeypad(string numValue)
        {
            _txtCPF.Focus();
            if (numValue == "Backspace")
            {
                InputSimulator sim = new InputSimulator();
                sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.BACK);
            }
            else
            {
                TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, _txtCPF, numValue));
            }
            _txtCPF.Focus();
        }
    }
}