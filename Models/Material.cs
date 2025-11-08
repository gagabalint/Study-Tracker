using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace StudyTracker.Models
{
    [Table("materials")]
    public partial class Material:ObservableObject
    {
        [ObservableProperty]
        [property:PrimaryKey,AutoIncrement]
        int id;
        [ObservableProperty]
        string pictureUri;
        [ObservableProperty]
        string description;
        [ObservableProperty]
        DateTime date;
        [ObservableProperty]
        [property:Indexed]
        int subjectId;

    }
}
