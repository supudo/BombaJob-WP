using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using BombaJob.Database.Tables;

namespace BombaJob.Database
{
    public class BombaJobDataContext : DataContext
    {
        public BombaJobDataContext(string connectionString) : base(connectionString)
        {
        }

        public Table<Texts> Texts;
        public Table<Categories> Categories;
        public Table<JobOffers> JobOffers;
    }
}
