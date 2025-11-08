using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
namespace StudyTracker.Models
{
    [Table("grades")]
    public partial class Grade:ObservableObject
    {
        [ObservableProperty]
        [property: PrimaryKey, AutoIncrement]
        int id;

        [ObservableProperty]
        
        int value;

        [ObservableProperty]
        DateTime date;
        [ObservableProperty]
        [property: Indexed]
        int subjectId;
        
    }
}
