## Intro

This is my solution to the line server problem for Salsify recruitment.

Note: Since this was coded on Windows 10, using .NET 5, I have included the bash scripts to build and run the application that I’ve tested under the Windows Linux Subsystem (WSL). I hope this is reliable and the scripts work.

## How does your system work? (if not addressed in comments in source)

This was a bit of a challenge for me since I am not used to working with large data files. 
My approach to the challenge was to think about the tradeoff i would have to make to make sure performance vs memory was acceptable.

Since the requirements state that a file of any size could be used the brute force approach of reading all the file contents to memory not an option.

I also looked into a read on demand, just for the sake of it, fully knowing that for large files each access had the potential to lock up the system.

Finally, I thought I would create an index of each lines’ starting position on the file instead of loading it’s content.The process consists in reading the file and saving the position of each “\n” character on a list. Each index of this list will map to a file line and the value will be the position on the file where the line starts. 
I sacrificed initial performance on the server load with this indexing process to guarantee that reading would be fast.

This system consists of a web application running on Kestrel web server. When the application starts it loads and indexes the file specified as parameter, which may take a bit of time for larger files.

Once the file is indexed, the application starts and the requested endpoint will be available. Calls to that endpoint will fetch the position where the the desired lines’ content is on the file from our index and read a line starting from that position (read until a new “\n”) and return the result. If the line number requested if out of bounds it’ll return null and that will trigger a  HTTP 413 response back.

## How will your system perform with a 1 GB file? a 10 GB file? a 100 G#B file?

With the increase of the file size the indexing process duration will go up and memory used will increase because the size of the index will also be bigger - but it will still be manageable since it’s only storing long values.

## How will your system perform with 100 users? 10000 users? 1000000 users?

I am using a FileStream to read from the file which, according to the docs (https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.-ctor?view=net-5.0#System_IO_FileStream__ctor_System_String_System_IO_FileMode_System_IO_FileAccess_System_IO_FileShare_) does not lock the file when opened for read only. Increasing the number of users will not impact the way the system behaves.

The system will always be limited by hardware capacity to respond to requests and to store the index data.

## What documentation, websites, papers, etc did you consult in doing this assignment?

I’ve used quite a bit of Googling, mostly for Kestrel server setup and stream usage and manipulation, which is not something I do often. Most of my reads were either on StackOverflow or MSDN.

## What third-party libraries or other tools does the system use? How did you choose each library or framework you used?

I am using ASP.NET CORE (.NET 5) and hosting the application on Kestrel web server, using only standard libraries.

## How long did you spend on this exercise? If you had unlimited more time to spend on this, how would you spend it and how would you prioritize each item?

I spent around 4h on the exercise. 

## If you were to critique your code, what would you have to say about it?

I think the code is simple, readable and organized. It is a very simple project which can be improved upon.

I’ve made the FileService a singleton class and passed it the index list. This means that the index will live as long as the application runs, but it will be available on a single instance of the application. A possible improvement would be to store the index on a distributed storage system that could be accessed by all instances.
