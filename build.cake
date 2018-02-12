#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.WebDeploy"
var target = Argument("target", "Default");

var siteName = "cake-demo";
var deployPassword = EnvironmentVariable("DEPLOY_PASSWORD");
var publishDir = "./Published/";

Task("Clean")
	.Does(() => 
	{
		CleanDirectory(publishDir);
		DotNetCoreClean("./aspnetcore-cake.sln");
	});

Task("Restore")
	.IsDependentOn("Clean")
	.Does(() => 
	{
		DotNetCoreRestore("./aspnetcore-cake.sln");
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() => 
	{
		DotNetCoreBuild("./aspnetcore-cake.sln");
	});

Task("Test")
	.IsDependentOn("Build")
    .Does(() =>
	{
		var projectFiles = GetFiles("./*.Tests/*.csproj");
		foreach(var file in projectFiles)
		{
			DotNetCoreTest(file.FullPath);
		}
	});

Task("Publish")
	.IsDependentOn("Test")
	.Does(() => 
	{
		var settings = new DotNetCorePublishSettings
		{
			OutputDirectory = publishDir
		};
		DotNetCorePublish("./ProjectToBuild/ProjectToBuild.csproj", settings);
	});

Task("Deploy")
	.IsDependentOn("Publishit s")
    .Does(() =>
    {
        DeployWebsite(new DeploySettings()
        {
            SourcePath = publishDir,
            SiteName = siteName,
            ComputerName = "https://" + siteName + ".scm.azurewebsites.net:443/msdeploy.axd?site=" + siteName,
            Username = "$" + siteName,
            Password = deployPassword
        });
    });

Task("Default")
	.IsDependentOn("Deploy")
	.Does(() =>
	{
	  Information("Congratulations, your build is done!");
	});

RunTarget(target);