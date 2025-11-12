namespace StudyTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("editsubject", typeof(EditSubjectPage));
            Routing.RegisterRoute("gradelist", typeof(GradeListPage));
            Routing.RegisterRoute("editgrade", typeof(EditGradePage));
            Routing.RegisterRoute("materiallist", typeof(MaterialListPage));
            Routing.RegisterRoute("editmaterial", typeof(EditMaterialPage));
            Routing.RegisterRoute("summary", typeof(SummaryPage));
        }
    }
}
