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

### Tenants Module

- :v: Users can register
- :v: Users can login
- :v: Users can create organisations
- :v: Users can be organisation administrators
- :v: Users can operate within an organisation's context
- :x: Users can invite other users to join organisations they administer
- :x: Users can accept invitations and become members
- :x: Users can reject invitations
- :v: Users can create projects within organisations
- :v: Users can operate within an projects's context

### Translations Module

- :v: Users can create terms
- :v: Users can update terms
- :v: Users can delete terms
- :v: Users can add languages
- :v: Users can add translations of a term to a language
- :x: Users can export to RESX files

# Routes
:question: should modules have defined user, org and project scoped functionality? or should the modular nature be confined to the backend of the modular monolith?

- User Scoped modules

  :computer: `/users/{user}/{module}/{path}`
  :computer: `/users/peter/tasks/list
  

- Org Scoped modules

  :computer: `/orgs/{org}/{module}/{path}`
  :computer: `/ogs/microsoft/invitations/list

- Project Scoped modules

  :computer: `/orgs/{org}/projects/{project}/{module}/{path}`
  :computer: `/ogs/microsoft/projects/landing-page/translations/terms/list
  :computer: `/ogs/microsoft/projects/landing-page/translations/en-au/list
