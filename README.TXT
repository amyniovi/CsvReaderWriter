A junior developer was tasked with writing a reusable implementation for a mass mailing application (the 
AddressProcessing project) to read and write text files that hold tab separated data. They proceeded and 
as a result produced the CSVReaderWriter class which can be found in the "Excercise" solution in the 
"AddressProcessing.CSV" namespace.
The implementation, although it works and meets the needs of the application, is of very low quality.

Your task:
     - Identify and annotate the shortcomings in the current implementation as if you were doing a code 
       review, using comments in the CSVReaderWriterForAnnotation.cs file.
     - Refactor the CSVReaderWriter implementation into clean, elegant, rock-solid & well performing 
       code, without over-engineering. 
     - Where you make trade offs, comment & explain why.
     - Assume this code is in production and backwards compatiblity must be maintained. Therefore if you 
       decide to change the public interface, please deprecate the existing methods. Feel free to evolve 
       the code in other ways though (hint: unit tests will help here).
     - Make sure that the code can be built using the included build.bat. It depends on msbuild/csc being 
       on the path so make sure you're opening a visual studio command prompt or have it on the path. 
     - Please don't change the .csproj filenames - it will break our automated build.
     - You can add projects and binaries as you see fit as long as everything will build on our systems  
       without manual intervention. We'll kick off the build through build.bat
     - Stick to .NET 4.0 and a VS2012 based solution.
     - You'll notice there are some other files in the solution that aren't relevant (v2.IMailShot, 
       AddressRecord) - ignore those for now. They'll come into play at a later stage in the interview process.
     - If you need a copy of visual studio a variety of options are at: http://www.visualstudio.com/downloads/
	   
Visual Studio 2012 Solution file: .\src\Exercise.sln