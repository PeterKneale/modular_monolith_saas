#!/bin/bash
set -e

NAME=$1
PROJECT="Micro.$NAME"
PROJECT_INTEGRATION_EVENTS="Micro.$NAME.IntegrationEvents"
UNIT_TESTS="Micro.$NAME.UnitTests"
INTEGRATION_TESTS="Micro.$NAME.IntegrationTests"
dotnet new classlib --force --name $PROJECT --output src/$PROJECT/
dotnet new classlib --force --name $PROJECT_INTEGRATION_EVENTS --output src/$PROJECT_INTEGRATION_EVENTS/
dotnet add src/$PROJECT/ reference src/$PROJECT_INTEGRATION_EVENTS/
dotnet add src/$PROJECT/ reference src/Micro.Common/

dotnet new xunit --force --name $UNIT_TESTS --output tests/$UNIT_TESTS/
dotnet new xunit --force --name $INTEGRATION_TESTS --output tests/$INTEGRATION_TESTS/
dotnet add tests/$UNIT_TESTS/ reference src/$PROJECT/
dotnet add tests/$INTEGRATION_TESTS/ reference src/$PROJECT/

dotnet sln add src/*
dotnet sln add tests/*

mkdir -p src/$PROJECT/Application
mkdir -p src/$PROJECT/Domain
mkdir -p src/$PROJECT/Infrastructure
