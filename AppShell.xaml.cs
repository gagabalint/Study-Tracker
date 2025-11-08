namespace StudyTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("editsubject", typeof(EditSubjectPage));
            Routing.RegisterRoute("gradelsit", typeof(GradeListPage));
        }
    }
}
