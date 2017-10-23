//Lab 5 by Johan Lång and Anders Eriksson, Göteborg 171020

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
using System.Text.RegularExpressions;

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

            /*
            const int snugContentWidth = 800;
            const int snugContentHeight = 400;

            var horizontalBorderHeight = SystemParameters.ResizeFrameHorizontalBorderHeight;
            var verticalBorderWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
            var captionHeight = SystemParameters.CaptionHeight;

            Width = snugContentWidth + 2 * verticalBorderWidth;
            Height = snugContentHeight + captionHeight + 2 * horizontalBorderHeight;
            */

        }

        private void TextBoxUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNameAndEmail();
            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem != null)
            {
                ButtonChangeUser.IsEnabled = true;
            }
        }

        private void TextBoxUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckNameAndEmail();

        }

        public void CheckNameAndEmail()
        {
            ButtonUserCreate.IsEnabled = TextBoxUserName.Text.Length != 0 && TextBoxUserName.Text.Trim().Length != 0 && TextBoxUserEmail.Text.Length != 0 && TextBoxUserEmail.Text.Trim().Length != 0;

            CheckBoxAdmin.IsEnabled = TextBoxUserName.Text.Length != 0 && TextBoxUserName.Text.Trim().Length != 0 && TextBoxUserEmail.Text.Length != 0 && TextBoxUserEmail.Text.Trim().Length != 0;

            ButtonChangeUser.IsEnabled = false;
            ButtonDeleteUser.IsEnabled = false;
            ButtonMoveToUser.IsEnabled = false;
            ButtonMoveToAdmin.IsEnabled = false;

            LabelUserInfo.Content = "";
        }

        List<User> userlist = new List<User>();

        private void ButtonUserCreate_Click(object sender, RoutedEventArgs e)
        {

            CreateUser();

        }

        private void ListBoxUserList_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            string[] lines = System.IO.File.ReadAllLines("UserList.txt");
            int size = lines.Length;

            for (int i = 0; i < size; i++)
            {
                ListBoxUserList.Items.Add(lines[i]);

            }
            */
        }

        private void ListBoxUserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((User)ListBoxUserList.SelectedItem != null)
            {
                LabelUserInfo.Content = "Name: " + ((User)ListBoxUserList.SelectedItem).Name + "\n" + "Email: " +
                                        ((User)ListBoxUserList.SelectedItem).Email;

                TextBoxUserName.Text = ((User)ListBoxUserList.SelectedItem).Name;
                TextBoxUserEmail.Text = ((User)ListBoxUserList.SelectedItem).Email;
                ButtonMoveToAdmin.IsEnabled = true;
                ButtonMoveToUser.IsEnabled = false;

            }

            else
                LabelUserInfo.Content = string.Empty;

            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem != null)
            {
                ButtonChangeUser.IsEnabled = true;
                ButtonDeleteUser.IsEnabled = true;
            }


            //LabelUserInfo.Content = ListBoxUserList.SelectedItem;
        }

        private void ButtonDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            for (int i = ListBoxUserList.Items.Count - 1; i >= 0; i--)
            {
                object[] itemsToRemove = new object[ListBoxUserList.SelectedItems.Count];
                ListBoxUserList.SelectedItems.CopyTo(itemsToRemove, 0);

                foreach (object item in itemsToRemove)
                {
                    ListBoxUserList.Items.Remove(item);
                    /*
                    string[] lines = System.IO.File.ReadAllLines("UserList.txt");
                    int size = lines.Length;

                    File.WriteAllLines("UserList.txt",
                    File.ReadLines("UserList.txt").Where(l => l != (string)item).ToList());
                    */
                }
            }

            for (int i = ListBoxAdminList.Items.Count - 1; i >= 0; i--)
            {
                object[] itemsToRemove = new object[ListBoxAdminList.SelectedItems.Count];
                ListBoxAdminList.SelectedItems.CopyTo(itemsToRemove, 0);

                foreach (object item in itemsToRemove)
                {
                    ListBoxAdminList.Items.Remove(item);
                    /*
                    string[] lines = System.IO.File.ReadAllLines("UserList.txt");
                    int size = lines.Length;

                    File.WriteAllLines("UserList.txt",
                    File.ReadLines("UserList.txt").Where(l => l != (string)item).ToList());
                    */
                }
            }
        }

        private void ListBoxAdminList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((User)ListBoxAdminList.SelectedItem != null)
            {
                LabelUserInfo.Content = "Name: " + ((User)ListBoxAdminList.SelectedItem).Name + "\n" + "Email: " +
                                        ((User)ListBoxAdminList.SelectedItem).Email;

                TextBoxUserName.Text = ((User)ListBoxAdminList.SelectedItem).Name;
                TextBoxUserEmail.Text = ((User)ListBoxAdminList.SelectedItem).Email;

                ButtonMoveToAdmin.IsEnabled = false;
                ButtonMoveToUser.IsEnabled = true;
            }
            else
                LabelUserInfo.Content = string.Empty;

            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem != null)
            {
                ButtonChangeUser.IsEnabled = true;
                ButtonDeleteUser.IsEnabled = true;
                ButtonMoveToUser.IsEnabled = true;
            }



        }

        private void ListBoxAdminList_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            string[] lines = System.IO.File.ReadAllLines("UserList.txt");
            int size = lines.Length;

            for (int i = 0; i < size; i++)
            {
                ListBoxAdminList.Items.Add(lines[i]);

            }
            */
        }

        private void ButtonMoveToAdmin_Click(object sender, RoutedEventArgs e)
        {
            for (int i = ListBoxUserList.Items.Count - 1; i >= 0; i--)
            {
                object[] itemsToRemove = new object[ListBoxUserList.SelectedItems.Count];
                ListBoxUserList.SelectedItems.CopyTo(itemsToRemove, 0);

                foreach (object item in itemsToRemove)
                {
                    ListBoxAdminList.Items.Add(item);
                    ListBoxUserList.Items.Remove(item);
                }
            }

        }

        private void ButtonMoveToUser_Click(object sender, RoutedEventArgs e)
        {
            for (int i = ListBoxAdminList.Items.Count - 1; i >= 0; i--)
            {
                object[] itemsToRemove = new object[ListBoxAdminList.SelectedItems.Count];
                ListBoxAdminList.SelectedItems.CopyTo(itemsToRemove, 0);

                foreach (object item in itemsToRemove)
                {
                    ListBoxUserList.Items.Add(item);


                }
            }
        }

        private void ButtonChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem == null)
            {
                ListBoxAdminList.Items.Remove((User)ListBoxUserList.SelectedItem);
                CreateUser();
            }
        }

        public void CreateUser()
        {
            string haystackName = (string)TextBoxUserName.Text;
            string patternName = @"\A[A-Z]\w{2,}";

            var matchesName = Regex.Matches(haystackName, patternName);
            int nrMatchName = matchesName.Count;

            string haystackEmail = (string)TextBoxUserEmail.Text;
            string patternEmail = @"\A[\w|\D]+[@][\w|\D]+[.][\w|\D]+";

            var matchesEmail = Regex.Matches(haystackEmail, patternEmail);
            int nrMatchEmail = matchesEmail.Count;


            //((User)ListBoxAdminList.SelectedItem).Name;
            if (ListBoxUserList.Items.Contains(TextBoxUserName.Text))

            //if ((User)(ListBoxUserList.Items.Contains)(TextBoxUserName.Text))
            //|| ListBoxAdminList.Items.Contains((string)TextBoxUserName.Text))
            {
                //Message.Content = "This name already exists!";
                MessageBox.Show("This name already exists!");
                TextBoxUserName.Text = "";
                TextBoxUserEmail.Text = "";
                CheckBoxAdmin.IsChecked = false;
            }

            /*
                        else if (nrMatchName < 1)
                            MessageBox.Show("This name is not correct!");

                        else if (nrMatchEmail < 1)
                            MessageBox.Show("This email is not correct!");
            */

            else
            {
                User user = new User(TextBoxUserName.Text, TextBoxUserEmail.Text);

                //string[] lines = { TextBoxUserName.Text, TextBoxUserEmail.Text };

                if (CheckBoxAdmin.IsChecked == false)
                    ListBoxUserList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));
                //ListBoxUserList.Items.Add(user.GetName());
                else
                    ListBoxAdminList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));

                // File.AppendAllLines("UserList.txt", lines);
                TextBoxUserName.Text = "";
                TextBoxUserEmail.Text = "";
                CheckBoxAdmin.IsChecked = false;
            }

        }
    }
}
