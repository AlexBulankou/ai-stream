# Application Insights Telemetry Stream
This Application Insights add-on lets you view telemetry items from the browser as they are issued by Application Insights. This is helpful in situations when you need to see what is happening with your application right this very second and cannot get to Azure portal or when telemetry indexing by Application Insights pipeline is delayed. 
You can also see what telemetry items where sampled and what were sent as well as some instant charts.
Essentially this add-on lets you view the same data you see in your Visual Studio debug output window, except from production.

![AI Stream](https://op8emw.dm2303.df.livefilestore.com/y3m0ELKEJzOCVoH7bEOLPsqCeTSEgHAtNw-ORcjP_YQqMS0bjfJ-yBdpubd9cwXIT74IpkpeoYv3QfNuiHoqeWSsGfijp3uzVSjmM-c8zu2pOYpxDEQEdYDyiLqgAlhbfsS-vfFikTleWnpdSHB-SqoZw?width=1024&height=643&cropmode=none)

## Use at your own risk
* This add-on is not part of Application Insights offering, it is not created or supported by Microsoft

## Step 1. Install Application Insights
This assumes you already installed Application Insights, if you haven't please follow the steps [here](https://azure.microsoft.com/en-us/documentation/articles/app-insights-start-monitoring-app-health-usage/).

## Step 2. Install ApplicationInsights.Stream Nuget
Make sure to check Pre-release option and search for ApplicationInsights.Stream in Visual Studio Nuget package manager or use "Install-Package ApplicationInsights.Stream -Pre" from Nuget Package Manager Console
![install Nuget](https://a7bp7q.dm2303.df.livefilestore.com/y3metpSHjpyFsSCkHpa6BKMLkmISoRDkXkMScvNpd39-EFJ3qT0y5hXeDCvlM_NdoOhDan6OIhtGNwWBV7zGJXamEHKNPUl0sm7PPqn5L6JEdu9yFferrgKX0EHArCMj1FGdoZlduJp-WTfO1-zT2Qw2w?width=998&height=468&cropmode=none)

## Step 3. Update aistream:key property in web.config
Open web.config and change aistream:key setting to a custom value (it is set to 'test' by default). You may also consider changing path value to a custom path.
![key setting](https://0eeosw.dm2303.df.livefilestore.com/y3maNM0_Kjyb_hEI42aQJS4Afp-n3547YokcRCo6ZbLZ_gx17q0uMHcyF9agUy95AYPtxJKGh0lCpZzY8r8_SBjDAg7SitaiANPdVSrRPKC32PvciPAF3m2V6v4-ZxFSlP5HeDKgIZT3iXFfiNcwqPHkw?width=823&height=287&cropmode=none)

## Step 4. Verify and deploy
Test it locally before deploying to production. Run the application locally and open the page specified is path (default is /aistream.html. You will see a page with a textbox, enter your key into textbox and press Start.
![enter key and press Start button](https://nc2aqa.dm2303.df.livefilestore.com/y3msNCuz8nwkxxQ5kEZgtuJOHW7titQhuBuM5ewOHGLcTqSuKkvXoMoEnhpZBJUwfJr_TMODgYkZg7iZ7ivYxvZeUAu8FyuGAEEhYqkQWmytug11nczV0V9bMo7rLDH9KEd8L7FL4IBGNJP0SZD8bMR-A?width=1024&height=428&cropmode=none)
Start button should disappear - you are now intercepting all telemetry. Try opening a separate browser instance and generating some load, e.g. reloading the page. On the AI Stream page should see events that you can filter by typing in the textbox:
![AI Stream](https://op8emw.dm2303.df.livefilestore.com/y3m0ELKEJzOCVoH7bEOLPsqCeTSEgHAtNw-ORcjP_YQqMS0bjfJ-yBdpubd9cwXIT74IpkpeoYv3QfNuiHoqeWSsGfijp3uzVSjmM-c8zu2pOYpxDEQEdYDyiLqgAlhbfsS-vfFikTleWnpdSHB-SqoZw?width=1024&height=643&cropmode=none)
That's it - you are all set to use it - please share your feedback!

