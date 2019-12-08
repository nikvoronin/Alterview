# Alterview

- Receive messages from RabbitMq server
- Sort messages by message type
- Update message in the database. Updating should be multithreaded.

## Agenda

1. Project strucutre
1. Database schema
1. WebApi
1. Import service
1. Documentation

## Installation

- Prepare Microsoft SQL Database. For example you can name it as `LocalTestDatabase`.
- Create two tables: `Sports` and `Events`. You can use files from the `db` folder.
- Add stored procedure `UpsertEvent` to the database. See `db/dbo.UpsertEvent.sql`.
- Configure `appsettings.json` for the WebApi and `appconfig.json` for the importer service.
- Build the Solution.
- Run `build/ImportService.exe` console app or implement your own one.
- Start WebApi service from `build/Alterview.Web.exe`.

## Project structure

- __/db__. Data base helper files
  - `dbo.Events.sql`. Table creation script.  The `Events` table.
  - `dbo.Sports.data.sql`. Test data for `Sports` table
  - `dbo.Sports.sql`.  Table creation script. The `Sports` table.
  - `dbo.UpsertEvent.sql`. Stored procedure for insert-or-update (upsert) database action.
- __/src__. Project source code
  - `Alterview.Core`. Core classes and interfaces. Does not depend on any networks, frameworks etc.
  - `Alterview.ImportService`. Console application that do all the import from RabbitMq and export messages to the database.
  - `Alterview.Infrastructure`. Business logic of the project components.
  - `Alterview.Web`. WebApi service based on `Kestrel` web server.
- __/tests__. Unit tests.
- __/build__. Build folder for binaries and assemblies.

## WebApi

### Sports

- `/api/sports`. Returns list of the sports with events count linked to sport.
- `/api/sports/{sport-id}/events/date/{date}`. Returns list of events by `sport-id` for the given `date`. Ex.: `/api/sports/149/events/date/2019-12-16`.

### Events

- `/api/events/{event-id}`. Returns event description by `event-id`.
