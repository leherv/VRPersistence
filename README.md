# VR-Persistence

## Purpose
The projects purpose is creating a microservice for saving releases of different media (e.g. Manga Capters, TV-Show Episodes, ...).
The API will also offer the possibility to query the releases.
There will be another microservice responsible for scraping different websites and searching for the newest releases or calling APIs where possible and then calling VR-Persistence to persist those releases.

## How to setup
You need at least Docker Engine Release 18.06.0 (due to docker-compose file format v3.7) and docker-compose installed.
Be sure to check out the .env file. The sensible parts of the service are configured via the environment. Normally the .env file would not be part of
the repository (I know), but as everything runs locally Docker and is not accessible from outside, there is no problem with it. If you want to change the setup you could for example remove postgres from the docker-compose.yaml and run your own database locally.
You would just have to update the variables in the .env file accordingly.

1. Execute *docker-compose up* 
    * builds the images, spins up the containers, reads the .env file and starts the containers

For debugging without attaching to the container I also added launchSettings.json under /Properties. These would also normally not be included in the repository for security reasons but due to the already mentioned circumstances it it is ok.
You can run the ASP.NET Core API as the environment variables are simply pasted into the file (only *VRPersistenceDbHost* needs to be exchanged to localhost if you still want to use the postgres container).

## How to use
VR-Persistence exposes its API on localhost:${VRPersistenceApiPort}. At the moment it is possible to call:
* AddRelease

Under /resources there is a Postman-Collection with example requests which can be used to test the service. Just dont forget to update the port in accordance with ${VRPersistenceApiPort}.

## 12 Factor App
See: https://12factor.net/

### 1. Codebase
Satisfied as this app is under version control with git and there is only one codebase.

### 2. Dependencies
As this service is dockerized all dependencies are declared and there is no implicit reliance on system tools or libraries.

### 3. Config
Strict separation of config from code:
All sensible config parts are expected to be stored in the environment. This is achieved by creating .env files and putting them into the same directory as the docker-compose.yaml.

### 4. Backing services
In this case the database is treated like an attached resource. The connectionString to the database and therefore the database itself can be exchanged by any other postgres database.
There are no code-changes to be done to achieve this. Only the configuration has to change.

### 5. Build, release, run
#### Build
Execute *docker-compose build*
This creates the image for the ASP.NET Core service as well as for the Postgres Database.
#### release
Correct .env file is in the same directory as the docker-compose file
This could be done manually on the server where you want to deploy the service or for example by loading a .env file in the deployment process before deploying to the destination. For example with Secure Files (Azure Devops).
#### run
Execute *docker-compose run*
This spins up containers for the services and loads the .env file for them which configures the containers. With each docker-compose run another .env file can be used.

### 6. Processes
This service is stateless and shares nothing. Any data that needs to persist is stored in the attached database.

### 7. Port binding
The service exports HTTP as a service by binding to localhost:5000 and listening to incoming requests. It is completely self-contained and does not rely on runtime injection of a webserver into the execution environment to create a web-facing service.

### 8. Concurrency
By using e.g. Docker Swarm it would be possible to achieve horizontal scaling.

### 9. Disposability
*docker stop vrpersistence_api_x* can be used to stop the container for the web service
*docker stop vrpersistence_db_x* can be used to stop the container for the database

### 10. Dev/prod parity
The developer can test if the deployed app will work at any time as he can run it under the exact same conditions it would run anywhere else, thanks to Docker. This will keep the time between deploys low.
The deploy itself should also be no problem for the developer as the development setup is essentially the same as the production setup.
Due to Docker the versions and types of the backing services will be the same for all developers and environments.

### 11. Logs
The service writes its logs directly to stdout. It does not concern itself with storage or routing of its output stream and instead leaves this open for the execution environment to decide.

### 12. Admin processes


 