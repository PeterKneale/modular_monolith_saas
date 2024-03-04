# MicroSAAS Platform

## Use cases
- Base project for a small - medium saas
- github style useer / organsation model but with projects instead of repo's
- modular in nature to allow additional feature domains 

## Tech Stack
- modular monolith
- choose boring technologies
  - postgres
  - dapper
  - razor pages
  - htmx
  - transactional outbox

## Modules

### Ideas
- Monitoring
  - Check HTTP Endpoint health
  - Check IP Address health
  - Check DNS resolution
- Image resizing
  - Thumbnail generation
- PDF generation
  - HTML to PDF
  - URL to PDF
- Feature switches
  - Manual
  - User based
  - Time based
  - Percentage based
  - Random
- Content Management
  - Data Types
  - Tags
  - Search / List / Filter
- Organisation Invitations
- Project Invitations
- Notifications (Email, SMS, Push)
- Analytics
- Metrics
  - Counters
  - Gauges
  - Timers 
  - Embeddable via URL / Iframe
- Security
  - User permissions
  - User role
  - User groups

### Tenants Module

- :v: Users can register
- :v: Users can login
- :v: Users can create ApiKey
- :v: Users can list ApiKeys
- :x: Users can revoke ApiKeys
- :v: Users can create organisations
- :v: Users can be organisation administrators
- :v: Users can operate within an organisation's context
- :x: Users can invite other users to join organisations they administer
- :x: Users can accept invitations and become members
- :x: Users can reject invitations
- :v: Users can create projects within organisations
- :v: Users can operate within an projects's context
- :question: Should organisations and projects be moved to their own modules?
- :question: Should invitiations be moved out to its own module too?

### Translations Module

- :v: Users can create terms
- :v: Users can update terms
- :v: Users can delete terms
- :v: Users can add languages
- :v: Users can add translations of a term to a language
- :x: Users can export to RESX files
- :x: Route to this via a 'Project scoped module' route
- :x: Rename to 'Localisation' module

## System Considerations
- :x: Publish integration events via transactional outbox
- :x: Publish queued commands via transactional outbox
- :x: Security at the postgres connection level via connection context being set
 
### Routes
:question: should modules have defined user, org and project scoped functionality? or should the modular nature be confined to the backend of the modular monolith?

- User Scoped modules
  - :computer: `/users/{user}/{module}/{path}`
  - :computer: `/users/peter/tasks/list`
  
- Org Scoped modules
  - :computer: `/orgs/{org}/{module}/{path}`
  - :computer: `/ogs/microsoft/invitations/list`

- Project Scoped modules
  - :computer: `/orgs/{org}/projects/{project}/{module}/{path}`
  - :computer: `/ogs/microsoft/projects/landing-page/localisation/terms/list`
  - :computer: `/ogs/microsoft/projects/landing-page/localisation/translations/en-au/list`

# Acceptance Tests
- Added PageIds to identify the current page
- :v: Organisation Selector Tests
- :v: Project Selector Tests
- :x: User Menu Tests
- Logs impersonation links for easier manual retesting of scenarios

# Architecture

## Context
- Endpoint
  - Endpoint provides a context accessor for modules to retreive the context of a command or query
  - Context is retrieved via route parameters and authentication details

```shell
dotnet ef dbcontext scaffold "Username=admin;Password=password;Database=db;Host=localhost;Port=5432;Search Path=tenants,translate;Include Error Detail=true;Log Parameters=true" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  -o Infrastructure/Database \
  -c "Db"
```