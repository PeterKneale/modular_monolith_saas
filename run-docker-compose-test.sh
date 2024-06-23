#!/bin/bash
set -e

echo "##########################################"
echo "Running unit tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-unit-tests.yml build
docker compose --progress quiet -f docker-compose-unit-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from unit-tests

echo "##########################################"
echo "Running integration tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-integration-tests.yml build
docker compose --progress quiet -f docker-compose-integration-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from integration-tests
  
echo "##########################################"
echo "Running system tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-system-tests.yml build
docker compose --progress quiet -f docker-compose-system-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from system-tests
  
echo "##########################################"
echo "Running acceptance tests"
echo "##########################################"
docker compose --progress quiet -f docker-compose-acceptance-tests.yml build
docker compose --progress quiet -f docker-compose-acceptance-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from acceptance-tests