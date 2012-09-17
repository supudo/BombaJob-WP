using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BombaJob.Database;
using BombaJob.Database.Tables;

namespace BombaJob.Database.ViewModel
{
    public class BombaJobViewModel : INotifyPropertyChanged
    {
        private BombaJobDataContext bjDB;

        public BombaJobViewModel(string bjDBConnectionString)
        {
            bjDB = new BombaJobDataContext(bjDBConnectionString);
        }

        private ObservableCollection<Texts> _allTexts;
        public ObservableCollection<Texts> AllTexts
        {
            get { return _allTexts; }
            set
            {
                _allTexts = value;
                NotifyPropertyChanged("AllTexts");
            }
        }

        public void SaveChangesToDB()
        {
            bjDB.SubmitChanges();
        }

        public void LoadCollectionsFromDatabase()
        {
            var textsInDB = from Texts texts in bjDB.Texts select texts;
            AllTexts = new ObservableCollection<Texts>(textsInDB);
        }

        /*
        public void AddText(ToDoItem newToDoItem)
        {
            // Add a to-do item to the data context.
            toDoDB.Items.InsertOnSubmit(newToDoItem);

            // Save changes to the database.
            toDoDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            AllToDoItems.Add(newToDoItem);

            // Add a to-do item to the appropriate filtered collection.
            switch (newToDoItem.Category.Name)
            {
                case "Home":
                    HomeToDoItems.Add(newToDoItem);
                    break;
                case "Work":
                    WorkToDoItems.Add(newToDoItem);
                    break;
                case "Hobbies":
                    HobbiesToDoItems.Add(newToDoItem);
                    break;
                default:
                    break;
            }
        }

        // Remove a to-do task item from the database and collections.
        public void DeleteToDoItem(ToDoItem toDoForDelete)
        {

            // Remove the to-do item from the "all" observable collection.
            AllToDoItems.Remove(toDoForDelete);

            // Remove the to-do item from the data context.
            toDoDB.Items.DeleteOnSubmit(toDoForDelete);

            // Remove the to-do item from the appropriate category.   
            switch (toDoForDelete.Category.Name)
            {
                case "Home":
                    HomeToDoItems.Remove(toDoForDelete);
                    break;
                case "Work":
                    WorkToDoItems.Remove(toDoForDelete);
                    break;
                case "Hobbies":
                    HobbiesToDoItems.Remove(toDoForDelete);
                    break;
                default:
                    break;
            }

            // Save changes to the database.
            toDoDB.SubmitChanges();
        }
        */

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
