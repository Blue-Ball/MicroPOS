using MicroPOS;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.RestAPI;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MicroPOS.View
{
    public partial class pagePayIPS02 : Page
    {
        public pagePayIPS02()
        {
            this.InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.OnPageLoaded);
        }

        private void _btnNo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null && mainWindow.m_naviService != null && mainWindow.m_naviService.CanGoBack)
            {
                mainWindow.m_naviService.GoBack();
            }
        }

        private void _btnYes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (((IpsInfo)this._mainGrid.Tag).Amount == 0)
                {
                    mainWindow.showAlertMessage("Rede Totem", "Escolha as faturas que deseja pagar.", AlertType.info, -1);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePayIPS03());
            }
        }

        private void _invoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool? nullable;
            if (e.AddedItems.Count <= 0)
            {
                RTFatura item = (RTFatura)e.RemovedItems[0];
                CheckBox checkBox = pagePayIPS02.FindVisualChild<CheckBox>((ListViewItem)this._invoiceList.ItemContainerGenerator.ContainerFromItem(item));
                if (checkBox != null)
                {
                    checkBox.IsChecked = new bool?(false);
                }
            }
            else
            {
                RTFatura rTFatura = (RTFatura)e.AddedItems[0];
                CheckBox checkBox1 = pagePayIPS02.FindVisualChild<CheckBox>((ListViewItem)this._invoiceList.ItemContainerGenerator.ContainerFromItem(rTFatura));
                if (checkBox1 != null)
                {
                    CheckBox checkBox2 = checkBox1;
                    bool? isChecked = checkBox1.IsChecked;
                    if (isChecked.HasValue)
                    {
                        nullable = new bool?(!isChecked.GetValueOrDefault());
                    }
                    else
                    {
                        nullable = null;
                    }
                    checkBox2.IsChecked = nullable;
                    return;
                }
            }
        }

        public static T FindVisualChild<T>(DependencyObject depObj)
        where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }
                    T t = pagePayIPS02.FindVisualChild<T>(child);
                    if (t != null)
                    {
                        return t;
                    }
                }
            }
            return default(T);
        }

        private void Invoice_Checked(object sender, RoutedEventArgs e)
        {
            IpsInfo tag = (IpsInfo)this._mainGrid.Tag;
            if (tag.Amount != 0)
            {
                this._infoTotalAmount.Visibility = System.Windows.Visibility.Visible;
                this._valTotalAmount.Visibility = System.Windows.Visibility.Visible;
                Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>().HideAlertMessage();
            }
            else
            {
                this._infoTotalAmount.Visibility = System.Windows.Visibility.Hidden;
                this._valTotalAmount.Visibility = System.Windows.Visibility.Hidden;
            }
            this._valTotalAmount.Text = PosHelper.GetCurrencyString(tag.Amount);
        }

        private void Invoice_Unchecked(object sender, RoutedEventArgs e)
        {
            IpsInfo tag = (IpsInfo)this._mainGrid.Tag;
            if (tag.Amount != 0)
            {
                this._infoTotalAmount.Visibility = System.Windows.Visibility.Visible;
                this._valTotalAmount.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this._infoTotalAmount.Visibility = System.Windows.Visibility.Hidden;
                this._valTotalAmount.Visibility = System.Windows.Visibility.Hidden;
            }
            this._valTotalAmount.Text = PosHelper.GetCurrencyString(tag.Amount);
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            IpsInfo tag = (IpsInfo)this._mainGrid.Tag;
            this._valName.Text = tag.Invoice.nome_razao;
            this._valCPF.Text = tag.Invoice.doc;
            this._infoTotalAmount.Visibility = System.Windows.Visibility.Hidden;
            this._invoiceList.ItemsSource = tag.Invoice.faturas;
        }
    }
}