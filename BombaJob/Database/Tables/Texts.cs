using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace BombaJob.Database.Tables
{
    [Table]
    public class Texts : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _textId;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int TextId
        {
            get { return _textId; }
            set
            {
                if (_textId != value)
                {
                    NotifyPropertyChanging("TextId");
                    _textId = value;
                    NotifyPropertyChanged("TextId");
                }
            }
        }

        private string _title;
        [Column]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    NotifyPropertyChanging("Title");
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string _content;
        [Column]
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    NotifyPropertyChanging("Content");
                    _content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }

        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }
        #endregion
    }
}
