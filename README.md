# TaxDataUpload

## Setup

- The repository is checkedin with all the 'packages' and a LocalDB and can be downloaded, opened in Visual Studio (.Net 4.5), built and
  run. The database also has some sample data.
- If you do not want to use the LocalDB and want to use some other SQL Server instance, then do the following steps:
  1. Modify the 'DefaultConnection' connection string in the website web.config.
  2. Replace the tag 
  ```xml
     <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
       <parameters>
         <parameter value="v11.0" />
       </parameters>
     </defaultConnectionFactory>
     ```
     in *web.config -> entityFramework* with
     ```xml 
     <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework" /> 
     ```
  3. Copy the file *ISO4217CurrencyList.xml* from *\TaxDataUpload\TaxDataUpload\App_Data* to *c:\* and make sure the Visual Studio has 
     privileges to access c:\.
  4. Open Package Manager Console and execute the command *update-database -verbose* 2 times (very important - 2 times). This will
     create *TaxDataUploadDB* in the SQL Server, using Entity Framework Code First migrations, and populate the table ISO4217Currency
     with the ISO 4217 currency data.
  5. Check if all the Nuget packages are installed or Restore Nuget.
  
## Assumptions

1. Valid input files (.csv and .xlsx) will have a header row.
2. For input .csv, the header will be in format 'account,description,currency code,amount' (case insensitive).
3. For input .xlsx, the first 4 columns of the first row (header) will have values account, description, currency code and amount.
4. The errors of failed input records need to be displayed on the web page but no audit or log needs to be maintained.
5. Blank rows in input files will be ignored and not counted in total records.
6. The website will be run from the Visual Studio.
7. No error logs are to be maintained.

## Design & implementation

- The website has 3 menus: *Home*, *Upload Tax Data* (to upload input files and show processing results), *Transaction Data* (to show
  all the transaction data, uploaded to the database, in a paginated format with Edit and Delete options).
- When a file is uploaded, various checks to ascertain that it is a valid (.csv, .xlsx), non-empty file are done in the action filter 
  *FileTypeFilter*.
- The file is stored in the folder *Temp* of the website (refer assumption 6) and is deleted once the processing is done.
- The *FileProcessorFactory* gives the *IFileProcessor* based on the input file type. The file processors read the respective file data.
- *DataProcessor* process, validate and save input records. It does this using Parallel execution.
- A custom ValidationAttribute *CurrencyCodeCheck* validates that a given currency code is ISO 4217 compliant.
- *CacheHelper* is used to add the ISO 4217 data to the application cache during application start and to validate currency codes.
- *TaxDataUploadDB* is the DB implementation using Entity Framework code first.
- *Ninject* is used for dependency injection.
- The database has 3 tables - *ISO4217Currency* (for ISO 4217 data), *TransactionDatas* (for uploaded data), *TaxDataFileDetails* (for
  input file name and upload datetime).
- *TaxDataUpload.Tests* is the usit test project.

## Test data and results

- File upload with *>100k* records was tested with .csv and .xlsx files. Due to parallel data processing the processing is very fast but
  saving >100k records to DB takes a few seconds. 
- Sample .csv, .xlsx files with >100k records and a few bad rows are at *\TaxDataUpload\TaxDataUpload\App_Data*.
- File upload with >100k records with >29k wrong records was tested. As >29k records are displayed at once on a web page (refer
  assumption 4), the rendering takes a few minutes. Please do not use IE for this test.
