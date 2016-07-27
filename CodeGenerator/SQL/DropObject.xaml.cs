using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CodeGenerator.SQL
{
    /// <summary>
    /// Interaction logic for DropObject.xaml
    /// </summary>
    public partial class DropObject : Window
    {
        public DropObject()
        {
            InitializeComponent();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            TypeComboBox.Items.Add(new ComboBoxItem() {Content = "Procedure", Tag = 1});
            TypeComboBox.Items.Add(new ComboBoxItem() { Content = "View", Tag = 2 });
            TypeComboBox.Items.Add(new ComboBoxItem() { Content = "Table", Tag = 3 });
        }

        private void OnNameChanged(object sender, TextChangedEventArgs e)
        {
          SetResult();
        }

        private void OnTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            SetResult();
        }

        private void SetResult()
        {
            var type = ((int)((ComboBoxItem)TypeComboBox.SelectedItem).Tag);
            var query = "";
            switch (type)
            {
                case 1:
                    query = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))\n" +
                           "      DROP PROCEDURE [dbo].[{0}]\n" +
                           "GO";
                    break;
                case 2:
                    query = "IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[{0}]'))\n" +
                            "    DROP VIEW [dbo].[{0}]\n" +
                            "GO";
                    break;
                case 3:
                    query = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U'))\n" +
                            "    DROP TABLE [dbo].[{0}]\n" +
                            "GO";
                    break;
            }
            ResultTextBox.Text = string.Format(query, NameTextBox.Text);
        }

        private void OnCopyClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(ResultTextBox.Text);
            }
            catch (Exception)
            {
                
                //ignored
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
