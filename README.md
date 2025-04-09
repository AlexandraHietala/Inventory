This is a play project I started in April 2025 to practice my code organization skills. I had wanted to work on an app that could organize a personal collection (figurines), so I used that as the basis. I assumed the app would be data-/storage-intensive and would primarily involve a lot of endpoints that could be called from either mobile or web frontends.

The primary things I wanted to explore were: 
1) What is the best organization strategy for files used for endpoints? What does the separation of responsibilities look like for those files?
2) How can I version endpoints for maximum backwards compatibility?
3) How can I get a very specific history of changes made to records for audit purposes?

My notes for #1...
- The flow of data in the app follows [Controller Called]>[Controller Validation Performed]>[Workflow Called]>[Workflow Validation Performed]>[Data Validation Performed]>[Data Operation Called]>[Data Passed Back to Workflow]>[Data Passed Back to Controller]>[Controller Responds].
- A controller should receive the request and hand it off to a validator to do very basic validation of the incoming request. Then it should hand the request off to a workflow for further processing. Then it should return the result of the processing to the requestor. Controllers should each handle only one type of operation for a type of item.
- A controller validator should do very basic validation of a request (does the expected format match? does it appear to be valid?) and then return the result to the controller in the form of a boolean value.
- A workflow should hand the request off to a validator to do specific validation of the incoming request. Then do the required processing (hand off to the relevant data operations as needed). Then return the result or exception to the controller. Workflows should each handle only one type of operation for a type of item.
- A workflow validator should do more advanced validation of a request (against the database as needed) and then return the end result to the workflow in the form of a boolean value.
- A data operation should accept a request and perform the necessary task against the database. A data operation should only be aware of Data Transfer Objects (DTOs). When complete, the data operation should return the results to the workflow. Data operations should each handle only one type of operation for a type of item.
- A data validator should check specific records within the database to ensure they exist and are accurate in relation to an incoming request and then return the result to the workflow in the form of a boolean value.

My notes for #2...
- Controller routes are versioned in the format of api/v1.0/<type>).
- Controller names and methods are versioned in the format of <name>V1.
- Controller, validator, workflow, operation, and model files are stored in a folder with the version (V1) for organizing multiple versions.
- To add a new api controller version, add new files with the proper versions changed into new V# folders and update Program.cs with the info for swagger.
- Using the decoration [ApiVersion("1.0", Deprecated = true)] will deprecate an API version. This will allow it to be decomissioned while the new API version (in a separate folder, with a separate number) is rolled out. When all connections are removed, the old API version can be deleted or archived.

My notes for #3...
- Using these custom SQL audit tables, I can see a clear history of EVERY change made to a record. I don't necessarily like that there is a performance concern with lots of records. Also, if there are lots of changes to the data, it could be cumbersome to keep updated. However.. there is a very clear historical record that can be summarized.
- I may want to explore having 'audit' records inserted from the code upon updates instead.

General Notes...
- Inerfaces should be used whenever possible, with the interface definition in the same file above the class. I've seen them organized separately, but I don't see a valid use case for that and it makes things more clean to store them close to the original object if there's a 1-to-1 relationship.











---------------------------------------------------------- -
