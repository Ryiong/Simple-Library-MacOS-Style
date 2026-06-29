using My_Library.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;

namespace My_Library.View
{
    /// <summary>
    /// Interaction logic for ViewHome.xaml
    /// </summary>
    public partial class ViewHome : UserControl
    {
        public ObservableCollection<MediaItem> MyCollection { get; set; }
        private readonly string jsonPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.json");
        public ViewHome()
        {
            InitializeComponent();
            MyCollection = new ObservableCollection<MediaItem>();
            LibraryItemsControl.ItemsSource = MyCollection;
            LoadData();
        }

        private void EmptyDisplay()
        {
            if (LibraryItemsControl.Items.Count == 0)
            {
                LibraryItemsControl.Visibility = Visibility.Collapsed;
                EmptyMessagePanel.Visibility = Visibility.Visible;    
            }
            else
            {
                LibraryItemsControl.Visibility = Visibility.Visible;   
                EmptyMessagePanel.Visibility = Visibility.Collapsed; 
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(jsonPath))
                {
                    string jsonString = File.ReadAllText(jsonPath);

                    var deserializedList = JsonSerializer.Deserialize<ObservableCollection<MediaItem>>(jsonString);

                    if (deserializedList != null)
                    {
                        MyCollection.Clear();
                        foreach (var item in deserializedList)
                        {
                            MyCollection.Add(item);
                        }
                    }
                }
                else
                {
                    SeedDefaultData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bug auto loading data: {ex.Message}", "Notification", MessageBoxButton.OK);
            }
            finally
            {
                EmptyDisplay();
            }
        }

        public void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(MyCollection, options);

                File.WriteAllText(jsonPath, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("$Bug save data: {ex.Message}");
            }
        }

        private void SeedDefaultData()
        {
            MyCollection.Add(new MediaItem
            {
                Title = "The Minimalist Book",
                Album = "Marcus Aurel",
                DateAdded = "24/06/2026",
                MediaType = "PDF",
                CoverImage = "../Resources/Cover-Placeholder.jpg"
            });

            MyCollection.Add(new MediaItem
            {
                Title = "Architecture of Tomorrow",
                Album = "Zaha Hadin",
                DateAdded = "15/05/2026",
                MediaType = "Văn bản",
                CoverImage = "../Resources/Cover-Placeholder.jpg"
            });
            SaveData();
        }
    }
}
