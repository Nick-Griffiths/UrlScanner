## UrlScanner

This is a solution to an assignment I was asked to complete as part of a job application process.  The requirements can be found in Net.docx, and the source data can be found in websites.csv.

Another potential employer is using message-based integration in their system, so I've added a basic event bus implementation using RabbitMQ in order to demonstrate some familiarity.

Along with those in appsettings.json, some additional settings are needed in User Secrets to run the solution:

- DbPassword
- EventBusOptions:Password

For email sending to work, the following are also required:

- EmailOptions:FromAddress
- EmailOptions:ToAddress
- EmailOptions:Smtp:UserName
- EmailOptions:Smtp:Password