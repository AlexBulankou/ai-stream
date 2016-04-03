# Application Insights Telemetry Stream
This Application Insights add-on lets you view telemetry items from the browser as they are issued by Application Insights. This is helpful in situations when you need to see what is happening with your application right this very second and cannot get to Azure portal or when telemetry indexing by Application Insights pipeline is delayed. 
Essentially this add-on lets you view the same data you see in your Visual Studio debug output window, except from production.

![AI Stream](https://qpmxyw.dm2303.df.livefilestore.com/y3mFomurbXpUhn1a2j8P7DtvKu_bn4k83wyheYO_dzUczL0ntpFP6lfvNMUpr3XjPW0mpAgRwISm8D8jhB2cOrD_P0MUfKDbbUeVnoWU4QmQy0FBnlYxa0Af_oJdl4kc-dr-N56Ee_DjWVeu1WcgJM2Kw?width=1024&height=944&cropmode=none)

## Use at your own risk
* This add-on is not part of Application Insights offering, it is not created or supported by Microsoft
* It is not recommended for production applications, because of security and potential performance implications

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
![AI Stream](https://mljzbq.dm2303.df.livefilestore.com/y3mcKuN76admEn7LzG55ZVrjG46lLwegx5q_hAbcCGRZLSqMOX0W2M7KoB_K_7NwThjvHt3SoLfGNqQyPMVVO-On1PL7Lvob7OogbdOKS4ruhsOQZa4qVODrI1zstY7FdHNf_iJq25z_eWh1XnKyaan1Q?width=1024&height=685&cropmode=none)
That's it - you are all set to use it - please share your feedback!

