#!/usr/bin/env sh

echo "Run integration tests with coverage"
dotnet test --collect:"XPlat Code Coverage" --verbosity=quiet --filter=Type=Integration
