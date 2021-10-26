using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MicroPOS
{
    public class TaskManagerForm
    {
        public TaskManagerForm()
        {
        }

        public static void InvokeControlAction<T>(T control, Action<T> action)
        where T : UIElement
        {
            if (control.Dispatcher.CheckAccess())
            {
                action(control);
                return;
            }
            control.Dispatcher.Invoke(new Action<T, Action<T>>(TaskManagerForm.InvokeControlAction<T>), new object[] { control, action });
        }

        public static void InvokeControlActionAsync<T>(T control, Action<T> action)
        where T : UIElement
        {
            if (control.Dispatcher.CheckAccess())
            {
                action(control);
                return;
            }
            control.Dispatcher.Invoke(new Action<T, Action<T>>(TaskManagerForm.InvokeControlActionAsync<T>), new object[] { control, action });
        }

        public static void Start(Action action)
        {
            Task.Factory.StartNew(action);
        }
    }
}