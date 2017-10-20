using System;
using System.IO;
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

namespace UserManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonUserCreate.IsEnabled = TextBoxUserName.Text.Length != 0 && TextBoxUserName.Text.Trim().Length != 0 && TextBoxUserEmail.Text.Length != 0 && TextBoxUserEmail.Text.Trim().Length != 0;
        }

        private void TextBoxUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonUserCreate.IsEnabled = TextBoxUserName.Text.Length != 0 && TextBoxUserName.Text.Trim().Length != 0 && TextBoxUserEmail.Text.Length != 0 && TextBoxUserEmail.Text.Trim().Length != 0;

        }

        private void ButtonUserCreate_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxUserList.Items.Contains(TextBoxUserName.Text))
            {
                MessageBox.Show("This name already exists!");
                TextBoxUserName.Text = "";
                TextBoxUserEmail.Text = "";
            }
            else
            {
               // User user = new User(){ Name = (string)TextBoxUserName.Text, Email = (string)TextBoxUserEmail.Text };
                ListBoxUserList.Items.Add(TextBoxUserName.Text);

                string[] lines = { TextBoxUserName.Text };
                File.AppendAllLines("UserList.txt", lines);

                TextBoxUserName.Text = "";
                TextBoxUserEmail.Text = "";
            }
        }

        private void ListBoxUserList_Loaded(object sender, RoutedEventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines("UserList.txt");
            int size = lines.Length;

            for (int i = 0; i < size; i++)
            {
                ListBoxUserList.Items.Add(lines[i]);

            }
        }
    }
}
