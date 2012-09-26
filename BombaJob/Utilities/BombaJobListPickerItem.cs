using System;
using System.Net;

namespace BombaJob.Utilities
{
    public class BombaJobListPickerItem
    {
        public BombaJobListPickerItem()
        {
            this.ID = 0;
            this.Title = "";
        }
        public BombaJobListPickerItem(int _id, string _title)
        {
            this.ID = _id;
            this.Title = _title;
        }

        public int ID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }
    }
}
