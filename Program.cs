await BuildHookRunner.RunAsync(args, async payLoad =>
{
    string tempLocation = ff1.GetApplicationDataFilePath("ProjectZipTool", "RequestedPath.txt");
    string requestedLocation;
    if (ff1.FileExists(tempLocation) == false)
    {
        Console.Write("Enter The Requested Path Where To Store The Project Zips.  ");
        requestedLocation = Console.ReadLine()!;
        await ff1.WriteAllTextAsync(tempLocation, requestedLocation);
    }
    else
    {
        requestedLocation = await ff1.AllTextAsync(tempLocation);
    }

    string oldProjectFolder = payLoad.ProjectDir;

    string rootFolder = Directory.GetParent(oldProjectFolder.TrimEnd(Path.DirectorySeparatorChar))!.FullName;
    await hh1.CreateZipFileAsync(requestedLocation, payLoad.ProjectName, oldProjectFolder);
    Console.WriteLine($"Created zip file at requested {requestedLocation}.  Check this out");



});