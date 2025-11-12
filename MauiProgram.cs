using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using QuestPDF.Infrastructure;
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
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
           


            builder.Services.AddSingleton<IStudyTrackerDatabase,StudyTrackerDatabase>();
            builder.Services.AddSingleton<SubjectListViewModel>();//singleton?
            builder.Services.AddSingleton<SubjectListPage>();//singleton?
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
            builder.Services.AddTransient<SummaryViewModel>();
            builder.Services.AddTransient<SummaryPage>();
            QuestPDF.Settings.License = LicenseType.Community;



            return builder.Build();
        }
    }
}
