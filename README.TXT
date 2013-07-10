A junior developer was tasked with writing a reusable implementation for a mass mailing application (the AddressProcessing project) to read and write text files that hold tab separated data. He proceeded and as a result produced the CSVReaderWriter class which can be found in the "Excercise" solution in the "AddressProcessing.CSV" namespace.
His implementation, although it works and meets the needs of the application, is of very low quality.

Your task:
     - Identify and annotate the shortcomings in the current implementation as if you were doing a code review, using comments in the CSVReaderWriterForAnnotation.cs file.
     - Refactor the CSVReaderWriter implementation into clean, elegant, rock-solid & well performing code, without over-engineering. 
     - Where you make trade offs, comment & explain.
     - Assume this code is in production and backwards compatiblity must be maintained. Therefore if you decide to change the public interface, 
       please deprecate the existing methods. Feel free to evolve the code in other ways though.   
	 - Make sure that the code can be built using the included build.bat. Please don't change the .csproj filenames - it will break our automated build.

	   
Visual Studio 2012 Solution file: .\src\Exercise.sln