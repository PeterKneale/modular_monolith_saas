# Modular Monolith SAAS Starter

## Use cases

- Base solution for a medium - large saas
- github style user / organsation model but with projects instead of repo's
- modular in nature to allow additional feature domains

## Tech Stack

- modular monolith
- choose boring technologies
    - postgres
    - ef/dapper
    - razor pages
    - htmx
    - transactional outbox

## Modules

### Users Module

- :v: Users can register
- :v: Users can login
- :v: Users can reset password
- :v: Users can update name
- :v: Users can update password
- :v: Users can create ApiKey
- :v: Users can list ApiKeys
- :v: Users can revoke ApiKeys
- :v: Users can authenticate with ApiKey

### Tenants Module

- :v: Users can create organisations
- :v: Users can be organisation administrators
- :v: Users can operate within an organisation's context
- :v: Users can add and remove members
- :v: Users can promote and demote members
- :v: Users can delete organisations
- :v: Users can create invitations
- :x: Users can accept invitations and become members
- :x: Users can reject invitations
- :v: Users can create projects
- :v: Users can operate within an projects's context
- :question: Should projects be moved to their own modules?
- :question: Should invitiations be moved out to its own module too?

### Translations Module

- :v: Users can create terms
- :v: Users can update terms
- :v: Users can delete terms
- :v: Users can add languages
- :v: Users can add translations of a term to a language
- :v: Users can update translations of a term to a language
- :v: Users can import terms
- :v: Users can import translations
- :v: Users can generate statistics on terms/translations
- :v: Users can export to RESX files
- :v: Users can export to CSV files
- :v: Route to this via a 'Project scoped module' route
- :question: Rename to 'Localisation' module

## System Considerations

- :v: Modules are well isolated, communicate via integration events
- :v: Modules contain complete functionality, razor pages / api endpoints included.
- :v: Use transactional outbox for publishing events
- :v: Publish integration events via transactional outbox
- :v: Publish queued commands via transactional outbox
- :v: In memory publishing of events between each module's outbox and another's inbox
- :x: AWS SQS for outbox/inbox (https://github.com/awslabs/aws-dotnet-messaging or similar)
- :v: Distinct container instance and composition root foreach module
- :x: Security at the postgres connection level via connection context being set

### Conventions

As the razor pages co-habitate in the web host a convention is needed to ensure the routes are globally unique.
:question: Perhaps modules could defined user, org and project scoped functionality?

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

- :v: Added PageIds to identify the current page
- :v: Organisation Selector Tests
- :v: Project Selector Tests
- :x: User Menu Tests
- :v: Basic user flow Tests
- Logs impersonation links for easier manual retesting of scenarios

# Other

## Context

- Endpoint
    - Endpoint provides a context accessor for modules to retreive the context of a command or query
    - Context is retrieved via route parameters and authentication details

```shell
dotnet ef dbcontext scaffold "Username=admin;Password=password;Database=db;Host=localhost;Port=5432;Search Path=translate;Include Error Detail=true;Log Parameters=true" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  -o Infrastructure/temp \
  -c "Db"
```

todo:

- remove all schema prefixes from application layer
- remove all ef queries from application layer
- look into SqlMapper.AddTypeHandler(LanguageIdTypeHandler.Default);

### Saas Ideas

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