# File storage system

## Local environment configuration

To run the needed infrastructure locally, you need to execute the following command in the project root directory:
- `docker-compose -f docker/docker-compose.yaml up`

The mariadb will be available at: `localhost:3608` with credentials `root:Qwerty123`
Seq will be available at: `http://localhost:5342`


## Homework

1. Create the docker file that will pack the local solution into docker image -> `./docker/app.dockerfile` -> `docker build . -t latest`  -> **done**
2. Create the way to publish docker image into gitlab package registry. Describe it inside `readme.md` file  using gitlab syntax. -> **done**
Added image to container registry. https://docs.gitlab.com/ee/user/packages/container_registry/ -> **done**
3. Add mariadb service into the docker-compose file -> `./docker/docker-compose.yml` -> **done**
4. **MEGA HARD TASK** Try to reconfigure application logging capabilities using `Serilog`, `Serilog.AspNetCore`,`Serilog.Settings.Configuration`,`Serilog.Sinks.Console`,`Serilog.Sinks.Seq` -> **done**
5. Add ReDoc for Documenting WEB API and show demo how do you use it. -> **done**
6. Create an API that will upload image to server, and API that will return this image to user -> **done**
7. Create the way to display <<summary>> inside the Redoc. **done**
8. Make sure that you can save file to the file system. **done**
9. Rework generation of swagger docs using "not upload to git" approach using docs folder -> **done**
10. Read about SOLID. Create the application architecture how do you see it using .Api .Persistance (storage management) .Application .Domain -> (DDD https://enterprisecraftsmanship.com/ blog) Rework code to correspond the structure. -> **done**


11. Store connection string to database inside the appsettings.Development/Integration.json **done**
12. Create database in mariadb manually with the needed table and fields (you can use the fluent migrator for it). **done**
13. Read connection string to database and init DB context to read data from the database **done**
14. Create the record inside the database using API and file upload mechanism that was implemented. **done**
15. Modify docker compose file to add MinIO service `https://docs.min.io/docs/deploy-minio-on-docker-compose.html` **done**
16. Investigate SDK to upload the file into the bucket `https://docs.aws.amazon.com/AmazonS3/latest/userguide/upload-objects.html` `https://docs.min.io/docs/how-to-use-aws-sdk-for-net-with-minio-server.html` **done**
17. Create configuration for S3 API inside the `appsettings.Development.json` **done**
18. Create the service that will be used to upload the file into S3 bucket. Use IFileProvider to implement new `IS3FileProvider` to upload/download files. **done**
19. Complex task:
    * Extend existing filemetadata model with datetime when file was uploaded (using Migrations!!!)
    * Create the background worker service `https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio`, override needed method
    * Register the service inside the Setup.cs file.  (`services.AddHostedService<>`)
    * Write the logic to check if file is expired, if yes delete the file. (keeping metadata) (think what must be changed inside the metadata)
20. Rewrite solution to use bulk operations according to the new design. (Do not forget to save LoadedDate in UTC format) **done**
21. Fix TODO items to make swagger document generation dynamic 
22. Make configuration for integration.profile-me.io OIDC service (configuration will be provided later) (and the ways to get the token). 
23. Prepare list of commands that will be used to build docker image with swagger documents xml
24. Run docker image locally (it is better to use docker-compose) make sure that you use the right environment variables to do that (ENV).
