using BCSH2_Sem_Zoo.Model.Attributes;
using BCSH2_Sem_Zoo.ViewModel;
using DataGridExtensions;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BCSH2_Sem_Zoo
{
    public partial class MainWindow : Window, IMainWindow
    {
        private readonly MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;

            mainViewModel.AnimalDataGrid = AnimalDataGrid;
        }

        private void AnimalDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Tooltip when cell contains error
            if (e.Column is DataGridTextColumn)
                ((DataGridBoundColumn)e.Column).EditingElementStyle = (Style)this.Resources["ErrTemplate"];
            
            // Enable validations from IDataErrorInfo
            (((DataGridBoundColumn)e.Column).Binding as Binding)!.ValidatesOnDataErrors = true;

            var descriptor = e.PropertyDescriptor as PropertyDescriptor;
            e.Column.Header = string.IsNullOrEmpty(descriptor?.DisplayName) ? e.PropertyName : descriptor.DisplayName;

            foreach (var item in descriptor!.Attributes)
            {
                if (item is DisplayFormatAttribute displayFormatAttribute)
                {
                    ((DataGridBoundColumn)e.Column).Binding.StringFormat = displayFormatAttribute.DataFormatString;
                }

                if (item is KeyAttribute)
                {
                    ((DataGridBoundColumn)e.Column).IsReadOnly = true;
                }

                // Exclude Error Column from IDataErrorInfo
                if (item is DataGridHiddenColumnAttribute)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Combobox for foreign key entities
            if (e.PropertyType.Namespace == "BCSH2_Sem_Zoo.Model.Entity")
            {
                var observableCollection = typeof(MainViewModel).GetMethod(nameof(MainViewModel.GetObservableCollectionForEntity))?.MakeGenericMethod(e.PropertyType)?.Invoke(mainViewModel, null);

                e.Column = new DataGridComboBoxColumn
                {
                    Header = e.Column.Header,
                    Width = e.Column.Width,
                    ItemsSource = observableCollection as IEnumerable,
                    SelectedValueBinding = new Binding(e.PropertyName) { ValidatesOnDataErrors = true }
                };
            }

            //Reset column filters on table switch xd
            AnimalDataGrid.SetIsAutoFilterEnabled(false);
            AnimalDataGrid.SetIsAutoFilterEnabled(true);

            if (e.PropertyType == typeof(int))
                e.Column.SetValue(DataGridFilterColumn.TemplateProperty, this.Resources["IntegerFilterWithPopupControl"]);
            else if (e.PropertyType == typeof(DateTime))
                e.Column.SetValue(DataGridFilterColumn.TemplateProperty, this.Resources["DateFilterWithPopupControl"]);
        }

        /// <summary>
        /// Auto increment of entity ID
        /// </summary>
        private void AnimalDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            int dbMaxId = mainViewModel.GetMaxIdOfSelectedTable();
            int dgMaxId = 0;

            foreach (dynamic item in AnimalDataGrid.Items)
                if (item.GetType().Namespace == "BCSH2_Sem_Zoo.Model.Entity")
                    if (item.Id > dgMaxId)
                        dgMaxId = item.Id;

            e.NewItem.GetType().GetProperty("Id")?.SetValue(e.NewItem, (dbMaxId > dgMaxId ? dbMaxId : dgMaxId) + 1);
        }

        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            EnableControlsBasedOnDataGridErrors();
        }

        private void AnimalDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            EnableControlsBasedOnDataGridErrors();
        }

        private void Tables_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            EnableControlsBasedOnDataGridErrors();
        }

        private void AnimalDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            EnableControlsBasedOnDataGridErrors();
        }

        /// <summary>
        /// Unresolved errors in data is persisted even when table is changed, resulting in NPE...
        /// Controls should be enabled based on the state of Command... Well yes, but actualy no.
        /// Serves only GUI purpose
        /// </summary>
        private void EnableControlsBasedOnDataGridErrors()
        {
            mainViewModel.ControlsEnabled = !mainViewModel.DataGridHasErrors();
        }
    }
}
