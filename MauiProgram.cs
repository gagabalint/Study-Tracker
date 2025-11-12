using Microsoft.Extensions.Logging;
using StudyTracker.Data;
using StudyTracker.ViewModels;



namespace StudyTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
           
            builder.Services.AddSingleton<IStudyTrackerDatabase,StudyTrackerDatabase>();
            builder.Services.AddSingleton<SubjectListViewModel>();
            builder.Services.AddSingleton<SubjectListPage>();
            builder.Services.AddTransient<EditSubjectViewModel>();
            builder.Services.AddTransient<EditSubjectPage>();
            builder.Services.AddTransient<GradeListViewModel>();
            builder.Services.AddTransient<GradeListPage>();
            builder.Services.AddTransient<EditGradeViewModel>();
            builder.Services.AddTransient<EditGradePage>();
            builder.Services.AddTransient<MaterialListPage>();
            builder.Services.AddTransient<MaterialListViewModel>();
            builder.Services.AddTransient<EditMaterialViewModel>();
            builder.Services.AddTransient<EditMaterialPage>();

            return builder.Build();
        }
    }
}
