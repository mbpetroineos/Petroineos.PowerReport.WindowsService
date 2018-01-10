# Petroineos.PowerReport.WindowsService
Petroineos Coding Challenge


Fetch missing packages from nuget

Run the Petroineos.PowerReport.ConsoleHost project to host the service in a console app 

or 

navigate to the output (eg bin\dedbug) directory of Petroineos.PowerReport.WindowsService and run Install.Cmd to install
it as a windows service. Then launch Services, find Petroineos.PowerReportService and start it.


By default the files and the logs will be saved to c:\temp. 
Modify the appSettings\path value and log4net\file value in App.config to change this




(Assumptions.txt)

Assumptions made 

That the purpose of this is to produce files showing aggregate positions for a given peiod - not aggregate positions for the 
day. That is, it does not maintain a running total across periods.

Assumed use is that the generated files are consumed by a downstream system

If GetTrades errors then we save an empty file rather than produce zeros for each period. That way a downstream system can handle it 
rather than assume that there was no activity in that period.

There are no more than 24 periods in a day

Assume that interval can't be less than 1 minute nor more than 1 day
