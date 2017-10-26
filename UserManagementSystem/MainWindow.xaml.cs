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
using System.ComponentModel;

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
            if (ListBoxUserList.Items.Count >= 2)
            {
                ButtonUserListSortAscending.IsEnabled = true;
                ButtonUserListSortDescending.IsEnabled = true;
            }

            else
            {
                ButtonUserListSortAscending.IsEnabled = false;
                ButtonUserListSortDescending.IsEnabled = false;
            }

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
            {

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
            }

            if ((User)ListBoxUserList.SelectedItem == null && ((User)ListBoxAdminList.SelectedItem == null))
            {
                ButtonMoveToAdmin.IsEnabled = false;
                ButtonMoveToUser.IsEnabled = false;
            }

            TextBoxUserName.Text = "";
            TextBoxUserEmail.Text = "";
            ButtonChangeUser.IsEnabled = false;
        }

        private void ListBoxAdminList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxAdminList.Items.Count >= 2)
            {
                ButtonAdminListSortAscending.IsEnabled = true;
                ButtonAdminListSortDescending.IsEnabled = true;
            }

            else
            {
                ButtonAdminListSortAscending.IsEnabled = false;
                ButtonAdminListSortDescending.IsEnabled = false;
            }

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

            if (ListBoxAdminList.Items.Count >= 2)
            {
                SortListBoxAscending(ListBoxAdminList);
                ButtonAdminListSortAscending.IsEnabled = false;
                ButtonAdminListSortDescending.IsEnabled = true;
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

            if (ListBoxUserList.Items.Count >= 2)
            {
                SortListBoxAscending(ListBoxUserList);
                ButtonUserListSortAscending.IsEnabled = false;
                ButtonUserListSortDescending.IsEnabled = true;
            }

        }

        private void ButtonChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if ((User)ListBoxUserList.SelectedItem != null || (User)ListBoxAdminList.SelectedItem == null)
            {

                int nrMatchName = CheckName();
                int nrMatchEmail = CheckEmail();

                List<string> userNameEmailInUserList = UserNameEmailList(ListBoxUserList);
                List<string> userNameEmailInAdminList = UserNameEmailList(ListBoxAdminList);


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
                else if (userNameEmailInUserList.Contains(TextBoxUserEmail.Text) || userNameEmailInAdminList.Contains(TextBoxUserEmail.Text))
                    MessageBox.Show("This email already exists!");

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

                List<string> userNameEmailInUserList = UserNameEmailList(ListBoxUserList);
                List<string> userNameEmailInAdminList = UserNameEmailList(ListBoxAdminList);

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

                else if (userNameEmailInUserList.Contains(TextBoxUserEmail.Text) || userNameEmailInAdminList.Contains(TextBoxUserEmail.Text))
                    MessageBox.Show("This email already exists!");

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
            string patternName = @"\A([A-Z]|[ÅÄÖ])\w{1,}";
            var matchesName = Regex.Matches(haystackName, patternName);
            int nrMatchName = matchesName.Count;
            return nrMatchName;
        }



        private int CheckEmail()
        {
            string haystackEmail = (string)TextBoxUserEmail.Text;
            string patternEmail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            var matchesEmail = Regex.Matches(haystackEmail, patternEmail);
            int nrMatchEmail = matchesEmail.Count;

            return nrMatchEmail;
        }

        private void CreateUser()
        {
            int nrMatchName = CheckName();
            int nrMatchEmail = CheckEmail();

            List<string> userNameEmailInUserList = UserNameEmailList(ListBoxUserList);
            List<string> userNameEmailInAdminList = UserNameEmailList(ListBoxAdminList);

            if (userNameEmailInUserList.Contains(TextBoxUserEmail.Text) || userNameEmailInAdminList.Contains(TextBoxUserEmail.Text))
            {
                MessageBox.Show("This email already exists!");
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
                {
                    ListBoxUserList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));

                    if (ListBoxUserList.Items.Count >= 2)
                    {
                        SortListBoxAscending(ListBoxUserList);
                        ButtonUserListSortAscending.IsEnabled = false;
                        ButtonUserListSortDescending.IsEnabled = true;
                    }
                }

                else
                {
                    ListBoxAdminList.Items.Add(new User(TextBoxUserName.Text, TextBoxUserEmail.Text));

                    if (ListBoxAdminList.Items.Count >= 2)
                    {
                        SortListBoxAscending(ListBoxAdminList);
                        ButtonAdminListSortAscending.IsEnabled = false;
                        ButtonAdminListSortDescending.IsEnabled = true;
                    }
                }

                TextBoxUserName.Text = "";
                TextBoxUserEmail.Text = "";
                CheckBoxAdmin.IsChecked = false;
            }
        }

        private List<string> UserNameEmailList(ListBox lb)
        {

            List<string> objectName = new List<string>();
            for (int i = 0; i < lb.Items.Count; i++)
            {
                objectName.Add(((User)lb.Items.GetItemAt(i)).Email);
            }

            return objectName;
        }


        private void SortListBoxAscending(ListBox lb)
        {
            List<User> users = new List<User>();
            for (int i = 0; i < lb.Items.Count; i++)
                users.Add(new User(((User)lb.Items.GetItemAt(i)).Name, ((User)lb.Items.GetItemAt(i)).Email));

            for (int i = lb.Items.Count - 1; i >= 0; i--)
                lb.Items.RemoveAt(i);

            var querySort = from user in users
                            orderby user.Name ascending
                            select user;

            foreach (var x in querySort)
                lb.Items.Add(x);
        }

        private void SortListBoxDescending(ListBox lb)
        {
            List<User> users = new List<User>();
            for (int i = 0; i < lb.Items.Count; i++)
                users.Add(new User(((User)lb.Items.GetItemAt(i)).Name, ((User)lb.Items.GetItemAt(i)).Email));

            for (int i = lb.Items.Count - 1; i >= 0; i--)
                lb.Items.RemoveAt(i);

            var querySort = from user in users
                            orderby user.Name descending
                            select user;

            foreach (var x in querySort)
                lb.Items.Add(x);
        }



        private void ButtonUserListSortAscending_Click(object sender, RoutedEventArgs e)
        {
            SortListBoxAscending(ListBoxUserList);
            ButtonUserListSortDescending.IsEnabled = true;
            ButtonUserListSortAscending.IsEnabled = false;
        }

        private void ButtonUserListSortDescending_Click(object sender, RoutedEventArgs e)
        {
            SortListBoxDescending(ListBoxUserList);
            ButtonUserListSortAscending.IsEnabled = true;
            ButtonUserListSortDescending.IsEnabled = false;
        }

        private void ButtonAdminListSortAscending_Click(object sender, RoutedEventArgs e)
        {
            SortListBoxAscending(ListBoxAdminList);
            ButtonAdminListSortDescending.IsEnabled = true;
            ButtonAdminListSortAscending.IsEnabled = false;
        }

        private void ButtonAdminListSortDescending_Click(object sender, RoutedEventArgs e)
        {
            SortListBoxDescending(ListBoxAdminList);
            ButtonAdminListSortAscending.IsEnabled = true;
            ButtonAdminListSortDescending.IsEnabled = false;
        }


        private void ListBoxUserList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ListBoxUserList, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
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
            }
        }

        private void ListBoxAdminList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ListBoxAdminList, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
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
            }
        }
    }
}

