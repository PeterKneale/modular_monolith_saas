#!/bin/bash
set -e

echo "##########################################"
echo "Running unit tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-unit.yml build
docker compose --progress quiet -f docker-compose-unit.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from unit-tests

echo "##########################################"
echo "Running integration tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-integration.yml build
docker compose --progress quiet -f docker-compose-integration.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from integration-tests
  
echo "##########################################"
echo "Running acceptance tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-acceptance.yml build
docker compose --progress quiet -f docker-compose-acceptance.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from acceptance-tests