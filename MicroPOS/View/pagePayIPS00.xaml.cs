using MicroPOS;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MicroPOS.View
{
    public partial class pagePayIPS00 : Page, IComponentConnector
    {
        private Label m_selLabel;

        public pagePayIPS00()
        {
            this.InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.OnPageLoaded);
        }

        private void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                mainWindow.GotoHome(true);
            }
        }

        private void _btnDown_Click(object sender, RoutedEventArgs e)
        {
            this._scrollViewer.ScrollToBottom();
        }

        private void _btnIpsTelecom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //2020-01-16 Tao Change to 1-Touch
            //if (this._mainGrid.Tag != null && !((string)this._mainGrid.Tag != "Telecom"))
            //{
            //    this._mainGrid.Tag = null;
            //    this._txtTelecom.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            //    return;
            //}
            this._mainGrid.Tag = "Telecom";
            //this._txtTelecom.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));

            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (this._mainGrid.Tag == null || string.IsNullOrEmpty((string)this._mainGrid.Tag))
                {
                    mainWindow.showDMessageBox("Escolha o Provedor de Internet que deseja realizar o pagamento.", "Informação do cartão requerida", DMessageBoxButtons.OK);
                    return;
                }
                var _ipsInfo = _buttonPanel.Tag as IpsInfo;

                _ipsInfo.PayMethod = 0;
                _ipsInfo.Company = this._mainGrid.Tag.ToString();
                mainWindow.m_naviService.Navigate(new pagePayIPS01());
            }


        }

        private void _btnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (this._mainGrid.Tag == null || string.IsNullOrEmpty((string)this._mainGrid.Tag))
                {
                    mainWindow.showDMessageBox("Escolha o Provedor de Internet que deseja realizar o pagamento.", "Informação do cartão requerida", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePayIPS01());
            }
        }

        private void _btnUp_Click(object sender, RoutedEventArgs e)
        {
            this._scrollViewer.ScrollToHome();
        }

        public T FindChild<T>(DependencyObject parent, string childName)
        where T : DependencyObject
        {
            if (parent == null)
            {
                return default(T);
            }
            T t = default(T);
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if ((T)(child as T) == null)
                {
                    t = this.FindChild<T>(child, childName);
                    if (t != null)
                    {
                        break;
                    }
                }
                else if (string.IsNullOrEmpty(childName))
                {
                    t = (T)child;
                    break;
                }
                else
                {
                    FrameworkElement frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        t = (T)child;
                        break;
                    }
                }
            }
            return t;
        }
        private void OnClick_ListItem(object sender, MouseButtonEventArgs e)
        {
            Label solidColorBrush = this.FindChild<Label>(sender as StackPanel, "_itemName");
            if (solidColorBrush != null)
            {
                this._mainGrid.Tag = (string)solidColorBrush.Content;
                solidColorBrush.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                if (this.m_selLabel != null)
                {
                    this.m_selLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
                this.m_selLabel = solidColorBrush;
            }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this.ss_init_controls();
        }

        private void ss_init_controls()
        {
            this._txtPageSubject.Text = "";
            this._txtPageSubject.Visibility = System.Windows.Visibility.Hidden;
        }

        private void ss_init_list()
        {
            if (Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>() != null)
            {
                foreach (string pSList in PosHelper.GetIPSList())
                {
                    StackPanel stackPanel = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };
                    stackPanel.MouseUp += new MouseButtonEventHandler(this.OnClick_ListItem);
                    Label label = new Label()
                    {
                        Name = "_itemName",
                        Content = pSList
                    };
                    stackPanel.Children.Add(label);
                    TextBox textBox = new TextBox()
                    {
                        Name = "_itemDesc",
                        Text = ""
                    };
                    stackPanel.Children.Add(textBox);
                    this._listIPSPanel.Children.Add(stackPanel);
                }
            }
        }
        
    }
}