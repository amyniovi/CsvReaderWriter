A junior developer was tasked with writing a reusable implementation for a mass mailing application (the 
AddressProcessing project) to read and write text files that hold tab separated data. They proceeded and 
as a result produced the CSVReaderWriter class which can be found in the "Exercise" solution in the 
"AddressProcessing.CSV" namespace.
The implementation, although it works and meets the needs of the application, is of very low quality.

Your task:
     - Identify and annotate the shortcomings in the current implementation as if you were doing a code 
       review, using comments in the CSVReaderWriterForAnnotation.cs file.
     - Refactor the CSVReaderWriter implementation into clean, elegant, rock-solid & well performing 
       code, without over-engineering. You are not required to update the AddressFileProcessor as part of
       this task.
     - Where you make trade offs, comment & explain why.
     - Assume this code is in production and backwards compatibility must be maintained. Therefore if you 
       decide to change the public interface, please deprecate the existing methods. Feel free to evolve 
       the code in other ways though (hint: unit tests will help here).
     - You'll notice there are some other files in the solution that aren't relevant (v2.IMailShot, 
       AddressRecord) - ignore those for now. They'll come into play at a later stage in the interview process.
     - Make sure that the code can be built using the included build.bat. It depends on msbuild/csc and nuget
       being on the path so make sure you're opening a visual studio command prompt or have it on the path. 
     - Please don't change the existing .csproj filenames - it will break our automated build.
     - You can add projects as you see fit as long as everything will build on our systems without manual 
       intervention. We'll kick off the build through build.bat
     - Stick to .NET 4.5 and a VS2013 based solution.
	 - You may introduce nuget based references to mocking or testing frameworks but do not include postsharp 
           or other proprietary dependencies.
     - Git is integral to our process but we understand that not everyone has previous experience with the tool. 
       If you're not yet familiar with Git we recommend that you read some online tutorials and use the SmartGit 
       client to get started or visit http://git-scm.com/ for the official git client.  
     - If you need a copy of visual studio you can use the free Visual Studio 2013 Community edition. 
       This edition and other options are available to download from: http://www.visualstudio.com/downloads/
	   
Visual Studio 2013 Solution file: .\src\Exercise.sln