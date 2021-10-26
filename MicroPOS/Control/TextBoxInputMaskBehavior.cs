using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MicroPOS.Control
{
	// Token: 0x0200007F RID: 127
	public class TextBoxInputMaskBehavior : Behavior<TextBox>
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0001086C File Offset: 0x0000EA6C
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0001087E File Offset: 0x0000EA7E
		public string InputMask
		{
			get
			{
				return (string)base.GetValue(TextBoxInputMaskBehavior.InputMaskProperty);
			}
			set
			{
				base.SetValue(TextBoxInputMaskBehavior.InputMaskProperty, value);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0001088C File Offset: 0x0000EA8C
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0001089E File Offset: 0x0000EA9E
		public char PromptChar
		{
			get
			{
				return (char)base.GetValue(TextBoxInputMaskBehavior.PromptCharProperty);
			}
			set
			{
				base.SetValue(TextBoxInputMaskBehavior.PromptCharProperty, value);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600034B RID: 843 RVA: 0x000108B1 File Offset: 0x0000EAB1
		// (set) Token: 0x0600034C RID: 844 RVA: 0x000108B9 File Offset: 0x0000EAB9
		public MaskedTextProvider Provider { get; private set; }

		/// <summary>
		/// RV: Applied Dynamic Mask
		/// </summary>
		public void RefreshMask(string newString)
		{
            try
            {
				this.Provider = new MaskedTextProvider(this.InputMask, new CultureInfo("en-US"));
				this.Provider.Set(base.AssociatedObject.Text);
				this.Provider.PromptChar = this.PromptChar;
				base.AssociatedObject.Text = this.Provider.ToDisplayString();
				base.AssociatedObject.Text = newString;
				this.RefreshText(3);
			}
            catch (Exception)
            {

            }			
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000108C4 File Offset: 0x0000EAC4
		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.Loaded += this.AssociatedObjectLoaded;
			base.AssociatedObject.PreviewTextInput += this.AssociatedObjectPreviewTextInput;
			base.AssociatedObject.PreviewKeyDown += this.AssociatedObjectPreviewKeyDown;
			DataObject.AddPastingHandler(base.AssociatedObject, new DataObjectPastingEventHandler(this.Pasting));
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00010934 File Offset: 0x0000EB34
		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.Loaded -= this.AssociatedObjectLoaded;
			base.AssociatedObject.PreviewTextInput -= this.AssociatedObjectPreviewTextInput;
			base.AssociatedObject.PreviewKeyDown -= this.AssociatedObjectPreviewKeyDown;
			DataObject.RemovePastingHandler(base.AssociatedObject, new DataObjectPastingEventHandler(this.Pasting));
		}

		// Token: 0x0600034F RID: 847 RVA: 0x000109A4 File Offset: 0x0000EBA4
		private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
		{
			this.Provider = new MaskedTextProvider(this.InputMask, new CultureInfo("en-US"));
			this.Provider.Set(base.AssociatedObject.Text);
			this.Provider.PromptChar = this.PromptChar;
			base.AssociatedObject.Text = this.Provider.ToDisplayString();
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
			if (dependencyPropertyDescriptor != null)
			{
				dependencyPropertyDescriptor.AddValueChanged(base.AssociatedObject, delegate(object s, EventArgs args)
				{
					this.UpdateText();
				});
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00010A3C File Offset: 0x0000EC3C
		private void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			this.TreatSelectedText();
			int num = this.GetNextCharacterPosition(base.AssociatedObject.SelectionStart);
			if (Keyboard.IsKeyToggled(Key.Insert))
			{
				if (this.Provider.Replace(e.Text, num))
				{
					num++;
				}
			}
			else if (this.Provider.InsertAt(e.Text, num))
			{
				num++;
			}
			num = this.GetNextCharacterPosition(num);
			this.RefreshText(num);
			e.Handled = true;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		private void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				this.TreatSelectedText();
				int nextCharacterPosition = this.GetNextCharacterPosition(base.AssociatedObject.SelectionStart);
				if (this.Provider.InsertAt(" ", nextCharacterPosition))
				{
					this.RefreshText(nextCharacterPosition);
				}
				e.Handled = true;
			}
			if (e.Key == Key.Back)
			{
				if (this.TreatSelectedText())
				{
					this.RefreshText(base.AssociatedObject.SelectionStart);
				}
				else if (base.AssociatedObject.SelectionStart != 0 && this.Provider.RemoveAt(base.AssociatedObject.SelectionStart - 1))
				{
					this.RefreshText(base.AssociatedObject.SelectionStart - 1);
				}
				e.Handled = true;
			}
			if (e.Key == Key.Delete)
			{
				if (this.TreatSelectedText())
				{
					this.RefreshText(base.AssociatedObject.SelectionStart);
				}
				else if (this.Provider.RemoveAt(base.AssociatedObject.SelectionStart))
				{
					this.RefreshText(base.AssociatedObject.SelectionStart);
				}
				e.Handled = true;
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00010BC0 File Offset: 0x0000EDC0
		private void Pasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				string input = (string)e.DataObject.GetData(typeof(string));
				this.TreatSelectedText();
				int nextCharacterPosition = this.GetNextCharacterPosition(base.AssociatedObject.SelectionStart);
				if (this.Provider.InsertAt(input, nextCharacterPosition))
				{
					this.RefreshText(nextCharacterPosition);
				}
			}
			e.CancelCommand();
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00010C34 File Offset: 0x0000EE34
		private void UpdateText()
		{
			if (this.Provider.ToDisplayString().Equals(base.AssociatedObject.Text))
			{
				return;
			}
			this.SetText(this.Provider.Set(base.AssociatedObject.Text) ? this.Provider.ToDisplayString() : base.AssociatedObject.Text);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00010C98 File Offset: 0x0000EE98
		private bool TreatSelectedText()
		{
			return base.AssociatedObject.SelectionLength > 0 && this.Provider.RemoveAt(base.AssociatedObject.SelectionStart, base.AssociatedObject.SelectionStart + base.AssociatedObject.SelectionLength - 1);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		private void RefreshText(int position)
		{
			this.SetText(this.Provider.ToDisplayString());
			base.AssociatedObject.SelectionStart = position;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00010D03 File Offset: 0x0000EF03
		private void SetText(string text)
		{
			base.AssociatedObject.Text = (string.IsNullOrWhiteSpace(text) ? string.Empty : text);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00010D20 File Offset: 0x0000EF20
		private int GetNextCharacterPosition(int startPosition)
		{
			int num = this.Provider.FindEditPositionFrom(startPosition, true);
			if (num == -1)
			{
				return startPosition;
			}
			return num;
		}

		// Token: 0x040002E6 RID: 742
		public static readonly DependencyProperty InputMaskProperty = DependencyProperty.Register("InputMask", typeof(string), typeof(TextBoxInputMaskBehavior), null);

		// Token: 0x040002E7 RID: 743
		public static readonly DependencyProperty PromptCharProperty = DependencyProperty.Register("PromptChar", typeof(char), typeof(TextBoxInputMaskBehavior), new PropertyMetadata('_'));
	}
}
