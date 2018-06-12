using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace SymlinkMaker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public partial class MainWindow : Window
	{
		public string option { get; set; }
		public MainWindow()
		{
			InitializeComponent();
			var str = "";
			str += System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName.Split(',')[0];
			str += " v";
			str += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.Title = str;

			TargetPathtb.Text = @"c:\";
			LinkPathtb.Text = @"D:\Desktop\";
			LinkNametb.Text = "SymlinkName";
			optionDirectory.IsChecked = true;
			option = optionDirectory.Content.ToString().Substring(0, 2); // "/J";
			UpdateCommandString();
		}

		private void UpdateCommandString()
		{
			UpdateSymlinkName();
			var str = "";
			str += " mklink";
			str += option;
			str += " \"" + LinkPathtb.Text + LinkNametb.Text + "\" ";
			str += " \"" + TargetPathtb.Text + "\"";
			CommandStringtb.Text = str;
		}

		private void UpdateSymlinkName()
		{
			var strlst = TargetPathtb.Text.Split('\\');
			LinkNametb.Text = strlst.ElementAt(strlst.Length - 2) + "-Symlink";
		}

		private void SelectLinkPathbtn_OnClick(object sender, RoutedEventArgs e)
		{
			SetPath(LinkPathtb);
		}

		private void SelectTargetPathbtn_OnClick(object sender, RoutedEventArgs e)
		{
			SetPath(TargetPathtb);

		}

		private void SetPath(TextBox tb)
		{
			var dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = true;
			dialog.Multiselect = false;
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok) ;
			{
				try
				{
					//if (dialog.FileName?.Substring(dialog.FileName.Length - 1) != "\\")
					tb.Text = dialog.FileName + @"\";
				}
				catch (Exception ex)
				{
					//tb.Text = dialog.FileName + "\\";
					//MessageBox.Show(ex.ToString());
					//throw;
				}
			}
			UpdateCommandString();

		}

		private void MakeLink_OnClick(object sender, RoutedEventArgs e)
		{
			if (LinkNametb.Text.Length < 1 || LinkPathtb.Text.Length < 1 || TargetPathtb.Text.Length < 1)
			{
				MessageBox.Show("Fill all required fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				var proc1 = new ProcessStartInfo();
				proc1.UseShellExecute = true;
				proc1.WorkingDirectory = @"C:\Windows\System32";
				proc1.FileName = @"C:\Windows\System32\cmd.exe";
				proc1.Verb = "runas";
				proc1.Arguments = "/c " + CommandStringtb.Text;
				proc1.WindowStyle = ProcessWindowStyle.Hidden;
				Process.Start(proc1);
				MessageBox.Show("Symlink " + LinkNametb.Text + " created.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
				throw;
			}

		}

		private void SetOptionD(object sender, RoutedEventArgs e)
		{
			SetOption("/D");
		}
		private void SetOptionH(object sender, RoutedEventArgs e)
		{
			SetOption("/H");
		}
		private void SetOptionJ(object sender, RoutedEventArgs e)
		{
			SetOption("/J");
		}
		private void SetOption(string v)
		{
			option = v;
			UpdateCommandString();
		}

		private void TopMostcb_Click(object sender, RoutedEventArgs e)
		{
			var v = sender as CheckBox;
			if (v.IsChecked.Value) Topmost = true;
			else this.Topmost = false;
		}

		private void InputTextBoxClicked(TextBox textBox)
		{
			textBox.SelectAll();
			//textBox.Text = "";
		}

		private void TargetPathtb_GotFocus(object sender, RoutedEventArgs e)
		{
			InputTextBoxClicked(sender as TextBox);
		}

		private void LinkPathtb_GotFocus(object sender, RoutedEventArgs e)
		{
			InputTextBoxClicked(sender as TextBox);
		}

		private void LinkNametb_GotFocus(object sender, RoutedEventArgs e)
		{
			InputTextBoxClicked(sender as TextBox);
		}

		private void LinkPathtb_LostFocus(object sender, RoutedEventArgs e)
		{
			UpdateCommandString();
		}

		private void TargetPathtb_LostFocus(object sender, RoutedEventArgs e)
		{
			UpdateCommandString();
		}

		private void LinkNametb_LostFocus(object sender, RoutedEventArgs e)
		{
			UpdateCommandString();
		}

		private void Aboutbtn_Click(object sender, RoutedEventArgs e)
		{
			var str = "";
			str += "SymlinkMaker v 1.0\n";
			str += "(c)2018 KubaMiszcz\n";
			str += "mailto: zielonyeufor@gmail.com\n";
			MessageBox.Show(str, "About", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
		}

		private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
		{
			TextBox tb = (sender as TextBox);
			if (tb != null)
			{
				if (!tb.IsKeyboardFocusWithin)
				{
					e.Handled = true;
					tb.Focus();
				}
			}
		}
	}
}
