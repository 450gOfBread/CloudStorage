using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CloudStorage
{
    public partial class MainWindow : Window
    {
        static FileManager fileManager;
        Binding binding = new Binding();

        DropShadowEffect loadingShadow = new DropShadowEffect();
        private string dir = "..\\Cloud\\";

        string selectedItem;

        public MainWindow()
        {
            InitializeComponent();
            FileDropControl.AllowDrop = true;
            linkBox.Effect = loadingShadow;
            InitLoadingShadow();

            RefreshFileData();
        }

        private void FileDropControl_Drop(object sender, DragEventArgs e)
        {
            linkBox.Text = "";

            string[] files = ((string[])e.Data.GetData(DataFormats.FileDrop));
            


            foreach (string file in files)
            {
                linkBox.Text += file.Split('\\').Last() + "; ";
            }

            linkBox.Text = linkBox.Text.Trim();
            linkBox.Text = linkBox.Text.Trim(';');

            
            loadingShadow.Color = TransferData(files, dir, 4096) ? Colors.LimeGreen : Colors.Crimson;
            RefreshFileData();
        }

        private bool TransferData(string[] files, string newDir, int bufferSize = 0)
        {
            try
            {

                foreach (string filePath in files)
                {
                    string[] splitPath = filePath.Split('\\');
                    string newPath = newDir + splitPath.Last();

                    FileStream sr = new FileStream(filePath, FileMode.Open);
                    FileStream sw = new FileStream(newPath, FileMode.Create);

                    if (bufferSize == 0)
                    {
                        bufferSize = 8192;

                    }else if (bufferSize < 0)
                    {
                        throw new Exception("Cannot have negative buffer size.");
                    }
                    
                    byte[] buffer = new byte[bufferSize];

                    int read;

                    while ((read = sr.Read(buffer, 0, bufferSize)) > 0)
                    {
                        sw.Write(buffer, 0, read);
                    }

                    sr.Close();
                    sw.Close();

                }
                
                return true;

            }catch(Exception e)
            {

                return false;
            }
        }

        private void InitLoadingShadow()
        {
            loadingShadow.ShadowDepth = -5;
            loadingShadow.Direction = -100;
            loadingShadow.BlurRadius = 10;
            loadingShadow.Color = Colors.White;
        }

        private void RefreshFileData()
        {
            if (fileManager == null)
            {
                fileManager = new FileManager(dir);
                binding.Source = fileManager.fileInfo;
                FileDropControl.SetBinding(ListBox.ItemsSourceProperty, binding);
                
            }
            else
            {
                fileManager.fileInfo.Refresh();
                FileDropControl.GetBindingExpression(ListView.ItemsSourceProperty).UpdateTarget();
                
            }
            
            
        }

        private void FileDropControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                linkBox.Text = item.Content.ToString();
                selectedItem = item.Content.ToString();
            }
        }

        private void FileDropControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {

                
                // KEEP FOR LATER. THIS ACTUALLY DELETES FILES
                //fileManager.DeleteFile(fileManager.GetFilePath(item.Content.ToString()));
                RefreshFileData();
            }
        }

        
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshFileData();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem != null)
            {
                // DELETE FUNCTION TOO UNSTABLE HERE
                //fileManager.DeleteFile(fileManager.GetFilePath(selectedItem));
            }
        }
    }
}
