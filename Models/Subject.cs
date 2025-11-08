using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace StudyTracker.Models
{
    [Table("subjects")]
    public partial class Subject:ObservableObject
    {

        [ObservableProperty]
        [property: PrimaryKey, AutoIncrement]
        int id;
        [ObservableProperty]
        [property: StringLength(100),NotNull]
        string name;
    }
}
