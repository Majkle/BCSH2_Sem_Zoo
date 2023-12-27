using BCSH2_Sem_Zoo.Model.Context;
using BCSH2_Sem_Zoo.Model.Entity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BCSH2_Sem_Zoo.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private ZooContext zooContext;

        #region Properties
        [ObservableProperty]
        private string windowTitle;

        [ObservableProperty]
        private object? currentTable;

        [ObservableProperty]
        private string absolutePath;

        [ObservableProperty]
        private Type selectedTableType;

        [ObservableProperty]
        private List<Type> availableTableTypes;

        [ObservableProperty]
        private DataGrid animalDataGrid;

        [ObservableProperty]
        private bool controlsEnabled = true;
        #endregion

        public MainViewModel()
        {
            zooContext = new();
            AvailableTableTypes = GetAllEntityTypes();

            LoadDatabase();
            SetWindowTitle();
        }

        public ObservableCollection<TEntity> GetObservableCollectionForEntity<TEntity>() where TEntity : class
        {
            return zooContext.Set<TEntity>().Local.ToObservableCollection();
        }

        public bool DataGridHasErrors()
        {
            foreach (var item in AnimalDataGrid.Items)
                if (AnimalDataGrid.ItemContainerGenerator.ContainerFromItem(item) is DataGridRow row && Validation.GetHasError(row))
                    return true;

            return false;
        }

        public int GetMaxIdOfSelectedTable()
        {
            return (int)typeof(ZooContext).GetMethod(nameof(zooContext.MaxIdOfEntity))?.MakeGenericMethod(new[] { SelectedTableType }).Invoke(zooContext, null)!;
        }

        #region Commands
        [RelayCommand(CanExecute = nameof(CanOpenExistingDatabase))]
        private void OpenExistingDatabase()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Databáze (*.db)|*.db|Všechny soubory (*.*)|*.*",
                RestoreDirectory = true,
                Title = "Otevření existující databáze"
            };

            if (ofd.ShowDialog() == true)
            {
                OpenCreateDatabase(ofd.FileName);
            }
        }

        private bool CanOpenExistingDatabase()
        {
            return !DataGridHasErrors();
        }

        [RelayCommand(CanExecute = (nameof(CanOpenNewDatabase)))]
        private void OpenNewDatabase()
        {
            SaveFileDialog sfd = new()
            {
                Filter = "Databáze (*.db)|*.db|Všechny soubory (*.*)|*.*",
                RestoreDirectory = true,
                Title = "Vytvoření nové databáze"
            };

            if (sfd.ShowDialog() == true)
            {
                OpenCreateDatabase(sfd.FileName);
            }
        }

        private bool CanOpenNewDatabase()
        {
            return !DataGridHasErrors();
        }

        [RelayCommand(CanExecute = nameof(CanInitializeData))]
        private void InitializeData()
        {
            var sql = Properties.Resources.DML;
            zooContext.Database.ExecuteSqlRaw(sql);

            LoadDatabase();
        }

        private bool CanInitializeData()
        {
            return zooContext.IsDatabaseEmpty();
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save()
        {
            zooContext.SaveChanges();
        }

        private bool CanSave()
        {
            return !DataGridHasErrors();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSelectedRow))]
        private void DeleteSelectedRow()
        {
            CurrentTable?.GetType().GetMethod("Remove")?.Invoke(CurrentTable, new object[] { AnimalDataGrid.SelectedItem });
        }

        private bool CanDeleteSelectedRow()
        {
            return !DataGridHasErrors();
        }
        #endregion

        partial void OnSelectedTableTypeChanged(Type value)
        {
            ChangeDisplayedTable(value);
        }

        private static List<Type> GetAllEntityTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && (t.Namespace == "BCSH2_Sem_Zoo.Model.Entity")).ToList();
        }

        private void SetWindowTitle()
        {
            Regex regexMatchingDataSource = new(@"(?<=Data Source=)[^;]+");
            WindowTitle = $"BCSH2 Sem - " + System.IO.Path.GetFileName(regexMatchingDataSource.Match(zooContext.Database.GetConnectionString() ?? "").Value);
        }

        private void ChangeDisplayedTable(Type entityType)
        {
            CurrentTable = typeof(MainViewModel).GetMethod(nameof(MainViewModel.GetObservableCollectionForEntity))?.MakeGenericMethod(entityType)?.Invoke(this, null);
        }

        private void OpenCreateDatabase(string path)
        {
            zooContext.Database.CloseConnection();
            zooContext = new();
            zooContext.ChangeDatabasePath(path);

            LoadDatabase();
            SetWindowTitle();
        }

        private void LoadDatabase()
        {
            zooContext.Database.EnsureCreated();
            zooContext.Animal.Load();
            zooContext.Caretaker.Load();
            zooContext.Show.Load();
            zooContext.BreedingNeed.Load();
            zooContext.BreedingNeedType.Load();
            zooContext.HistoryAnimalBreedingNeed.Load();
            zooContext.Spieces.Load();

            CurrentTable = zooContext.Animal.Local.ToObservableCollection();
            SelectedTableType = AvailableTableTypes.FirstOrDefault(typeof(Animal));
        }
    }
}
