# Application Insights Live Event Stream
This Application Insights add-on lets you view telemetry items from the browser as they are issued by Application Insights. This is helpful in situations when you need to see what is happening with your application right this very second and cannot get to Azure portal or when telemetry indexing by Application Insights pipeline is delayed. To start using.
## Step 1. Install Application Insights
This assumes you already installed Application Insights, if you haven't please follow the steps here.

## Step 2. Install ApplicationInsights.Stream Nuget
Make sure to check Pre-release option and search for ApplicationInsights.Stream in Visual Studio Nuget package manager or use "Install-Package ApplicationInsights.Stream -Pre" from Nuget Package Manager Console
![install Nuget](https://a7bp7q.dm2303.df.livefilestore.com/y3metpSHjpyFsSCkHpa6BKMLkmISoRDkXkMScvNpd39-EFJ3qT0y5hXeDCvlM_NdoOhDan6OIhtGNwWBV7zGJXamEHKNPUl0sm7PPqn5L6JEdu9yFferrgKX0EHArCMj1FGdoZlduJp-WTfO1-zT2Qw2w?width=998&height=468&cropmode=none)

## Step 3. Update aistream:key property in web.config
Open web.config
