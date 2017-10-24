//Lab 5 by Johan Lång and Anders Eriksson, Göteborg 171024

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
       }

        private void TextBoxUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableTextBoxes();

            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem != null)
            {
               ButtonChangeUser.IsEnabled = true;
            }
        }

        private void TextBoxUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableTextBoxes();

            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem != null)
            {
               ButtonChangeUser.IsEnabled = true;
            }

        }

        private void EnableTextBoxes()
        {
            ButtonUserCreate.IsEnabled = TextBoxUserName.Text.Length != 0 && TextBoxUserName.Text.Trim().Length != 0 && TextBoxUserEmail.Text.Length != 0 && TextBoxUserEmail.Text.Trim().Length != 0;

            CheckBoxAdmin.IsEnabled = TextBoxUserName.Text.Length != 0 && TextBoxUserName.Text.Trim().Length != 0 && TextBoxUserEmail.Text.Length != 0 && TextBoxUserEmail.Text.Trim().Length != 0;

            ButtonChangeUser.IsEnabled = false;
            ButtonDeleteUser.IsEnabled = false;
            ButtonMoveToUser.IsEnabled = false;
            ButtonMoveToAdmin.IsEnabled = false;

            LabelUserInfo.Content = "";
        }

         
        private void ButtonUserCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateUser();
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
        }

        private void ButtonDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirm delete", System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)

                for (int i = ListBoxUserList.Items.Count - 1; i >= 0; i--)
            {
                object[] itemsToRemove = new object[ListBoxUserList.SelectedItems.Count];
                ListBoxUserList.SelectedItems.CopyTo(itemsToRemove, 0);

                foreach (object item in itemsToRemove)
                {
                    ListBoxUserList.Items.Remove(item);
       
                }
            }

            for (int i = ListBoxAdminList.Items.Count - 1; i >= 0; i--)
            {
                object[] itemsToRemove = new object[ListBoxAdminList.SelectedItems.Count];
                ListBoxAdminList.SelectedItems.CopyTo(itemsToRemove, 0);

                foreach (object item in itemsToRemove)
                {
                    ListBoxAdminList.Items.Remove(item);
                 }
            }

            TextBoxUserName.Text = "";
            TextBoxUserEmail.Text = "";
            ButtonChangeUser.IsEnabled = false;
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
                    ListBoxAdminList.Items.Remove(item);
                }
            }
        }

        private void ButtonChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem == null)
            {

                int nrMatchName = CheckName();
                int nrMatchEmail = CheckEmail();

                List<string> userNamneInUnserList = UserNameList(ListBoxUserList);
                List<string> userNamneInAdminList = UserNameList(ListBoxAdminList);

       
                if (((User)ListBoxUserList.SelectedItem).Name == TextBoxUserName.Text)
                {

                    if (nrMatchEmail < 1)
                        MessageBox.Show("This email is not correct!");

                    else
                    {
                        ListBoxUserList.Items.Remove((User)ListBoxUserList.SelectedItem);
                        User user = new User(TextBoxUserName.Text, TextBoxUserEmail.Text);
                        ListBoxUserList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));
                    }


                }
                else if (userNamneInUnserList.Contains(TextBoxUserName.Text) || userNamneInAdminList.Contains(TextBoxUserName.Text))
                    MessageBox.Show("This name already exists!");

                else if (nrMatchName < 1)
                    MessageBox.Show("This name is not correct!");

                else if (nrMatchEmail < 1)
                    MessageBox.Show("This email is not correct!");

                else
                {
                    ListBoxUserList.Items.Remove((User)ListBoxUserList.SelectedItem);
                    User user = new User(TextBoxUserName.Text, TextBoxUserEmail.Text);
                    ListBoxUserList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));
                }

            }

            else if ((User)ListBoxUserList.SelectedItem == null || (User)ListBoxAdminList.SelectedItem != null)
            {
                int nrMatchName = CheckName();
                int nrMatchEmail = CheckEmail();

                List<string> userNamneInUnserList = UserNameList(ListBoxUserList);
                List<string> userNamneInAdminList = UserNameList(ListBoxAdminList);

                if (((User)ListBoxAdminList.SelectedItem).Name == TextBoxUserName.Text)
                {

                    if (nrMatchEmail < 1)
                        MessageBox.Show("This email is not correct!");

                    else
                    {
                        ListBoxAdminList.Items.Remove((User)ListBoxAdminList.SelectedItem);
                        User user = new User(TextBoxUserName.Text, TextBoxUserEmail.Text);
                        ListBoxAdminList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));
                    }
                }

                else if (userNamneInUnserList.Contains(TextBoxUserName.Text) || userNamneInAdminList.Contains(TextBoxUserName.Text))
                    MessageBox.Show("This name already exists!");

                else if (nrMatchName < 1)
                    MessageBox.Show("This name is not correct!");

                else if (nrMatchEmail < 1)
                    MessageBox.Show("This email is not correct!");

                else
                {
                    ListBoxAdminList.Items.Remove((User)ListBoxAdminList.SelectedItem);
                    User user = new User(TextBoxUserName.Text, TextBoxUserEmail.Text);
                    ListBoxAdminList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));
                }

            }

            TextBoxUserName.Text = "";
            TextBoxUserEmail.Text = "";
            CheckBoxAdmin.IsChecked = false;
            
        }

        private int CheckName()
        {
            string haystackName = (string)TextBoxUserName.Text;
            string patternName = @"\A[A-Z]\w{1,}";

            var matchesName = Regex.Matches(haystackName, patternName);
            int nrMatchName = matchesName.Count;
            return nrMatchName;
        }



        private int CheckEmail()
        {
            string haystackEmail = (string)TextBoxUserEmail.Text;
            string patternEmail = @"\A[\w]+[@][\w]+[.][\w]+";

            var matchesEmail = Regex.Matches(haystackEmail, patternEmail);
            int nrMatchEmail = matchesEmail.Count;

            return nrMatchEmail;
        }

        private void CreateUser()
        {
            int nrMatchName = CheckName();
            int nrMatchEmail = CheckEmail();

            List<string> userNamneInUnserList = UserNameList(ListBoxUserList);
            List<string> userNamneInAdminList = UserNameList(ListBoxAdminList);

            if (userNamneInUnserList.Contains(TextBoxUserName.Text) || userNamneInAdminList.Contains(TextBoxUserName.Text))
            {
                MessageBox.Show("This name already exists!");
                TextBoxUserName.Text = "";
                TextBoxUserEmail.Text = "";
                CheckBoxAdmin.IsChecked = false;
            }

            else if (nrMatchName < 1)
                MessageBox.Show("This name is not correct!");

            else if (nrMatchEmail < 1)
                MessageBox.Show("This email is not correct!");

            else
            {

                User user = new User(TextBoxUserName.Text, TextBoxUserEmail.Text);

                if (CheckBoxAdmin.IsChecked == false)
                    ListBoxUserList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));

                else
                    ListBoxAdminList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));

                TextBoxUserName.Text = "";
                TextBoxUserEmail.Text = "";
                CheckBoxAdmin.IsChecked = false;
            }
        }

        private List<string> UserNameList(ListBox lb)
        {

            List<string> objectName = new List<string>();
            for (int i = 0; i < lb.Items.Count; i++)
            {
                objectName.Add(((User)lb.Items.GetItemAt(i)).Name);
            }

            return objectName;
        }

        private List<object> UserNameListObject(ListBox lb)
        {

            List<object> objectName = new List<object>();
            for (int i = 0; i < lb.Items.Count; i++)
            {
                objectName.Add(((User)lb.Items.GetItemAt(i)));
            }

            return objectName;
        }

        private void SortDesending()
        {

            List<string> userNameInUnserList = UserNameList(ListBoxUserList);

                    var querySortUserList = from user in userNameInUnserList
                                            orderby user descending
                                        select user;

                foreach(object x in querySortUserList)
                    ListBoxUserList.Items.Add(x);
            }

    
    }
    }

