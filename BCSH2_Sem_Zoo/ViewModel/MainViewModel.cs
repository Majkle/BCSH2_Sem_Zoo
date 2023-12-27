using BCSH2_Sem_Zoo.Model.Context;
using BCSH2_Sem_Zoo.Model.Entity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace BCSH2_Sem_Zoo.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public ZooContext zooContext;

        [ObservableProperty]
        private string windowTitle;

        [ObservableProperty]
        private object? currentTable; //TOOD tzv na prasáka

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


        public MainViewModel()
        {
            zooContext = new();
            AvailableTableTypes = GetAllEntityTypes();

            LoadDatabase();
            SetWindowTitle();
            CommandManager.InvalidateRequerySuggested();
        }

        public ObservableCollection<TEntity> GetObservableCollectionForEntity<TEntity>() where TEntity : class
        {
            return zooContext.Set<TEntity>().Local.ToObservableCollection();
        }

        partial void OnSelectedTableTypeChanged(Type value)
        {
            ChangeDisplayedTable(value);
        }

        private List<Type> GetAllEntityTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && (t.Namespace == "BCSH2_Sem_Zoo.Model.Entity")).ToList();
        }

        public int GetMaxIdOfSelectedTable()
        {
            return (int)typeof(ZooContext).GetMethod(nameof(zooContext.MaxIdOfEntity))?.MakeGenericMethod(new[] { SelectedTableType }).Invoke(zooContext, null)!;
        }

        private void ChangeDisplayedTable(Type entityType)
        {
            CurrentTable = typeof(MainViewModel).GetMethod(nameof(MainViewModel.GetObservableCollectionForEntity))?.MakeGenericMethod(entityType)?.Invoke(this, null);
        }

        [RelayCommand(CanExecute = nameof(CanOpenExistingDatabase))]
        private void OpenExistingDatabase()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Databáze (*.db)|*.db|Všechny soubory (*.*)|*.*";
            ofd.RestoreDirectory = true;
            ofd.Title = "Otevření existující databáze";

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
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Databáze (*.db)|*.db|Všechny soubory (*.*)|*.*";
            sfd.RestoreDirectory = true;
            sfd.Title = "Vytvoření nové databáze";

            if (sfd.ShowDialog() == true)
            {
                OpenCreateDatabase(sfd.FileName);
            }
        }

        private bool CanOpenNewDatabase()
        {
            return !DataGridHasErrors();
        }

        private void OpenCreateDatabase(string path)
        {
            zooContext.Database.CloseConnection();
            zooContext = new();
            zooContext.ChangeDatabasePath(path);

            LoadDatabase();
            SetWindowTitle();
        }

        private void SetWindowTitle()
        {
            Regex regexMatchingDataSource = new Regex(@"(?<=Data Source=)[^;]+");
            WindowTitle = $"BCSH2 Sem - " + System.IO.Path.GetFileName(regexMatchingDataSource.Match(zooContext.Database.GetConnectionString() ?? "").Value);
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

            CurrentTable = zooContext.Animal.Local.ToObservableCollection();
            SelectedTableType = AvailableTableTypes.FirstOrDefault(typeof(Animal));
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

        public bool DataGridHasErrors()
        {
            foreach (var item in AnimalDataGrid.Items)
            {
                var row = AnimalDataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null && Validation.GetHasError(row))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
